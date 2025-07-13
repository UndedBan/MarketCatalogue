using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Authentication.Domain.Enumerations;
using MarketCatalogue.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketCatalogue.Authentication.Application.Extensions;
using MarketCatalogue.Authentication.Infrastructure.Data;
using AutoMapper;
using MarketCatalogue.Presentation;
using MarketCatalogue.Presentation.Automapper;
using Hangfire;
using Hangfire.MemoryStorage;
using MarketCatalogue.Presentation.Middlewares;
using MarketCatalogue.Commerce.Application.Services;

var builder = WebApplication.CreateBuilder(args);


var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.ConnectionString.json", optional: true)
    .AddEnvironmentVariables()
    .Build();


builder.Services.ConfigureDbContexts(config);
builder.Services.ConfigureOptions(config);
builder.Services.ConfigureDependencyInjection();

builder.Services.AddIdentity();


var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ShopMappingProfile());
    mc.AddProfile(new ProductMappingProfile());
    mc.AddProfile(new CartMappingProfile());
    mc.AddProfile(new OrderMappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddHangfire(cfg => cfg.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<OrdersService>(
    "update-order-status-job",            
    os => os.UpdateOrderStatusJob(),      
    Cron.Never(),                         
    TimeZoneInfo.Local                    
);

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = Enum.GetValues<UserRoles>();

    foreach (var roleEnum in roles)
    {
        var roleName = roleEnum.ToRoleName();
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var email = "marketrep@example.com";
    var user = await userManager.FindByEmailAsync(email);

    if (user == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = "Market",
            LastName = "Rep",
            Birthday = new DateTime(1990, 1, 1)
        };

        var result = await userManager.CreateAsync(newUser, "StrongPassword123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, UserRoles.MarketRepresentative.ToRoleName());
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

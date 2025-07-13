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
                .AddJsonFile("appsettings.ConnectionString.json")
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
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage()
);
builder.Services.AddHangfireServer();

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
var app = builder.Build();
app.UseHangfireDashboard("/hangfire");
RecurringJob.AddOrUpdate<OrdersService>(
        "update-order-status-job",                 // unique job id
        os => os.UpdateOrderStatusJob(),           // method to call
        Cron.Never(),                             // schedule (e.g. every hour)
        TimeZoneInfo.Local                         // optional timezone
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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Authentication.Infrastructure.Data;
using MarketCatalogue.DependencyInjection.Helpers;
using MarketCatalogue.Shared.Domain.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketCatalogue.Extensions;

public static class IServiceCollectionExtensions
{

    public static void ConfigureDbContexts(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(
            config.GetConnectionString("AuthenticationDbContext"),
            b => b.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName)));
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()    
            .AddEntityFrameworkStores<AuthenticationDbContext>();
    }

    public static void ConfigureOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SmtpConfig>(config.GetSection("Smtp"));
        ConfigurationHelper.Initialize(config);
    }

    public static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        var modules = ModuleHelper.LoadAll();

        foreach (var module in modules)
            module.ConfigureDependencyInjection(services);
    }
}

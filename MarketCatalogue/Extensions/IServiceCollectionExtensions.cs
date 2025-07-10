using MarketCatalogue.Authentication.Domain.Entities;
using MarketCatalogue.Authentication.Infrastructure.Data;
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
}

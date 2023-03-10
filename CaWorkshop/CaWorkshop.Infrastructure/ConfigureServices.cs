using CaWorkshop.Application.Common.Interfaces;
using CaWorkshop.Infrastructure.Identity;
using CaWorkshop.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaWorkshop.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                               IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
                .AddIdentityServerJwt();
        
        services.AddScoped<IApplicationDbContext>(provider => 
                provider.GetRequiredService<ApplicationDbContext>());
        
        return services;
    }
}
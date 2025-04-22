using EfCoreDemo.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo;

public static class Services
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        return services;
    }
}
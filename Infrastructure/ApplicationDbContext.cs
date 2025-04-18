using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EfCoreDemo.Infrastructure.Conversion;

namespace EfCoreDemo.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.RegisterAllInVogenEfCoreConverters();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Scan for all IEntityTypeConfiguration<> in the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}

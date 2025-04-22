using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EfCoreDemo.Domain;
using EfCoreDemo.Infrastructure.Conversion;
using EfCoreDemo.Infrastructure.Interceptors;

namespace EfCoreDemo.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    
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

        // Apply soft delete filters for all ISoftDeleteEntity entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasQueryFilter(GetSoftDeleteFilter(entityType.ClrType));
            }
        }

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(new SoftDeleteInterceptor());

    public async Task EnsureSeededAsync()
    {
        if (Database.IsSqlServer() && (await Database.GetPendingMigrationsAsync()).Any())
        {
            await Database.MigrateAsync();
        }

        if (await Customers.AnyAsync())
        {
            return;
        }

        var databaseSeeder = new DatabaseSeeder();

        await databaseSeeder.SeedCustomersAsync(this);
    }
    
    private static LambdaExpression GetSoftDeleteFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var property = Expression.Property(parameter, nameof(ISoftDeleteEntity.IsDeleted));
        var equal = Expression.Equal(property, Expression.Constant(true));
        return Expression.Lambda(equal, parameter);
    }
}

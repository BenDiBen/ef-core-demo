using BenchmarkDotNet.Attributes;
using EfCoreDemo.Persistence;
using EfCoreDemo.Web.Customer;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Benchmarks;

[MemoryDiagnoser]
[MinColumn]
[MaxColumn]
public class JoinedQueryBenchmarks
{
    private readonly string _connectionString =
        "Server=localhost,1433;Database=EfCoreDemo;Integrated Security=true;TrustServerCertificate=true;";

    private readonly DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder = new();

    [GlobalSetup]
    public void Setup()
    {
        _optionsBuilder.UseSqlServer(_connectionString);
        using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
    }

    [Benchmark]
    public async Task<List<CustomerResponse>> GetCustomers()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        var customers = await dbContext.Customers
            .Include(c => c.Accounts.OrderBy(a => a.Number))
            .OrderBy(c => c.Name.Last)
            .ThenBy(c => c.Name.First)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();

        return customers
            .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
            .ToList();
    }

    [Benchmark]
    public async Task<List<CustomerResponse>> GetCustomersSplitQuery()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        var customers = await dbContext.Customers
            .Include(c => c.Accounts.OrderBy(a => a.Number))
            .OrderBy(c => c.Name.Last)
            .ThenBy(c => c.Name.First)
            .Take(100)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        return customers
            .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
            .ToList();
    }

    [Benchmark]
    public async Task<List<CustomerResponse>> GetCustomersProjected()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        return await dbContext.Customers
            .Include(c => c.Accounts.OrderBy(a => a.Number))
            .OrderBy(c => c.Name.Last)
            .ThenBy(c => c.Name.First)
            .Take(100)
            .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
            .ToListAsync();
    }

    [Benchmark]
    public async Task<List<CustomerResponse>> GetCustomersSplitQueryProjected()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        return await dbContext.Customers
            .Include(c => c.Accounts.OrderBy(a => a.Number))
            .OrderBy(c => c.Name.Last)
            .ThenBy(c => c.Name.First)
            .Take(100)
            .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
            .AsSplitQuery()
            .ToListAsync();
    }
}
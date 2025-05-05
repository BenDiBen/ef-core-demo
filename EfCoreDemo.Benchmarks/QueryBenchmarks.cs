using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.dotMemory;
using Dapper;
using EfCoreDemo.Domain;
using EfCoreDemo.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Benchmarks;

[DotMemoryDiagnoser]
[SimpleJob]
[InProcess]
[MemoryDiagnoser]
[MinColumn, MaxColumn]
public class QueryBenchmarks
{
    private DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder;
    private readonly FormattableString _sql =
        $"SELECT ContactDetails_Email FROM Customers WHERE MarketingPreferences_AcceptsMarketingEmails = 1 AND IsDeleted = 0";
    private readonly string _connectionString = "Server=localhost,1433;Database=EfCoreDemo;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true;";

    [GlobalSetup]
    public void Setup()
    {
        _optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _optionsBuilder.UseSqlServer(_connectionString);
    }

    [Benchmark]
    public async Task<List<EmailAddress>> GetMarketingMailList()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        var customers = await dbContext.Customers
            .ToListAsync();
        return customers
            .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
            .Select(c => c.ContactDetails.Email)
            .ToList();
    }
    
    [Benchmark]
    public async Task<List<EmailAddress>> GetMarketingMailListNoTracking()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        var customers = await dbContext.Customers
            .AsNoTracking()
            .ToListAsync();
        return customers
            .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
            .Select(c => c.ContactDetails.Email)
            .ToList();
    }

    [Benchmark]
    public async Task<List<EmailAddress>> GetMarketingMailListPreFiltered()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        var optedInCustomers = await dbContext.Customers
            .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
            .AsNoTracking()
            .ToListAsync();
        return optedInCustomers
            .Select(c => c.ContactDetails.Email)
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public async Task<List<EmailAddress>> GetMarketingMailListPreFilteredWithProjection()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        return await dbContext.Customers
            .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
            .Select(c => c.ContactDetails.Email)
            .ToListAsync();
    }
    
    [Benchmark]
    public async Task<List<EmailAddress>> GetMarketingMailListRawSql()
    {
        await using var dbContext = new ApplicationDbContext(_optionsBuilder.Options);
        var emails = await dbContext.Database
            .SqlQuery<string>(_sql)
            .ToListAsync();
        return emails
            .Select(EmailAddress.From)
            .ToList();
    }

    [Benchmark]
    public async Task<List<EmailAddress>> GetMarketingMailListDapper()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var emails = await connection.QueryAsync<string>(_sql.ToString());
        return emails
            .Select(EmailAddress.From)
            .ToList();
    }
}
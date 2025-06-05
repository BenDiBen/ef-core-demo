using System.Collections.Immutable;
using Bogus;
using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Persistence;

public class DatabaseSeeder
{
    private static int _accountNumberCounter = 100;
    private CustomerId _currentCustomerId;
    private readonly Faker<Customer> _customerFaker;
    private readonly Faker<Account> _accountFaker;
    private readonly Faker<Transaction> _transactionFaker;
    private List<AccountId> _accountIds = [];

    public DatabaseSeeder()
    {
        _accountFaker = new Faker<Account>()
            .RuleFor(c => c.Id, _ => AccountId.New())
            .RuleFor(c => c.Number, _ => AccountNumber.From((_accountNumberCounter++).ToString().PadLeft(10, '0')))
            .RuleFor(c => c.Type, f => f.PickRandom(AccountType.Cheque, AccountType.Savings, AccountType.Transaction))
            .RuleFor(c => c.BranchCode,
                f => f.PickRandom(BranchCode.Capetown, BranchCode.Durban, BranchCode.Johannesburg, BranchCode.Pretoria))
            .RuleFor(c => c.Balance, f => Money.From(f.Finance.Amount(-1e6M, 1e8M)))
            .RuleFor(c => c.OverdraftLimit, f => Money.From(f.PickRandom(1e3M, 1e4M, 1e6M)));

        _transactionFaker = new Faker<Transaction>()
            .RuleFor(c => c.Id, _ => TransactionId.New())
            .RuleFor(c => c.Credit, f => Money.From(f.Finance.Amount(50, 1e6M)))
            .RuleFor(c => c.CreditedAccountId, f => f.PickRandom(_accountIds))
            .RuleFor(c => c.DebitedAccountId, f => f.PickRandom(_accountIds))
            .RuleFor(c => c.Requested, f => f.Date.Past())
            .RuleFor(c => c.Processed, f => f.Date.Past())
            .RuleFor(c => c.CreditorReference, _ => TransactionReference.From("Blah blah"))
            .RuleFor(c => c.DebtorReference, _ => TransactionReference.From("Blah blah"));

        _customerFaker = new Faker<Customer>()
            .RuleFor(c => c.Id, _ =>
            {
                _currentCustomerId = CustomerId.New();
                return _currentCustomerId;
            })
            .RuleFor(c => c.Name, f => new CustomerName(
                GivenName.From(f.Name.FirstName()),
                LastName.From(f.Name.LastName())
            )
            {
                MiddleNames = f.Random.Bool(0.3f)
                    ? ImmutableList.Create(GivenName.From(f.Name.FirstName()))
                    : ImmutableList<GivenName>.Empty
            })
            .RuleFor(c => c.Addresses, f => Addresses.Create(
                Address.Create(
                    StreetDescription.Create(AddressLine.From(f.Address.StreetAddress())),
                    AddressLine.From(f.Address.City()),
                    PostalCode.From(f.Random.Int(0, 9999).ToString().PadLeft(4, '0')),
                    f.PickRandom(Province.Limpopo, Province.Mpumalanga, Province.Gauteng, Province.EasternCape,
                        Province.FreeState, Province.NorthernCape, Province.NorthWest, Province.WesternCape))
            ))
            .RuleFor(c => c.ContactDetails, f => new ContactDetails(
                GeneratePhoneNumber(f),
                EmailAddress.From(f.Internet.Email()),
                f.Random.Bool(0.7f) ? GeneratePhoneNumber(f) : null,
                f.Random.Bool(0.3f) ? GeneratePhoneNumber(f) : null
            ))
            .RuleFor(c => c.DemographicInfo, f => new DemographicInfo(
                DateOnly.FromDateTime(f.Date.Past(80, DateTime.Now.AddYears(-18))),
                f.PickRandom(Gender.Male, Gender.Female),
                f.PickRandom(Language.English, Language.Zulu)
            ))
            .RuleFor(c => c.MarketingPreferences, f => new MarketingPreferences(
                f.Random.Bool(),
                f.Random.Bool()
            ))
            .RuleFor(c => c.Accounts, f => _accountFaker.Generate(f.Random.Int(1, 4)));
    }

    public async Task SeedCustomersAsync(ApplicationDbContext context, int count = 10000)
    {
        if (await context.Customers.AnyAsync())
        {
            return;
        }

        var customers = await GenerateParallelEntitiesAsync(_customerFaker, count);
        
        _accountIds = customers.SelectMany(c => c.Accounts.Select(a => a.Id)).Take(10).ToList();
        
        var transactions = await GenerateParallelEntitiesAsync(_transactionFaker, count * 10);
        
        await context.Customers.AddRangeAsync(customers);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }

    private static async Task<List<T>> GenerateParallelEntitiesAsync<T>(Faker<T> faker, int count, int threadCount = 10) where T : class
    {
        var factoryTasks = Enumerable
            .Range(1, threadCount)
            .Select((_) => Task.Run(() => faker.Generate(count / threadCount)))
            .ToList();
            
        var result = (await Task.WhenAll(factoryTasks)).SelectMany(list => list).ToList();

        return result;
    }

    private static PhoneNumber GeneratePhoneNumber(Faker faker)
    {
        return PhoneNumber.From(
            $"+27 {faker.Random.Int(10, 99)} {faker.Random.Int(100, 999)} {faker.Random.Int(1000, 9999)}");
    }
}
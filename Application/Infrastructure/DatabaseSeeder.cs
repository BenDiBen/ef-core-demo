using Bogus;
using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace EfCoreDemo.Infrastructure;

public class DatabaseSeeder
{
    private static int _accountNumberCounter = 100;
    private CustomerId _currentCustomerId;
    private List<AccountId> _accountIds = [];

    public async Task SeedCustomersAsync(ApplicationDbContext context, int count = 10000)
    {
        if (await context.Customers.AnyAsync())
        {
            return;
        }

        await context.Customers.AddRangeAsync(GenerateCustomers(count));
        await context.SaveChangesAsync();
        for (var i = 0; i < 10; i++)
        {
            await context.Transactions.AddRangeAsync(GenerateTransactions());
            await context.SaveChangesAsync();
        }
    }

    private List<Account> GenerateAccounts(int count, CustomerId customerId)
    {
        return new Faker<Account>()
            .RuleFor(c => c.Id, _ =>
            {
                var accountId = AccountId.New();
                _accountIds.Add(accountId);
                return accountId;
            })
            .RuleFor(c => c.Number, _ => AccountNumber.From((_accountNumberCounter++).ToString().PadLeft(10, '0')))
            .RuleFor(c => c.Type, f => f.PickRandom(AccountType.Cheque, AccountType.Savings, AccountType.Transaction))
            .RuleFor(c => c.BranchCode,
                f => f.PickRandom(BranchCode.Capetown, BranchCode.Durban, BranchCode.Johannesburg, BranchCode.Pretoria))
            .RuleFor(c => c.Balance, f => Money.From(f.Finance.Amount(1e2M, 1e8M)))
            .RuleFor(c => c.OverdraftLimit, f => Money.From(f.PickRandom(1e3M, 1e4M, 1e6M)))
            .RuleFor(c => c.CustomerId, _ => customerId)
            .Generate(count);
    }

    public List<Transaction> GenerateTransactions(int count = 1000000)
    {
        return new Faker<Transaction>()
            .RuleFor(c => c.Id, _ => TransactionId.New())
            .RuleFor(c => c.Credit, f => Money.From(f.Finance.Amount(50, 1e6M)))
            .RuleFor(c => c.CreditedAccountId, f => f.PickRandom(_accountIds))
            .RuleFor(c => c.DebitedAccountId, f => f.PickRandom(_accountIds))
            .RuleFor(c => c.Requested, f => f.Date.Past())
            .RuleFor(c => c.Processed, f => f.Date.Past())
            .RuleFor(c => c.CreditorReference, _ => TransactionReference.From("Blah blah"))
            .RuleFor(c => c.DebtorReference, _ => TransactionReference.From("Blah blah"))
            .Generate(count);
    }

    private List<Customer> GenerateCustomers(int count)
    {
        var customerFaker = new Faker<Customer>()
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
            .RuleFor(c => c.Accounts, f => GenerateAccounts(f.Random.Int(1, 4), _currentCustomerId));

        var result = customerFaker.Generate(count);

        return result;
    }

    private static PhoneNumber GeneratePhoneNumber(Faker faker)
    {
        return PhoneNumber.From(
            $"+27 {faker.Random.Int(10, 99)} {faker.Random.Int(100, 999)} {faker.Random.Int(1000, 9999)}");
    }
}
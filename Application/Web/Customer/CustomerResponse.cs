using EfCoreDemo.Domain;

namespace EfCoreDemo.Web.Customer;

public record CustomerResponse(CustomerId Id, string FullName, List<CustomerResponse.Account> Accounts)
{
    public record Account(AccountId Id, AccountNumber Number, AccountType Type);
}
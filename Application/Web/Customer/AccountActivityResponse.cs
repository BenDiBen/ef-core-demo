using EfCoreDemo.Domain;

namespace EfCoreDemo.Web.Customer;

public record AccountActivityResponse(
    AccountId AccountId,
    AccountNumber AccountNumber,
    Money Balance,
    List<AccountActivityTransaction> Transactions);

public record AccountActivityTransaction(TransactionReference Reference, Money Credit, DateTime Requested);
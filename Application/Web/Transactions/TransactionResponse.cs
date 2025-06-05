using EfCoreDemo.Domain;

namespace EfCoreDemo.Web.Transactions;

public record TransactionResponse(
    TransactionId Id,
    AccountNumber DebitedAccount,
    AccountNumber CreditedAccount,
    Money Debit,
    Money Credit,
    TransactionReference Reference,
    DateTime Requested);
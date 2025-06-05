namespace EfCoreDemo.Domain;

public class Transaction
{
    public TransactionId Id { get; set; }
    public Money Credit { get; set; }
    public AccountId DebitedAccountId { get; set; }
    public AccountId CreditedAccountId { get; set; }
    public required TransactionReference DebtorReference { get; set; }
    public required TransactionReference CreditorReference { get; set; }
    public Money Debit => -Credit;
    public DateTime Requested { get; set; }
    public DateTime? Processed { get; set; }
    public Account DebitedAccount { get; set; }
    public Account CreditedAccount { get; set; }
}
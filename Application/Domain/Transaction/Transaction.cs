namespace EfCoreDemo.Domain;

public class Transaction : ISoftDeleteEntity
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
    public required Account DebitedAccount { get; set; }
    public required Account CreditedAccount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
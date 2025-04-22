namespace EfCoreDemo.Domain;

public class Account : ISoftDeleteEntity
{
    public AccountId Id { get; set; }
    public required AccountNumber Number { get; set; }
    public required AccountType Type { get; set; } = AccountType.Default;
    public required BranchCode BranchCode { get; set; }
    public Money Balance { get; set; }
    public Money OverdraftLimit { get; set; }
    public bool IsInExcessOfOverdraft => -Balance < OverdraftLimit;
    public ICollection<Transaction> CreditTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> DebitTransactions { get; set; } = new List<Transaction>();

    public IEnumerable<Transaction> Transactions =>
        new List<Transaction>([..CreditTransactions, ..DebitTransactions]).OrderBy(t => t.Requested);

    public DateTime Created { get; set; } =  DateTime.Now;
    public CustomerId CustomerId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
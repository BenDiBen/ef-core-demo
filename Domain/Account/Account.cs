namespace EfCoreDemo.Domain;

public class Account
{
    public AccountId Id { get; set; }
    public BranchCode BranchCode { get; set; }
    public Money Balance { get; set; }
    public Money OverdraftLimit { get; set; }
    public bool IsInExcessOfOverdraft => -Balance < OverdraftLimit;
    public ICollection<Transaction> CreditTransactions { get; set; }
    public ICollection<Transaction> DebitTransactions { get; set; }
    public IEnumerable<Transaction> Transactions => new List<Transaction>([..CreditTransactions, ..DebitTransactions]).OrderBy(t => t.Requested);
    public DateTime Created { get; set; }
    public CustomerId CustomerId { get; set; }
}
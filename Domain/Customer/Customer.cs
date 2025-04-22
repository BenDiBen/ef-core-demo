namespace EfCoreDemo.Domain;

public class Customer : ISoftDeleteEntity
{
    public CustomerId Id { get; set; }
    public required CustomerName Name { get; set; }
    public required ICollection<Account> Accounts { get; set; } = new List<Account>();
    public required Addresses Addresses { get; set; }
    public required ContactDetails ContactDetails { get; set; }
    public required DemographicInfo DemographicInfo { get; set; }
    public MarketingPreferences MarketingPreferences { get; set; } = MarketingPreferences.Default;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
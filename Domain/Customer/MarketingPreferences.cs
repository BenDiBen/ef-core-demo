namespace EfCoreDemo.Domain;

public record MarketingPreferences(
    bool AcceptsMarketingEmails,
    bool AcceptsSmsNotifications
)
{
    public static MarketingPreferences Default => new(true, true);
}

using EfCoreDemo.Domain;
using EfCoreDemo.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Marketing;

public static class GetMailListQuery
{
    public static async Task<List<EmailAddress>> Execute(ApplicationDbContext context)  {
        var customers = await context.Customers
            .ToListAsync();
        
        return customers.
            Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
            .Select(c => c.ContactDetails.Email)
            .ToList();
    }
}
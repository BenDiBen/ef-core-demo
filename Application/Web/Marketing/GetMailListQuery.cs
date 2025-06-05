using EfCoreDemo.Domain;
using EfCoreDemo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Marketing;

public static class GetMailListQuery
{
    public static Task<List<EmailAddress>> Execute(ApplicationDbContext context)
    {
        return PreFilteredWithProjection();

        async Task<List<EmailAddress>> Simple()
        {
            var customers = await context.Customers
                .ToListAsync();
            return customers
                .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
                .Select(c => c.ContactDetails.Email)
                .ToList();
        }

        async Task<List<EmailAddress>> NoTracking()
        {
            var customers = await context.Customers
                .AsNoTracking()
                .ToListAsync();
            return customers
                .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
                .Select(c => c.ContactDetails.Email)
                .ToList();
        }

        async Task<List<EmailAddress>> PreFiltered()
        {
            var optedInCustomers = await context.Customers
                .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
                .AsNoTracking()
                .ToListAsync();
            return optedInCustomers
                .Select(c => c.ContactDetails.Email)
                .ToList();
        }

        Task<List<EmailAddress>> PreFilteredWithProjection()
        {
            return context.Customers
                .Where(c => c.MarketingPreferences.AcceptsMarketingEmails)
                .Select(c => c.ContactDetails.Email)
                .ToListAsync();
        }
    }
}
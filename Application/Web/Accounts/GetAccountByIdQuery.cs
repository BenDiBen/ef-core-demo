using EfCoreDemo.Domain;
using EfCoreDemo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Accounts;

public static class GetAccountByIdQuery
{
    public static async Task<Account> Execute([FromRoute]AccountId id, ApplicationDbContext context)
    {
        return await context.Accounts
            .Include(a => a.CreditTransactions.OrderByDescending(t => t.Requested).Take(10))
            .Include(a => a.DebitTransactions.OrderByDescending(t => t.Requested).Take(10))
            .Where(a => a.Id == id)
            .FirstAsync();
    }
}
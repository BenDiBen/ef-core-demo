using EfCoreDemo.Domain;
using EfCoreDemo.Persistence;
using EfCoreDemo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Accounts;

public static class GetIsAccountOverdrawnQuery
{
    public static async Task<bool> Execute([FromRoute]AccountId id, ApplicationDbContext context)
    {
        return await context.Accounts
            .Where(a => a.Id == id)
            .Select(a => a.Balance + a.OverdraftLimit < Money.None)
            .FirstOrDefaultAsync();
    }
}
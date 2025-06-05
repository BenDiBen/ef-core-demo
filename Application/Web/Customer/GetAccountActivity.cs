using EfCoreDemo.Domain;
using EfCoreDemo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Customer;

public static class GetAccountActivityQuery
{
    public static Task<List<AccountActivityResponse>> Execute([FromRoute] CustomerId id, ApplicationDbContext context)
    {
        return context.Accounts
            .Include(a => a.DebitTransactions.OrderByDescending(t => t.Requested).Take(10))
            .OrderBy(a => a.Number)
            .Where(a => a.CustomerId == id)
            .Select(a => new AccountActivityResponse(a.Id, a.Number, a.Balance,
                a.DebitTransactions
                    .Select(t => new AccountActivityTransaction(t.DebtorReference, t.Credit, t.Requested))
                    .ToList()))
            .AsSplitQuery()
            .ToListAsync();
    }
}
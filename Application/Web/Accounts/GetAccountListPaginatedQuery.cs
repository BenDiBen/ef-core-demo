using EfCoreDemo.Domain;
using EfCoreDemo.Persistence;
using EfCoreDemo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Accounts;

public static class GetAccountListPaginatedQuery
{
    public static async Task<List<GetAccountsResult>> Execute(ApplicationDbContext context, [FromQuery]int skip = 0, [FromQuery]int take = 20, CancellationToken cancellationToken = default)
    {
        return await context.Accounts
            .OrderByDescending(a => a.Number)
            .Skip(skip)
            .Take(take)
            .Select(a => new GetAccountsResult(a.Id, a.Number))
            .ToListAsync(cancellationToken);
    }
}

public record GetAccountsResult(AccountId Id, AccountNumber Number);
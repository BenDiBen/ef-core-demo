using EfCoreDemo.Domain;
using EfCoreDemo.Persistence;
using EfCoreDemo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Transactions;

public class GetTransactionsByDebitedAccountIdQuery
{
    public static Task<List<TransactionResponse>> Execute([FromRoute] AccountId id, ApplicationDbContext context)
    {
        return ExecuteAsSplitQuery(id, context);
    }

    private static async Task<List<TransactionResponse>> ExecuteBasic(AccountId id, ApplicationDbContext context)
    {
        var transactions = await context.Transactions
            .Where(t => t.DebitedAccountId == id)
            .Include(t => t.CreditedAccount)
            .Include(t => t.DebitedAccount)
            .OrderByDescending(t => t.Requested)
            .ToListAsync();

        return transactions.Select(t => new TransactionResponse(t.Id, t.DebitedAccount.Number, t.CreditedAccount.Number,
            -t.Credit,
            t.Credit, t.DebtorReference, t.Requested)).ToList();
    }

    private static async Task<List<TransactionResponse>> ExecuteAsSplitQuery(AccountId id, ApplicationDbContext context)
    {
        var account = await context.Accounts
            .Include(a => a.DebitTransactions.OrderByDescending(t => t.Requested))
            .ThenInclude(t => t.CreditedAccount)
            .AsSplitQuery()
            .FirstAsync(a => a.Id == id);

        return account.Transactions.Select(t => new TransactionResponse(t.Id, t.DebitedAccount.Number,
            t.CreditedAccount.Number,
            -t.Credit,
            t.Credit, t.DebtorReference, t.Requested)).ToList();
    }

    private static Task<List<TransactionResponse>> ExecuteProjected(AccountId id, ApplicationDbContext context)
    {
        return context.Transactions
            .Where(t => t.DebitedAccountId == id)
            .OrderByDescending(t => t.Requested)
            .Select(t => new TransactionResponse(t.Id, t.DebitedAccount.Number, t.CreditedAccount.Number, -t.Credit,
                t.Credit, t.DebtorReference, t.Requested))
            .AsSplitQuery()
            .ToListAsync();
    }
}
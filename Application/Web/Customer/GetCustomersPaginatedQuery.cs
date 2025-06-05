using Bogus.DataSets;
using EfCoreDemo.Persistence;
using EfCoreDemo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Web.Customer;

public static class GetCustomersPaginatedQuery
{
    public static Task<List<CustomerResponse>> Execute(ApplicationDbContext context, [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        return SplitQuery();
        
        async Task<List<CustomerResponse>> Simple()
        {
            var customers = await context.Customers
                .Include(c => c.Accounts.OrderBy(a => a.Number))
                .OrderBy(c => c.Name.Last)
                .ThenBy(c => c.Name.First)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return customers
                .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                    c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
                .ToList();
        }
        
        async Task<List<CustomerResponse>> SplitQuery()
        {
            var customers = await context.Customers
                .Include(c => c.Accounts.OrderBy(a => a.Number))
                .OrderBy(c => c.Name.Last)
                .ThenBy(c => c.Name.First)
                .Skip(skip)
                .Take(take)
                .AsSplitQuery()
                .ToListAsync();

            return customers
                .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                    c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
                .ToList();
        }
        
        Task<List<CustomerResponse>> SplitQueryWithProjection()
        {
            return context.Customers
                .Include(c => c.Accounts.OrderBy(a => a.Number))
                .OrderBy(c => c.Name.Last)
                .ThenBy(c => c.Name.First)
                .Skip(skip)
                .Take(take)
                .Select(c => new CustomerResponse(c.Id, $"{c.Name.Last}, {c.Name.First}",
                    c.Accounts.Select(a => new CustomerResponse.Account(a.Id, a.Number, a.Type)).ToList()))
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
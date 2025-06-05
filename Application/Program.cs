using EfCoreDemo;
using EfCoreDemo.Persistence;
using EfCoreDemo.Web.Accounts;
using EfCoreDemo.Web.Customer;
using EfCoreDemo.Web.Marketing;
using EfCoreDemo.Web.Transactions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed database if in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.EnsureSeededAsync();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app
    .MapGroup("api/marketing")
    .MapGet("/mail-list", GetMailListQuery.Execute);

var customersGroup = app.MapGroup("api/customers");

customersGroup.MapGet("/", GetCustomersPaginatedQuery.Execute);
customersGroup.MapGet("/{id}/account-activity", GetAccountActivityQuery.Execute);

var accountsGroup = app.MapGroup("api/accounts");

accountsGroup.MapGet("/", GetAccountListPaginatedQuery.Execute);
accountsGroup.MapGet("/{id}", GetAccountByIdQuery.Execute);
accountsGroup.MapGet("/{id}/is-overdrawn", GetIsAccountOverdrawnQuery.Execute);
accountsGroup.MapGet("/{id}/debited-transactions", GetTransactionsByDebitedAccountIdQuery.Execute);


app.Run();
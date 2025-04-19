namespace EfCoreDemo.Domain;

public class Customer
{
    public CustomerId Id { get; set; }
    public CustomerName Name { get; set; }
    public ICollection<Account> Accounts { get; set; }
    public Address Address { get; set; }
}
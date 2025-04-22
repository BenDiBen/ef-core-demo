namespace EfCoreDemo.Domain;

public record Addresses
{
    protected Address? PostalInternal { get; init; }
    
    public bool IsSameAsResidential { get; }
    public Address Postal => IsSameAsResidential ? Residential : PostalInternal!;
    public required Address Residential { get; init; }

    private Addresses()
    {
        IsSameAsResidential = PostalInternal is null;
    }

    public static Addresses Create(Address residential, Address? postal = null) => new() { Residential = residential, PostalInternal = postal };
};
namespace EfCoreDemo.Domain;

public record Address
{
    public AddressLine? Suburb { get; init; }
    public required StreetDescription Street { get; init; }
    public required AddressLine City { get; init; }
    public required PostalCode PostalCode { get; init; }
    public required Province Province { get; init; }

    public static Address Create(
        StreetDescription street,
        AddressLine city,
        PostalCode postalCode,
        Province province,
        AddressLine? suburb = null) => new()
    {
        Suburb = suburb,
        Street = street,
        City = city,
        PostalCode = postalCode,
        Province = province
    };
}

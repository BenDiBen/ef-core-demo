namespace EfCoreDemo.Domain;

public record StreetDescription
{
    public required AddressLine FirstLine { get; init; }
    public AddressLine? SecondLine { get; init; }

    public static StreetDescription Create(AddressLine firstLine, AddressLine? secondLine = null) =>
        new StreetDescription
        {
            FirstLine = firstLine,
            SecondLine = secondLine
        };
}
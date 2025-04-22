using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class Province
{
    public static readonly Province Gauteng = From("Gauteng");
    public static readonly Province WesternCape = From("Western Cape");
    public static readonly Province EasternCape = From("Eastern Cape");
    public static readonly Province KwaZuluNatal = From("KwaZulu-Natal");
    public static readonly Province FreeState = From("Free State");
    public static readonly Province Limpopo = From("Limpopo");
    public static readonly Province Mpumalanga = From("Mpumalanga");
    public static readonly Province NorthWest = From("North West");
    public static readonly Province NorthernCape = From("Northern Cape");

    private static string NormalizeInput(string input) => input.Trim();
}

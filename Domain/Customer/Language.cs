using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class Language
{
    public static readonly Language English = From("en");
    public static readonly Language Afrikaans = From("af");
    public static readonly Language Zulu = From("zu");
}

using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class Language
{
    public static readonly Language English = From("en");
    public static readonly Language Afrikaans = From("af");
    public static readonly Language Zulu = From("zu");
    public static readonly Language Xhosa = From("xh");
    public static readonly Language NorthernSotho = From("ns");
    public static readonly Language Tswana = From("tn");
    public static readonly Language SouthernSotho = From("st");
    public static readonly Language Tsonga = From("ts");
    public static readonly Language Swati = From("ss");
    public static readonly Language Venda = From("ve");
    public static readonly Language Ndebele = From("nd");
}

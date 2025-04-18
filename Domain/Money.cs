using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<decimal>]
public readonly partial struct Money : IComparable<Money>
{
    public static bool operator >(Money first, Money second)
    {
        return first.Value > second.Value;
    }

    public static bool operator <(Money first, Money second)
    {
        return first.Value < second.Value;
    }
    
    public static Money operator -(Money first)
    {
        return From(-first.Value);
    }
}
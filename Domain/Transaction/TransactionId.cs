using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<Guid>]
public readonly partial struct  TransactionId
{
    public static TransactionId New() => From(Guid.NewGuid());
}
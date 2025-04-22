using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<Guid>]
public readonly partial struct  AccountId
{
    public static AccountId New() => From(Guid.NewGuid());
}
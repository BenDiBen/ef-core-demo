using System.Collections.Immutable;

namespace EfCoreDemo.Domain;

public record CustomerName(GivenName First, LastName Last)
{
    public ImmutableList<GivenName> MiddleNames { get; init; } = ImmutableList<GivenName>.Empty;
}
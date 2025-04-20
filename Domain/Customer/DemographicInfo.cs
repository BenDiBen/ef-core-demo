namespace EfCoreDemo.Domain;

public record DemographicInfo(
    DateTime DateOfBirth,
    Gender? Gender,
    Language? PreferredLanguage
)
{
    public Gender? Gender { get; init; } = Gender ?? Gender.Unknown;
    public Language? PreferredLanguage { get; init; } = PreferredLanguage ?? Language.English;
}

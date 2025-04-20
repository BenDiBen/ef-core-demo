namespace EfCoreDemo.Domain;

public record ContactDetails(
    PhoneNumber PhoneNumber,
    EmailAddress Email,
    PhoneNumber? WorkNumber,
    PhoneNumber? AlternateNumber
);

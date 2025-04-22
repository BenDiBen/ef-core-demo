using Vogen;

namespace EfCoreDemo.Infrastructure.Conversion;

[EfCoreConverter<Domain.AccountId>]
[EfCoreConverter<Domain.BranchCode>]
[EfCoreConverter<Domain.CustomerId>]
[EfCoreConverter<Domain.GivenName>]
[EfCoreConverter<Domain.LastName>]
[EfCoreConverter<Domain.TransactionId>]
[EfCoreConverter<Domain.Money>]
[EfCoreConverter<Domain.AddressLine>]
[EfCoreConverter<Domain.PostalCode>]
[EfCoreConverter<Domain.Province>]
[EfCoreConverter<Domain.EmailAddress>]
[EfCoreConverter<Domain.Gender>]
[EfCoreConverter<Domain.Language>]
[EfCoreConverter<Domain.AccountType>]
[EfCoreConverter<Domain.AccountNumber>]
[EfCoreConverter<Domain.TransactionReference>]
internal partial class VogenEfCoreConverters;
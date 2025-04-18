using Vogen;

namespace EfCoreDemo.Infrastructure.Conversion;

[EfCoreConverter<Domain.AccountId>]
[EfCoreConverter<Domain.BranchCode>]
[EfCoreConverter<Domain.CustomerId>]
[EfCoreConverter<Domain.FirstName>]
[EfCoreConverter<Domain.LastName>]
[EfCoreConverter<Domain.TransactionId>]
[EfCoreConverter<Domain.Money>]
internal partial class VogenEfCoreConverters;
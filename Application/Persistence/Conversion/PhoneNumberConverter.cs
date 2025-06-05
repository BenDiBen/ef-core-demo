using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfCoreDemo.Persistence.Conversion;

public class PhoneNumberConverter() : ValueConverter<PhoneNumber, string>(v => v.Value.Replace(" ", string.Empty),
    v => PhoneNumber.From(v));

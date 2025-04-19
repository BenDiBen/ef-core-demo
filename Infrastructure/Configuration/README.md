# EF Core Configuration Guide

## Configuration Classes Overview

Configuration classes in EF Core (like `CustomerConfiguration`) provide a clean, centralized way to:
- Define database mappings
- Configure relationships
- Set constraints (lengths, required fields, etc.)
- Manage value object conversions

Advantages over model annotations:
1. **Separation of concerns** - Database concerns stay out of domain models
2. **More powerful** - Can configure complex relationships and conversions
3. **Maintainable** - All database mappings are in one place

## OwnsOne Navigation Builder

The `OwnsOne` builder configures value objects (like `CustomerName`) to be stored in the same table as the parent entity (`Customer`). Example:

```csharp
builder.OwnsOne(c => c.Name, nameBuilder => 
{
    nameBuilder.Property(n => n.Given).HasMaxLength(50);
    nameBuilder.Property(n => n.Family).HasMaxLength(50);
});
```

## Migration Tips

To test configuration changes:
1. `dotnet ef migrations add TestConfigChange`
2. Examine the generated migration file
3. If satisfied: `dotnet ef database update`
4. If not: `dotnet ef migrations remove`

### Example: Fixing varchar(max)

If you see `varchar(max)` in a migration:
1. Find the property in your configuration class
2. Add `.HasMaxLength(100)` (or appropriate length)
3. Create a new migration to verify the change

Example fixing an address line:
```csharp
builder.Property(a => a.Line1).HasMaxLength(100);
```

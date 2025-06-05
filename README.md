# Entity Framework Core Demo

This project is intended as a demonstration of advanced Entity Framework Core features in a .NET-based application. It's designed to help developers understand various EF Core concepts with a focus on optimizing the performance of EF Core queries.

## 🚀 Getting Started

### Prerequisites

- .NET 9.0
- SQL Server (or compatible database)

### Configuration

1. Update the connection string in your configuration:
2. The application will automatically:
   - Apply pending migrations on startup in development
   - Seed initial data if the database is empty

## 🏗️ Project Structure

### [Domain Layer](./Application/Domain)

Contains the core business entities and domain logic. The project implements:

- Strong typing for entity IDs using value objects
- Domain entities like `Customer`, `Account`, and `Transaction`
- Soft delete functionality through `ISoftDeleteEntity` interface

### [Persistence Layer](./Application/Persistence)

Demonstrates advanced Entity Framework Core configurations and patterns:

- Custom value object conversions
- Soft delete implementation using query filters and interceptors
- Entity configurations using `IEntityTypeConfiguration<T>`
- Database seeding

[Learn more here.](./Applicaiton/Persistence/Configuration/README.md)

### [Web Layer](./Application/Web)

Implements a clean API structure using minimal APIs and demonstrates various query patterns:

- Grouped API endpoints for different domains (accounts, customers, marketing)
- Pagination implementation
- Various query optimization techniques
- Split query demonstrations for complex data loading

## 🔧 Features

### Value Objects using Vogen

Value objects are immutable types that represent a concept in your domain through their values rather than their identity. They:

- Encapsulate validation logic
- Prevent primitive obsession
- Make the code more type-safe and expressive
- Ensure domain invariants are maintained

[Vogen](https://github.com/SteveDunn/Vogen) is a .NET Source Generator and analyzer that turns primitives into strongly type value objects. It has the following benefits:

1. **Type Safety**

   - Compile-time type checking
   - Prevention of accidental type mixing (e.g., can't mix CustomerId with AccountId)
   - Elimination of primitive obsession

2. **Code Generation**

   - Automatic generation of value object implementations
   - Built-in validation support
   - Clean, boilerplate-free code
   - Source generation for better performance

3. **Entity Framework Core Integration**

   - Seamless integration with EF Core value converters
   - Automatic conversion to/from primitive types in the database
   - Efficient storage and querying

4. **DDD Alignment**
   - Natural fit with Domain-Driven Design principles
   - Clear expression of domain concepts
   - Improved code maintainability and readability

### Entity Framework Core Features

1. **Query Filters**

   - Global filters for soft delete functionality
   - Automated application through `ISoftDeleteEntity` interface

2. **Advanced Querying**

   - Split queries for performance optimization
   - Includes with ordering
   - Pagination implementation

3. **Configuration**
   - Fluent API configurations
   - Convention-based configuration
   - Alternate keys and constraints

## 📖 Additional Resources

For more information about the concepts demonstrated in this project:

- [Entity Framework Core Documentation](https://learn.microsoft.com/en/ef/core/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en/aspnet/core)
- [Vogen GitHub Repository](https://github.com/SteveDunn/Vogen)
- [Domain-Driven Design Fundamentals](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

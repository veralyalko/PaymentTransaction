# PaymentTransaction Challenge Task
### Challenge Task
A service that ingests payment transaction data from multiple external providers (e.g., PayPal, Trustly, BitPay), normalizes the data, stores it internally, and exposes it via an internal API for other systems to consume.
### Note
While this solution provides a working implementation of the required features—including provider-based ingestion, normalized data storage, Swagger documentation, unit testing, API key authentication, and idempotency handling—there is still significant room for improvement. Additional validations, stricter input checks, and provider-specific logic could further enhance the reliability and robustness of the system.

# PaymentTransaction API

A lightweight ASP.NET Core Web API for managing payment transactions across multiple providers, supporting API key authentication, Swagger documentation, and EF Core migrations.

---

## Setup Instructions

### Prerequisites

- .NET SDK 9.0 
- SQL Server or SQL Server Express
- Git
- (Optional) Visual Studio 2022+ or VS Code

---

## Configuration

1. Clone the repository:

```bash
git clone https://github.com/veralyalko/PaymentTransaction.git
```

2. Edit appsettings.json:

```json
{
  "ApiKey": "simple-test-api-key-12345",
  "ConnectionStrings": {
    "PaymentTransactionConnectionString": "Server=localhost,1433;Database=PaymentTransactionDb; Encrypt=False;TrustServerCertificate=True; User Id=sa;Password=Password123;",
    "PaymentAuthConnectionString": "Server=localhost,1433;Database=PaymentTransactionAuthDb; Encrypt=False;TrustServerCertificate=True; User Id=sa;Password=Password123;"
  }
}
```

3. Install Dependencies & Tools

```bash
dotnet restore
dotnet tool install --global dotnet-ef
```

4. Apply Migrations

```bash
dotnet ef database update
```

5. Run Swagger UI in browser:

```
https://localhost:[PORT]/swagger
```

---

## Authentication

All secured endpoints require an API key in the request headers. You can use the "Authorize" button in Swagger UI to enter your key.

Test API key is configured in `appsettings.json`. 
I used https://jwt.io/ to create JWT token and APIKey

---

## Unit Tests

Run unit tests from the root folder. There are a few sample test cases to demonstration purpouses:

```bash
dotnet test ./Tests/PaymentTransaction.Tests.csproj
```

---

## API Documentation

### Transaction

- `POST /ingest/{providerName}` (Requires `Idempotency-Key`)
- `POST /api/transaction`
- `GET /api/transaction/{id}`
- `PUT /api/transaction/{id}`
- `DELETE /api/transaction/{id}`

Example provider payloads
- providerName required string (path) PayPal
- Idempotency-Key required string GUID (header) 
```json
{
  "amount": 0.01,
  "payerEmail": "user@example.com",
  "currencyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "statusId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "paymentMethodId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
### Status

- `GET /api/status`
- `GET /api/status/{id}`
- `POST /api/status`
- `PUT /api/status/{id}`
- `DELETE /api/status/{id}`

Example Status data
```json
{
  "statusName": "Failed"
}
```
### Currency

- `GET /api/currency`
- `GET /api/currency/{id}`
- `POST /api/currency`
- `PUT /api/currency/{id}`
- `DELETE /api/currency/{id}`

Example Currency data
```json
{
  "currencyName": "USD"
}
```

### Payment Method

- `GET /api/paymentmethod`
- `GET /api/paymentmethod/{id}`
- `POST /api/paymentmethod`
- `PUT /api/paymentmethod/{id}`
- `DELETE /api/paymentmethod/{id}`

Example Payment Method data
```json
{
  "paymentMethodName": "Wallet"
}
```

### Provider

- `GET /api/providers`
- `GET /api/providers/{id}`
- `POST /api/providers`
- `PUT /api/providers/{id}`
- `DELETE /api/providers/{id}`

Example Provider data
1. PayPal:
```json
{
 "providerName": "PayPal",
  "type": "PayPal",
  "metadataJson": "{\"Mode\":\"test\",\"defaultCurrency\":\"USD\"}"
}
```
2. Trustly:
```json
{
  "providerName": "Trustly",
  "type": "Trustly",
  "metadataJson": "{\"Region\":\"EU\",\"Currency\":\"EUR\"}"
}
```

### Summary

- `GET /summary`

### Webhook (Simulated Push)

- `POST /webhook/{providerName}` (Requires `Idempotency-Key`)

---

## Filters and Query Examples

`GET /transactions?filterOn=Status&filterQuery=Completed&fromDate=2024-01-01&toDate=2024-03-01&sortBy=providerName&isAssending=true`

---

## Swagger Enhancements

Swagger UI uses annotations ([SwaggerSchema], [SwaggerSchemaExample]) for better model documentation.

---

## Project Structure

```
PaymentTransaction/
├── Attributes/
├── Controllers/
├── CustomActionFilters/
├── Data/
├── Mappings/
├── Middleware/
├── Models/
│   ├── Configs/
│   ├── Domain/
│   ├── DTO/
├── Migrations/
├── Properties/
├── Repositories/
├── Validators/
├── appsettings.json
├── Program.cs
├── README.md
Tests/
└── Controllers/
```

---

## Middleware

`CombinedAuthMiddleware.cs`: Simulates lightweight API key authentication using claims-based identity.

---

## Models Folder

- `Domain`: Entity Framework Core models representing database structure.
- `DTO`: Models for API request/response validation.
- `Configs`: Provider-specific configurations (e.g., PayPalConfig).

---

## Migrations

EF Core migration files created with:

To add a new migration, use:
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

To apply the existing migration to the database run:
```bash
dotnet ef database update --context PaymentTransactionDbContext
dotnet ef database update --context PaymentTransactionAuthDbContext
```

---

## Mappings

AutoMapper profiles used to convert between DTOs and Domain Models.

---

## Data Folder

Contains `PaymentTransactionDbContext`, which connects EF Core models to the database.

---

## Repositories

Abstract data access logic from controllers, using EF Core in `SQLProviderRepository`.

---

## Validators

DTO validation is enforced using Model Data Annotations for basic validations, while Transaction DTO validation is handled using FluentValidation for more complex and custom validation logic

---

## Custom Action Filters

Reusable components to enforce cross-cutting concerns like model validation and idempotency.

---

## Attributes

Custom attributes to enhance Swagger docs and enforce headers like `Idempotency-Key`.

---

## Idempotency-Key Implementation

- Endpoints like `/ingest/{providerName}` and `/webhook/{providerName}` require `Idempotency-Key`.
- `IdempotencyFilter` prevents duplicate transactions.
- Swagger automatically documents this requirement.

---

## Global Exception Handling
Implemented a custom middleware (ExceptionHandlerMiddleware) to catch unhandled exceptions, log error details, and return a standardized JSON error response.

---

## Versioning
Configured API versioning using AddApiVersioning() and optional IApiVersionDescriptionProvider for managing and documenting multiple API versions.

---

## Serilog
Integrated Serilog for structured logging with output to both console and rolling log files to aid in diagnostics and monitoring.

---

## Dependency Injection

ASP.NET Core DI is used for injecting DbContext, repositories, and AutoMapper.

---

## Design Notes and Assumptions

- Domain models are decoupled from DTOs using AutoMapper.
- Custom filters manage reusable logic (validation, idempotency).
- Modular and scalable structure to add new providers.
- Swagger improves discoverability.
- Data validation ensures input integrity.

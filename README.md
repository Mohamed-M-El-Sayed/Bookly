# Bookly – Hotel Booking Backend API
[**Swagger API Documentation**](https://YOUR-DEPLOYED-DOMAIN/swagger)

Bookly is a hotel booking backend API built with **ASP.NET Core**. It provides hotel management and booking functionality, allowing guests to search for hotels, make reservations, and manage their bookings while hotel managers manage their properties.

## Features

- **Authentication & Authorization**
  - ASP.NET Core Identity.
  - JWT authentication with refresh tokens.
  - Role-based access control (Admin, Hotel Manager, Customer).
  - Email confirmation and password reset.

- **Hotel Management**
  - Create and manage hotels.
  - Manage room types, amenities, and hotel information.
  - Hotel manager access control.

- **Hotel Search**
  - Search hotels by location and availability.
  - Filter by price and occupancy.
  - Support for date-based availability and pricing.

- **Booking Management**
  - Create and manage bookings.
  - Booking availability validation.
  - Automatic expiration of pending bookings using background jobs.

- **Payments**
  - Stripe Checkout integration.
  - Payment processing through Stripe webhooks.

- **Reviews & Favorites**
  - Guests can review hotels based on their bookings.
  - Add and remove favorite hotels.

- **Caching**
  - Redis-backed ASP.NET Core Output Caching.
  - Cache policies for hotel search, amenities, and reviews.
  - Cache invalidation using tags.

- **Background Processing**
  - Hangfire for background and scheduled jobs.

- **API Security**
  - Global Sliding Window rate limiting.
  - Fixed Window rate limiting for authentication endpoints.
  - Token Bucket rate limiting for booking endpoints.

## Technologies

| Category | Technologies |
|---|---|
| Backend | C#, ASP.NET Core, REST APIs, FluentValidation |
| Database | SQL Server, Entity Framework Core |
| Architecture | Clean Architecture, CQRS, MediatR |
| Authentication | ASP.NET Core Identity, JWT |
| Payments | Stripe |
| Caching | Redis, Output Caching |
| Background Jobs | Hangfire |
| API Documentation | Swagger / OpenAPI |
| Design Patterns | Repository, Unit of Work, Specification, Result Pattern |

## Architecture
Bookly follows **Clean Architecture** and **Vertical Slice Architecture** to maintain separation of concerns and organize features around business functionality.
```text
Bookly/
│
├── Bookly.Api/
│   ├── Controllers/
│   ├── Middlewares/
│   ├── Extensions/
│   └── Program.cs
│
├── Bookly.Application/
│   ├── Common/
│   │   └── Behaviors/
│   ├── Contracts/
│   └── Features/
│
├── Bookly.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── Exceptions/
│   └── ...
│
├── Bookly.Infrastructure/
│   ├── Persistence/
│   ├── Services/
│   └── ...
│
└── Bookly.sln
```
### Architectural Approach

- **Clean Architecture:** Keeps business logic independent of infrastructure.
- **CQRS:** Separates commands and queries using MediatR.
- **Repository & Unit of Work:** Organizes data access.
- **Specification Pattern:** Encapsulates reusable query conditions.
- **Result Pattern:** Handles application operation outcomes consistently.
- **Pipeline Behaviors:** Centralizes validation and cache invalidation.

## Getting Started

### Prerequisites

Make sure you have installed:

- .NET SDK compatible with the project.
- SQL Server.
- Redis.
- Stripe account for payment integration.

### 1. Clone the repository

```bash
git clone https://github.com/Mohamed-M-El-Sayed/Bookly.git
cd Bookly
```

### 2. Configure application settings

Configure the required settings using User Secrets or environment variables.

Example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Secret": "YOUR_JWT_SECRET",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE"
  },
  "Stripe": {
    "SecretKey": "YOUR_STRIPE_SECRET_KEY",
    "WebhookSecret": "YOUR_STRIPE_WEBHOOK_SECRET"
  }
}
```

You may also need to configure email and other external service settings.

**Never commit real credentials, API keys, or connection strings to source control.**

### 3. Apply database migrations

Run the EF Core migrations using the appropriate project and startup project:

```bash
dotnet ef database update --project Bookly.Infrastructure --startup-project Bookly.Api
```

### 4. Run the API

```bash
dotnet run --project Bookly.Api
```

### 5. Explore the API

Once the application is running, open Swagger:

```text
https://localhost:7271/swagger
```

The exact URL may differ depending on your launch settings.

## External Services

| Service | Purpose |
|---|---|
| SQL Server | Persistent application data |
| Redis | Output caching |
| Stripe | Payment processing |
| Hangfire | Background job processing |
| SMTP | Email notifications |

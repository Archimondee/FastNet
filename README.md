# FastNet

A high-performance .NET 9 Web API template built with **FastEndpoints**, designed with Clean Architecture principles and enterprise-grade features out of the box.

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![FastEndpoints](https://img.shields.io/badge/FastEndpoints-7.2.0-00D4AA?style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Ready-336791?style=for-the-badge&logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)

---

## 📁 Project Structure

```
FastNet/
├── API/                        # Presentation Layer (Web API)
│   ├── Endpoints/              # FastEndpoints route handlers
│   │   ├── Health/             # Health check endpoint
│   │   └── User/               # User management endpoints
│   ├── Extensions/             # Service configuration extensions
│   ├── Middlewares/            # Custom middleware components
│   └── Properties/             # Launch settings
├── Application/                # Application Layer (Use Cases, DTOs)
│   ├── Interface/              # Application interfaces (IAppDbContext)
│   ├── Behavior/               # Pipeline behaviors (Transaction, Logging)
│   └── Users/                  # User use cases
│       └── CreateUser/         # Create user command, handler, validator
├── Domain/                     # Domain Layer (Entities, Value Objects)
│   ├── Common/                 # Base entities with audit fields
│   └── Entities/               # Domain entities (User)
├── Infrastructure/             # Infrastructure Layer (Data Access, External Services)
│   ├── Configurations/         # EF Core entity configurations
│   ├── Migrations/             # Database migrations
│   └── Persistence/            # DbContext implementation
├── Shared/                     # Shared Kernel (Commons, Exceptions, Utilities)
│   └── Commons/
│       ├── Error/              # Error codes definitions
│       ├── Exceptions/         # Custom exception classes
│       └── Response/           # API response models
├── Directory.Build.props       # Central build properties
├── Directory.Packages.props    # Central package management
├── StyleCop.ruleset            # Code style rules
└── compose.yaml                # Docker Compose configuration
```

---

## ✅ Implemented Features

### 🏗️ Architecture & Infrastructure

| Feature | Status | Description |
|---------|--------|-------------|
| **Clean Architecture** | ✅ | Layered structure (API, Application, Domain, Infrastructure, Shared) |
| **.NET 9** | ✅ | Latest .NET runtime with performance improvements |
| **FastEndpoints** | ✅ | High-performance endpoint routing (v7.2.0) |
| **Central Package Management** | ✅ | `Directory.Packages.props` for unified versioning |
| **Code Analysis** | ✅ | StyleCop + SonarAnalyzer integration |
| **Pipeline Behaviors** | ✅ | Transactional behavior wrapping handlers |
| **Docker Support** | ✅ | Multi-stage Dockerfile for optimized builds |
| **Docker Compose** | ✅ | Container orchestration ready |

### 🗄️ Database & Persistence

| Feature | Status | Description |
|---------|--------|-------------|
| **Entity Framework Core 9** | ✅ | Modern ORM with full async support |
| **PostgreSQL** | ✅ | Production-ready database with Npgsql provider |
| **DbContext Abstraction** | ✅ | `IAppDbContext` interface for testability |
| **Entity Configurations** | ✅ | Fluent API configurations for entities |
| **Migrations** | ✅ | Code-first database migrations support |
| **Soft Delete** | ✅ | Global query filter for soft-deleted entities |
| **Audit Fields** | ✅ | Automatic `CreatedAt`, `UpdatedAt`, `DeletedAt` tracking |
| **Audit Trail** | ✅ | `CreatedBy`, `UpdatedBy`, `DeletedBy` user tracking |

### 👤 User Management

| Feature | Status | Description |
|---------|--------|-------------|
| **User Entity** | ✅ | Domain entity with email and password |
| **Create User Endpoint** | ✅ | `POST /api/users` - Register new users |
| **Email Validation** | ✅ | FluentValidation for email format |
| **Password Validation** | ✅ | Minimum 8 characters requirement |
| **Duplicate Prevention** | ✅ | Unique email constraint with conflict handling |

### 🔒 Security Features

| Feature | Status | Description |
|---------|--------|-------------|
| **Security Headers** | ✅ | X-Content-Type-Options, X-XSS-Protection, Referrer-Policy, CSP |
| **HSTS** | ✅ | HTTP Strict Transport Security (non-development) |
| **CORS** | ✅ | Environment-aware CORS policies |
| **HTTPS Redirection** | ✅ | Automatic HTTPS enforcement |

### 📊 Logging & Observability

| Feature | Status | Description |
|---------|--------|-------------|
| **Serilog** | ✅ | Structured logging framework |
| **Console Sink** | ✅ | Colored console output with custom template |
| **File Sink** | ✅ | Daily rolling file logs (`logs/log-.txt`) |
| **Correlation ID** | ✅ | Request tracing via `X-Correlation-Id` header |
| **Log Enrichment** | ✅ | Automatic enrichment with ClientIP and Path |

### ⚡ Performance Features

| Feature | Status | Description |
|---------|--------|-------------|
| **Rate Limiting** | ✅ | Fixed window limiter (100 req/min per IP) |
| **Response Compression** | ✅ | Gzip + Brotli with fastest compression |
| **Response Caching** | ✅ | Built-in response caching middleware |
| **Performance Middleware** | ✅ | Request timing logs + Slow request (>500ms) warnings |

### 📝 API Documentation

| Feature | Status | Description |
|---------|--------|-------------|
| **Swagger/OpenAPI** | ✅ | FastEndpoints.Swagger integration |
| **API Versioning** | ✅ | Documented as v1 |

### 🛡️ Exception Handling

| Feature | Status | Description |
|---------|--------|-------------|
| **Global Exception Middleware** | ✅ | Centralized error handling |
| **FluentValidation Support** | ✅ | 400 Bad Request with field-level errors |
| **Custom App Exceptions** | ✅ | Typed exceptions with error codes |
| **Standard Error Response** | ✅ | Consistent `ApiErrorResponse` format |

### 📌 Custom Exceptions

| Exception | HTTP Status | Error Code |
|-----------|-------------|------------|
| `NotFoundException` | 404 | `NOT_FOUND` |
| `UnauthorizedException` | 401 | `UNAUTHORIZED` |
| `ForbiddenException` | 403 | `FORBIDDEN` |
| `ConflictException` | 409 | `CONFLICT` |
| Validation Errors | 400 | `VALIDATION_ERROR` |
| System Errors | 500 | `INTERNAL_SERVER_ERROR` |

### 🏛️ Domain-Driven Design

| Feature | Status | Description |
|---------|--------|-------------|
| **Base Entity** | ✅ | Abstract base with ID and audit tracking |
| **Encapsulated Entities** | ✅ | Private setters, factory constructors |
| **Value Objects Ready** | ✅ | Architecture supports value objects |

### 🩺 Health & Monitoring

| Feature | Status | Description |
|---------|--------|-------------|
| **Health Endpoint** | ✅ | `GET /health` - Returns status and UTC time |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/) (or use Docker)
- [Docker](https://www.docker.com/) (optional)

### Database Setup

```bash
# Start PostgreSQL with Docker (if not installed locally)
docker run -d --name postgres \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_DB=fastapi \
  -p 5432:5432 \
  postgres:16

# Update connection string in API/appsettings.json
```

### Running Locally

```bash
# Restore dependencies
dotnet restore

# Apply database migrations
cd API
dotnet ef database update --project ../Infrastructure

# Run the API
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

### Running with Docker

```bash
# Build and run with Docker Compose
docker-compose up --build

# Or build manually
docker build -t fastnet-api -f API/Dockerfile .
docker run -p 8080:8080 fastnet-api
```

---

## 🔧 Configuration

### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=fastapi;Username=postgres;Password=yourpassword"
  },
  "Cors": {
    "Origins": ["https://localhost:3000"]
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}
```

### Environment-Based CORS

| Environment | Behavior |
|-------------|----------|
| `Development` / `Staging` | Allow any origin |
| `Production` | Whitelist from `Cors:Origins` config |

---

## 📦 NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| FastEndpoints | 7.2.0 | High-performance routing |
| FastEndpoints.Swagger | 7.2.0 | OpenAPI documentation |
| Microsoft.EntityFrameworkCore | 9.0.12 | ORM for database access |
| Npgsql.EntityFrameworkCore.PostgreSQL | 9.0.4 | PostgreSQL provider |
| FluentValidation | 12.1.1 | Request validation |
| Serilog | 4.3.0 | Structured logging |
| Serilog.AspNetCore | 9.0.0 | ASP.NET Core integration |
| Serilog.Sinks.Console | 6.1.1 | Console output |
| Serilog.Sinks.File | 7.0.0 | File logging |
| Serilog.Settings.Configuration | 9.0.0 | Configuration binding |
| StyleCop.Analyzers | 1.2.0-beta.435 | Code style enforcement |
| SonarAnalyzer.CSharp | 9.23.0 | Static code analysis |

---

## 🗂️ Feature Status Legend

| Icon | Status |
|------|--------|
| ✅ | Implemented |
| 🚧 | In Progress |
| ❌ | Not Started |

---

## 🏷️ API Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/health` | Health check endpoint | Anonymous |
| `POST` | `/api/users` | Create a new user | Anonymous |

### User Endpoints

#### Create User

```http
POST /api/users
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "securepassword123"
}
```

**Response (201 Created):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validation Errors (400 Bad Request):**
```json
{
  "code": "VALIDATION_ERROR",
  "message": "Validation failed",
  "errors": [
    { "field": "Email", "error": "'Email' is not a valid email address." },
    { "field": "Password", "error": "'Password' must be at least 8 characters." }
  ],
  "correlationId": "abc123"
}
```

**Conflict Error (409 Conflict):**
```json
{
  "code": "CONFLICT",
  "message": "Email already exists",
  "correlationId": "abc123"
}
```

---

## 📋 TODO / Roadmap

| Feature | Priority | Status |
|---------|----------|--------|
| Authentication (JWT/OAuth) | High | ❌ |
| Password Hashing (BCrypt) | High | ❌ |
| Repository Pattern | Medium | ❌ |
| Unit of Work | Medium | ✅ |
| MediatR/CQRS | Medium | ❌ |
| Background Jobs (Hangfire/Quartz) | Medium | ❌ |
| Distributed Caching (Redis) | Medium | ❌ |
| API Versioning (Advanced) | Low | ❌ |
| Health Checks (Database) | Low | ❌ |
| Integration Tests | Medium | ❌ |
| Unit Tests | Medium | ❌ |
| Get User Endpoint | Medium | ❌ |
| Update User Endpoint | Medium | ❌ |
| Delete User Endpoint | Medium | ❌ |

---

## 📄 License

This project is licensed under the MIT License.

---

## 👤 Author

FastNet Template - Built for high-performance .NET APIs.

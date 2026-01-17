# FastNet

A high-performance .NET 9 Web API template built with **FastEndpoints**, designed with Clean Architecture principles and enterprise-grade features out of the box.

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![FastEndpoints](https://img.shields.io/badge/FastEndpoints-7.2.0-00D4AA?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)

---

## 📁 Project Structure

```
FastNet/
├── API/                        # Presentation Layer (Web API)
│   ├── Endpoints/              # FastEndpoints route handlers
│   ├── Extensions/             # Service configuration extensions
│   ├── Middlewares/            # Custom middleware components
│   └── Properties/             # Launch settings
├── Application/                # Application Layer (Use Cases, DTOs)
├── Domain/                     # Domain Layer (Entities, Value Objects)
├── Infrastructure/             # Infrastructure Layer (Data Access, External Services)
├── Shared/                     # Shared Kernel (Commons, Exceptions, Utilities)
│   └── Commons/
│       ├── Error/              # Error codes definitions
│       ├── Exceptions/         # Custom exception classes
│       └── Response/           # API response models
├── Directory.Build.props       # Central build properties
├── Directory.Packages.props    # Central package management
├── StyleCop.ruleset           # Code style rules
└── compose.yaml               # Docker Compose configuration
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
| **Docker Support** | ✅ | Multi-stage Dockerfile for optimized builds |
| **Docker Compose** | ✅ | Container orchestration ready |

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
| Validation Errors | 400 | `VALIDATION_ERROR` |
| System Errors | 500 | `INTERNAL_SERVER_ERROR` |

### 🩺 Health & Monitoring

| Feature | Status | Description |
|---------|--------|-------------|
| **Health Endpoint** | ✅ | `GET /health` - Returns status and UTC time |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (optional)

### Running Locally

```bash
# Restore dependencies
dotnet restore

# Run the API
cd API
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
| Serilog | 4.3.0 | Structured logging |
| Serilog.AspNetCore | 9.0.0 | ASP.NET Core integration |
| Serilog.Sinks.Console | 6.1.1 | Console output |
| Serilog.Sinks.File | 7.0.0 | File logging |
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

## 📋 TODO / Roadmap

| Feature | Priority | Status |
|---------|----------|--------|
| Authentication (JWT/OAuth) | High | ❌ |
| Database Integration (EF Core) | High | ❌ |
| Repository Pattern | High | ❌ |
| Unit of Work | Medium | ❌ |
| MediatR/CQRS | Medium | ❌ |
| Background Jobs (Hangfire/Quartz) | Medium | ❌ |
| Distributed Caching (Redis) | Medium | ❌ |
| API Versioning | Low | ❌ |
| Health Checks (Advanced) | Low | ❌ |
| Integration Tests | Medium | ❌ |
| Unit Tests | Medium | ❌ |

---

## 🏷️ API Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/health` | Health check endpoint | Anonymous |

---

## 📄 License

This project is licensed under the MIT License.

---

## 👤 Author

FastNet Template - Built for high-performance .NET APIs.

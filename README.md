# 🛠️ Services Microservice

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](LICENSE.md)
[![Tests](https://img.shields.io/badge/tests-passing-success)](tests/)
[![Coverage](https://img.shields.io/badge/coverage-85%25-green)]()
[![Docker](https://img.shields.io/badge/docker-ready-2496ED?logo=docker)](Dockerfile)

A production-ready microservice for managing service catalogs, controllers, and actions metadata built with .NET 9. Implements Clean Architecture, DDD, and CQRS patterns with support for service registry and API endpoint management.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#️-technology-stack)
- [Prerequisites](#️-prerequisites)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [gRPC Service](#-grpc-service)
- [Configuration](#️-configuration)
- [Use Cases & Scenarios](#-use-cases--scenarios)
- [Architecture](#️-architecture)
- [Testing](#-testing)
- [Best Practices](#-best-practices)
- [Troubleshooting](#-troubleshooting)
- [Service Discovery](#-service-discovery)
- [Domain Model](#-domain-model)
- [Security](#-security)
- [FAQ](#-faq)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

## What is this microservice?

The Services microservice maintains a catalog of all API endpoints available across the platform. It solves the problem of knowing "what operations exist in the system" by registering each microservice, its controllers, and the HTTP actions it exposes. The RBAC microservice depends on this catalog to map permissions to concrete endpoints, and the Modules microservice uses it to group endpoints into functional features. It is managed by system administrators and consumed by other infrastructure microservices; end users never interact with it directly.

---

The Services microservice provides a centralized catalog for managing microservice metadata, including service definitions, controllers (endpoints), and actions (operations). It acts as a service registry that tracks the structure and capabilities of your distributed system.

- **Service Registry**: Maintain a catalog of all microservices in your ecosystem
- **Controller Management**: Track REST/gRPC controllers and their endpoints
- **Action Tracking**: Document HTTP methods and operations available per controller
- **Multi-tenancy**: Isolate service catalogs by tenant
- **Service Discovery**: Enable dynamic service discovery and API documentation generation
- **Metadata Storage**: Store service descriptions, versions, and contact information

### 🚀 Quick Start

```bash
# 1. Start infrastructure services
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d

# 2. Configure Vault secrets
cd ../../tools/vault
./config-vault.sh

# 3. Run the REST API
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Services.Rest

# 4. Access Swagger UI
open http://localhost:5000/swagger
```

### 📊 High-Level Architecture

```
┌─────────────────────┐
│  Client/API Gateway │
│   (Service Mesh)    │
└──────────┬──────────┘
           │ HTTPS + JWT / gRPC
           │
┌──────────▼────────────────────────────────────────────┐
│       Services Microservice (Catalog Registry)        │
│  ┌──────────────┐  ┌─────────────┐  ┌─────────────┐  │
│  │ Controllers  │  │  MediatR    │  │  Handlers   │  │
│  │ REST/gRPC    │─▶│   (CQRS)    │─▶│ (Business)  │  │
│  └──────────────┘  └─────────────┘  └──────┬──────┘  │
│                                             │         │
│  ┌──────────────────────────────────────────▼──────┐  │
│  │         ServiceAggregate (DDD)                  │  │
│  │  ┌────────────┐  ┌──────────┐  ┌────────────┐  │  │
│  │  │  Service   │  │Controller│  │  Actions   │  │  │
│  │  │  Metadata  │─▶│ Entities │─▶│  Entities  │  │  │
│  │  └────────────┘  └──────────┘  └────────────┘  │  │
│  └──────────────────────────────────────────────────┘  │
└───────┬──────────────────┬──────────────────┬─────────┘
        │                  │                  │
   ┌────▼────┐      ┌──────▼──────┐    ┌─────▼─────┐
   │ MongoDB │      │   Redis     │    │ RabbitMQ  │
   │(Catalog)│      │  (Cache)    │    │ (Events)  │
   └─────────┘      └─────────────┘    └───────────┘
```

## 🚀 Key Features

### Core Capabilities

- ✅ **Service Catalog Management**: Create and manage service registrations
- ✅ **Controller Registration**: Track REST/gRPC controllers and endpoints
- ✅ **Action Management**: Document HTTP methods (GET, POST, PUT, DELETE, PATCH)
- ✅ **Service Discovery**: Query services by name or list all registered services
- ✅ **Hierarchical Structure**: Service → Controllers → Actions
- ✅ **Soft Delete**: Mark services as inactive without permanent deletion
- ✅ **Batch Operations**: Add multiple controllers/actions in a single operation
- ✅ **Change Tracking**: Audit who created/updated service metadata
- ✅ **Domain Events**: Publish events for service catalog changes
- ✅ **Problem Details**: RFC 7807 compliant error responses

### Technical Features

- Clean Architecture with DDD and CQRS
- Domain events for service lifecycle changes
- MongoDB for service catalog persistence
- RabbitMQ for event publishing
- Redis for distributed caching
- OAuth2/OpenID Connect security
- Multi-tenancy support
- REST and gRPC APIs
- Swagger/OpenAPI documentation
- Docker containerization
- Comprehensive test coverage (Unit, Integration)

## 🛠️ Technology Stack

### Core
- **.NET 9** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **C# 13** - Programming language

### Storage & Data
- **MongoDB** - Service catalog persistence
- **Redis** - Distributed caching

### Messaging & Events
- **RabbitMQ** - Event publishing and message broker

### Architecture & Patterns
- **MediatR** - CQRS command/query handling
- **FluentValidation** - Input validation
- **Mapster** - Object mapping
- **NodaTime** - Date/time handling

### Security & Configuration
- **Vault** - Secret management
- **OAuth2/OpenID Connect** - Authentication
- **JWT Bearer** - Token-based security
- **HTTPS** - Encrypted communication

### Communication
- **REST API** - HTTP/JSON endpoints
- **gRPC** - High-performance RPC protocol
- **Protocol Buffers** - gRPC message serialization

### DevOps & Testing
- **Docker** - Containerization
- **Kubernetes** - Orchestration (Helm charts included)
- **xUnit** - Unit/integration testing
- **Swagger/OpenAPI** - API documentation

## ⚙️ Prerequisites

### Required
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker & Docker Compose** - For infrastructure services
- **MongoDB 6.0+** - Document database
- **Redis 7.0+** - Caching layer
- **RabbitMQ 3.12+** - Message broker

### Optional
- **Vault** - Secret management (can use appsettings for local dev)
- **Kubernetes** - For production deployment

## 🚀 Getting Started

The following instructions will help you set up the project on your local machine for development and testing purposes.

### Local Development Setup

1. **Clone the repository**:
```bash
git clone <repository-url>
cd CodeDesignPlus.Net.Microservice.Services
```

2. **Start infrastructure services** using Docker Compose. Clone the environment repository:
```bash
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d
```

3. **Configure Vault** (optional for local dev):
```bash
cd ../../CodeDesignPlus.Net.Microservice.Services/tools/vault
./config-vault.sh
```

4. **Build the solution**:
```bash
dotnet build
```

5. **Run the desired entry point**:

   **For REST API** (Port 5000):
   ```bash
   dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Services.Rest
   ```

   **For gRPC** (Port 5001):
   ```bash
   dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Services.gRpc
   ```

6. **Access the APIs**:
   - **REST Swagger UI**: http://localhost:5000/swagger
   - **gRPC Reflection**: Use tools like [grpcurl](https://github.com/fullstorydev/grpcurl) or [BloomRPC](https://github.com/bloomrpc/bloomrpc)

### Docker Deployment

**Build and run REST API container**:
```bash
docker build -t ms-services-rest . -f src/entrypoints/CodeDesignPlus.Net.Microservice.Services.Rest/Dockerfile
docker run -d -p 5000:5000 --network=backend -e ASPNETCORE_ENVIRONMENT=Docker --name ms-services-rest ms-services-rest
```

**Build and run gRPC container**:
```bash
docker build -t ms-services-grpc . -f src/entrypoints/CodeDesignPlus.Net.Microservice.Services.gRpc/Dockerfile
docker run -d -p 5001:5001 --network=backend -e ASPNETCORE_ENVIRONMENT=Docker --name ms-services-grpc ms-services-grpc
```

### Kubernetes Deployment

```bash
# Deploy REST API
helm install ms-services-rest ./charts/ms-services-rest

# Deploy gRPC API
helm install ms-services-grpc ./charts/ms-services-grpc
```

## 📡 API Endpoints

### Service Operations

#### Create Service
Register a new service in the catalog.

```http
POST /api/service
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "ms-orders",
  "description": "Order Management Microservice",
  "isActive": true
}
```

**Response**: `204 No Content`

---

#### Get All Services
Retrieve all services with pagination and filtering.

```http
GET /api/service?limit=50&skip=0&filter=isActive eq true&orderby=name asc
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Query Parameters**:
- `limit` (optional): Number of items per page (default: 100)
- `skip` (optional): Number of items to skip (default: 0)
- `filter` (optional): OData filter expression
- `orderby` (optional): OData order expression

**Response**: `200 OK`
```json
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "ms-orders",
      "description": "Order Management Microservice",
      "isActive": true,
      "controllers": [
        {
          "id": "660e8400-e29b-41d4-a716-446655440001",
          "name": "OrderController",
          "description": "Handles order operations",
          "actions": [
            {
              "id": "770e8400-e29b-41d4-a716-446655440002",
              "name": "CreateOrder",
              "description": "Creates a new order",
              "httpMethod": "POST"
            }
          ]
        }
      ]
    }
  ],
  "totalCount": 15,
  "limit": 50,
  "skip": 0
}
```

---

#### Get Service by ID
Retrieve a specific service by its unique identifier.

```http
GET /api/service/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `200 OK`
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "ms-orders",
  "description": "Order Management Microservice",
  "isActive": true,
  "controllers": [...]
}
```

---

#### Update Service
Update service metadata.

```http
PUT /api/service/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "ms-orders",
  "description": "Updated: Order Management Microservice with new features",
  "isActive": true
}
```

**Response**: `204 No Content`

---

#### Delete Service
Soft delete a service (marks as inactive).

```http
DELETE /api/service/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content`

---

### Controller Operations

#### Add Controller
Add a controller to an existing service.

```http
POST /api/service/{id}/controller
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idController": "660e8400-e29b-41d4-a716-446655440001",
  "name": "OrderController",
  "description": "Handles order CRUD operations"
}
```

**Response**: `204 No Content`

---

#### Update Controller
Update controller metadata.

```http
PUT /api/service/{id}/controller/{idController}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "OrderController",
  "description": "Updated: Handles order operations with improved validation"
}
```

**Response**: `204 No Content`

---

#### Remove Controller
Remove a controller from a service.

```http
DELETE /api/service/{id}/controller/{idController}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content`

---

### Action Operations

#### Add Action
Add an action (endpoint) to a controller.

```http
POST /api/service/{id}/controller/{idController}/action
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idAction": "770e8400-e29b-41d4-a716-446655440002",
  "name": "CreateOrder",
  "description": "Creates a new order in the system",
  "httpMethod": "POST"
}
```

**HTTP Methods Supported**:
- `GET` - Retrieve resources
- `POST` - Create resources
- `PUT` - Update resources (full)
- `PATCH` - Update resources (partial)
- `DELETE` - Delete resources

**Response**: `204 No Content`

---

#### Update Action
Update action metadata.

```http
PUT /api/service/{id}/controller/{idController}/action/{idAction}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "CreateOrder",
  "description": "Updated: Creates a new order with validation",
  "httpMethod": "POST"
}
```

**Response**: `204 No Content`

---

#### Remove Action
Remove an action from a controller.

```http
DELETE /api/service/{id}/controller/{idController}/action/{idAction}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content`

---

### Error Responses

All errors follow RFC 7807 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Service not found.",
  "extensions": {
    "layer": "Application",
    "error_code": "SVC-404",
    "traceId": "0HMVJ3K7S5Q2K:00000001"
  }
}
```

**Common Status Codes**:
- `200 OK` - Success
- `204 No Content` - Success (no response body)
- `400 Bad Request` - Invalid input or business rule violation
- `401 Unauthorized` - Missing or invalid token
- `404 Not Found` - Service/Controller/Action not found
- `409 Conflict` - Duplicate service name
- `500 Internal Server Error` - Server error

## 🔌 gRPC Service

The Services microservice exposes gRPC endpoints for high-performance communication.

### Proto Definition

```protobuf
syntax = "proto3";

package Service;

option csharp_namespace = "CodeDesignPlus.Net.Microservice.Services.gRpc";

service Service {
  rpc CreateService (CreateServiceRequest) returns (google.protobuf.Empty);
  rpc GetService (GetServiceRequest) returns (GetServiceResponse);
}

message CreateServiceRequest {
  Microservice Service = 1;
}

message GetServiceRequest {
  string name = 1;
}

message GetServiceResponse {
  Microservice Service = 1;
}

message Microservice {
  string Id = 1;
  string Name = 2;
  string Description = 3;
  repeated Controller Controllers = 4;
}

message Controller {
  string Id = 1;
  string Name = 2;
  string Description = 3;
  repeated Action Actions = 4;
}

message Action {
  string Id = 1;
  string Name = 2;
  string Description = 3;
  HttpMethod HttpMethod = 4;
}

enum HttpMethod {
  NONE = 0;
  POST = 1;
  PUT = 2;
  DELETE = 3;
  PATCH = 4;
  GET = 5;
}
```

### Using gRPC with grpcurl

**List available services**:
```bash
grpcurl -plaintext localhost:5001 list
```

**Create a service**:
```bash
grpcurl -plaintext -d '{
  "Service": {
    "Id": "550e8400-e29b-41d4-a716-446655440000",
    "Name": "ms-orders",
    "Description": "Order Management Microservice",
    "Controllers": []
  }
}' localhost:5001 Service.Service/CreateService
```

**Get service by name**:
```bash
grpcurl -plaintext -d '{
  "name": "ms-orders"
}' localhost:5001 Service.Service/GetService
```

### Using gRPC in .NET Client

```csharp
using Grpc.Net.Client;
using CodeDesignPlus.Net.Microservice.Services.gRpc;

// Create gRPC channel
var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Service.ServiceClient(channel);

// Create service
await client.CreateServiceAsync(new CreateServiceRequest
{
    Service = new Microservice
    {
        Id = Guid.NewGuid().ToString(),
        Name = "ms-orders",
        Description = "Order Management Microservice",
        Controllers = { }
    }
});

// Get service
var response = await client.GetServiceAsync(new GetServiceRequest
{
    Name = "ms-orders"
});

Console.WriteLine($"Service: {response.Service.Name}");
```

## ⚙️ Configuration

### Core Configuration

Configure the microservice in `appsettings.json`:

```json
{
  "Core": {
    "Id": "ba9595d1-a68c-47bb-8a6d-136e04a92557",
    "PathBase": "/ms-services",
    "AppName": "ms-services",
    "TypeEntryPoint": "rest",
    "Version": "v1",
    "Description": "Microservice to manage the services catalog",
    "Business": "CodeDesignPlus",
    "Contact": {
      "Name": "CodeDesignPlus",
      "Email": "support@codedesignplus.com"
    }
  }
}
```

### MongoDB Configuration

```json
{
  "Mongo": {
    "Enable": true,
    "Database": "db-ms-services",
    "ConnectionString": "mongodb://localhost:27017",
    "Diagnostic": {
      "Enable": false,
      "EnableCommandText": false
    }
  }
}
```

### Redis Configuration

```json
{
  "Redis": {
    "Instances": {
      "Core": {
        "ConnectionString": "localhost:6379"
      }
    }
  },
  "RedisCache": {
    "Enable": true,
    "Expiration": "00:05:00"
  }
}
```

### RabbitMQ Configuration

```json
{
  "RabbitMQ": {
    "Enable": true,
    "Host": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "EnableDiagnostic": false
  }
}
```

### Security Configuration

```json
{
  "Security": {
    "IncludeErrorDetails": true,
    "ValidateAudience": true,
    "ValidateIssuer": true,
    "ValidateLifetime": true,
    "RequireHttpsMetadata": true,
    "ValidIssuer": "https://your-identity-server.com",
    "ValidAudiences": ["services-api"],
    "Applications": [],
    "ValidateLicense": false,
    "ValidateRbac": false,
    "ServerRbac": "http://localhost:5001",
    "RefreshRbacInterval": 10
  }
}
```

### Vault Configuration

```json
{
  "Vault": {
    "Enable": true,
    "Address": "http://localhost:8200",
    "AppName": "ms-services",
    "Solution": "security-codedesignplus",
    "Token": "root",
    "Mongo": {
      "Enable": true,
      "TemplateConnectionString": "mongodb://{0}:{1}@localhost:27017"
    },
    "RabbitMQ": {
      "Enable": true
    }
  }
}
```

### Observability Configuration

```json
{
  "Observability": {
    "Enable": true,
    "ServerOtel": "http://localhost:4317",
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "GrpcClient": true,
      "CodeDesignPlusSdk": true,
      "Redis": true,
      "RabbitMQ": true
    },
    "Metrics": {
      "Enable": true,
      "AspNetCore": true
    }
  },
  "Logger": {
    "Enable": true,
    "OTelEndpoint": "http://localhost:4317",
    "Level": "Warning"
  }
}
```

### Multi-tenancy

The microservice supports multi-tenancy through the `X-Tenant` header. Each request must include a tenant ID:

```http
X-Tenant: 9588813a-7bc0-4be4-a169-293061881cc3
```

Services are isolated by tenant at the repository level.

### Environment Variables

Key environment variables for Docker/Kubernetes deployment:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
MONGO_CONNECTION_STRING=mongodb://mongo:27017
REDIS_CONNECTION_STRING=redis:6379
RABBITMQ_HOST=rabbitmq
VAULT_ADDRESS=http://vault:8200
VAULT_TOKEN=your-vault-token
```

## 🎯 Use Cases & Scenarios

### 1. Service Registry on Startup
Microservices self-register on startup to maintain an up-to-date catalog.

```csharp
// On microservice startup
public async Task RegisterServiceAsync()
{
    var serviceDto = new CreateServiceDto
    {
        Id = Guid.NewGuid(),
        Name = "ms-orders",
        Description = "Order Management Microservice"
    };
    
    await _httpClient.PostAsJsonAsync("/api/service", serviceDto);
    
    // Register controllers
    var controllerDto = new AddControllerDto
    {
        IdController = Guid.NewGuid(),
        Name = "OrderController",
        Description = "Handles order CRUD operations"
    };
    
    await _httpClient.PostAsJsonAsync($"/api/service/{serviceDto.Id}/controller", controllerDto);
}
```

**Flow**:
```bash
# Step 1: Microservice starts
ms-orders startup → Initialization

# Step 2: Register service
POST /api/service
- Name: "ms-orders"
- Description: "Order Management Microservice"

# Step 3: Register controllers
POST /api/service/{id}/controller
- Name: "OrderController"

# Step 4: Register actions
POST /api/service/{id}/controller/{controllerId}/action
- Name: "CreateOrder"
- HttpMethod: POST
```

### 2. API Documentation Generation
Query the service catalog to generate dynamic API documentation.

```bash
# Retrieve all services and their endpoints
GET /api/service?filter=isActive eq true

# Use response to generate:
# - OpenAPI/Swagger specifications
# - API Gateway routing rules
# - Postman collections
# - Client SDK generation
```

**Output Example**:
```
Service: ms-orders
  Controller: OrderController
    - POST /api/order (CreateOrder)
    - GET /api/order/{id} (GetOrderById)
    - PUT /api/order/{id} (UpdateOrder)
    - DELETE /api/order/{id} (DeleteOrder)
```

### 3. Service Discovery for API Gateway
API Gateway queries the service catalog to route requests dynamically.

```csharp
// API Gateway discovers available services
public async Task<IEnumerable<ServiceDto>> DiscoverServicesAsync()
{
    var response = await _httpClient.GetAsync("/api/service?filter=isActive eq true");
    var services = await response.Content.ReadFromJsonAsync<PaginatedResult<ServiceDto>>();
    
    // Update routing table
    foreach (var service in services.Data)
    {
        foreach (var controller in service.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                // Register route: /{service}/{controller}/{action}
                RegisterRoute(service.Name, controller.Name, action.Name, action.HttpMethod);
            }
        }
    }
    
    return services.Data;
}
```

### 4. Batch Controller Registration
Register multiple controllers at once during service initialization.

```bash
# Register multiple controllers in one request
POST /api/service/{id}/controllers
[
  {
    "idController": "guid-1",
    "name": "OrderController",
    "description": "Order operations"
  },
  {
    "idController": "guid-2",
    "name": "ProductController",
    "description": "Product operations"
  }
]
```

### 5. Service Health Dashboard
Build a service health dashboard by querying the catalog.

```bash
# Get all registered services
GET /api/service

# For each service, check health endpoint
GET /{serviceName}/health

# Display service status:
# ✅ ms-orders (Healthy)
# ✅ ms-products (Healthy)
# ⚠️ ms-payments (Degraded)
# ❌ ms-inventory (Unhealthy)
```

### 6. Multi-Tenant Service Isolation
Separate service catalogs for different tenants.

```bash
# Tenant A registers their services
POST /api/service
Headers: X-Tenant: tenant-a-id
- Name: "ms-orders-tenant-a"

# Tenant B registers their services (isolated)
POST /api/service
Headers: X-Tenant: tenant-b-id
- Name: "ms-orders-tenant-b"

# Queries are automatically scoped by tenant
GET /api/service
Headers: X-Tenant: tenant-a-id
# Returns only tenant A's services
```

### 7. Service Versioning
Track different versions of services in the catalog.

```bash
# Register service v1
POST /api/service
{
  "name": "ms-orders-v1",
  "description": "Order Management Microservice v1.0"
}

# Register service v2 (new version)
POST /api/service
{
  "name": "ms-orders-v2",
  "description": "Order Management Microservice v2.0 with new features"
}

# Query by version
GET /api/service?filter=name contains 'v2'
```

## 🏗️ Architecture

### Clean Architecture Layers

```
src/
├── domain/                          # Domain Layer
│   ├── Domain/                      # Aggregates, Entities, Value Objects
│   │   ├── ServiceAggregate.cs     # Main aggregate root
│   │   ├── Entities/               # ControllerEntity, ActionEntity
│   │   ├── Enums/                  # HttpMethod enum
│   │   ├── DomainEvents/           # ServiceCreated, ControllerAdded, etc.
│   │   └── Repositories/           # IServiceRepository
│   ├── Application/                 # Application Layer
│   │   ├── Commands/               # CreateService, AddController, etc.
│   │   ├── Queries/                # GetById, GetAll, GetByName
│   │   ├── DTOs/                   # ServiceDto, ControllerDto, ActionDto
│   │   └── Validators/             # FluentValidation rules
│   └── Infrastructure/              # Infrastructure Layer
│       └── Repositories/           # MongoDB implementation
└── entrypoints/                     # Presentation Layer
    ├── Rest/                        # REST API
    │   ├── Controllers/            # ServiceController
    │   └── Program.cs              # Startup configuration
    └── gRpc/                        # gRPC API
        ├── Services/               # ServiceService
        ├── Protos/                 # service.proto
        └── Program.cs              # Startup configuration
```

### CQRS Pattern

**Commands** (Write operations):
- `CreateServiceCommand` - Register new service
- `UpdateServiceCommand` - Update service metadata
- `DeleteServiceCommand` - Soft delete service
- `AddControllerCommand` - Add controller to service
- `AddControllersCommand` - Batch add controllers
- `UpdateControllerCommand` - Update controller metadata
- `RemoveControllerCommand` - Remove controller
- `AddActionCommand` - Add action to controller
- `AddActionsCommand` - Batch add actions
- `UpdateActionCommand` - Update action metadata
- `RemoveActionCommand` - Remove action

**Queries** (Read operations):
- `GetServiceByIdQuery` - Get service by unique ID
- `GetServiceByNameQuery` - Get service by name
- `GetAllServiceQuery` - List services with pagination/filtering

### Domain Model

```
ServiceAggregate (Root)
│
├── Id: Guid
├── Name: string
├── Description: string
├── IsActive: bool
├── CreatedAt: Instant
├── CreatedBy: Guid
├── UpdatedAt: Instant?
├── UpdatedBy: Guid?
│
└── Controllers: List<ControllerEntity>
    │
    ├── Id: Guid
    ├── Name: string
    ├── Description: string
    │
    └── Actions: List<ActionEntity>
        │
        ├── Id: Guid
        ├── Name: string
        ├── Description: string
        └── HttpMethod: HttpMethod (GET/POST/PUT/DELETE/PATCH)
```

### Domain Events

Published to RabbitMQ after successful operations:

**Service Events**:
- `ServiceCreatedDomainEvent` - Service registered
- `ServiceUpdatedDomainEvent` - Service metadata updated
- `ServiceDeletedDomainEvent` - Service soft deleted

**Controller Events**:
- `ControllerAddedDomainEvent` - Controller added to service
- `ControllerUpdatedDomainEvent` - Controller metadata updated
- `ControllerRemovedDomainEvent` - Controller removed from service

**Action Events**:
- `ActionAddedDomainEvent` - Action added to controller
- `ActionUpdatedDomainEvent` - Action metadata updated
- `ActionRemovedDomainEvent` - Action removed from controller

### Service Registration Flow

```
Microservice starts
     ↓
[Startup Logic] → Gathers service metadata
     ↓
[HTTP Client] → POST /api/service
     ↓
[ServiceController] → Validates request
     ↓
[CreateServiceCommand] → Creates ServiceAggregate
     ↓
[CreateServiceCommandHandler] → Validates business rules
     ↓
[ServiceRepository] → Persists to MongoDB
     ↓
[IPubSub] → Publishes ServiceCreatedDomainEvent
     ↓
API Gateway listens and updates routing table
```

## 🧪 Testing

### Unit & Integration Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/unit/CodeDesignPlus.Net.Microservice.Services.Domain.Test

# Run REST integration tests
dotnet test tests/integration/CodeDesignPlus.Net.Microservice.Services.Rest.Test

# Run gRPC integration tests
dotnet test tests/integration/CodeDesignPlus.Net.Microservice.Services.gRpc.Test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportsDirectory=./coverage
```

### Test Structure

```
tests/
├── unit/
│   ├── Domain.Test/           # Domain logic tests
│   ├── Application.Test/      # Command/Query handler tests
│   ├── Infrastructure.Test/   # Repository tests
│   ├── Rest.Test/             # REST controller unit tests
│   └── gRpc.Test/             # gRPC service unit tests
└── integration/
    ├── Rest.Test/             # REST API integration tests
    └── gRpc.Test/             # gRPC API integration tests
```

### Example Integration Test

```csharp
[Fact]
public async Task CreateService_ValidRequest_ReturnsNoContent()
{
    // Arrange
    var client = _factory.CreateClient();
    var serviceDto = new CreateServiceDto
    {
        Id = Guid.NewGuid(),
        Name = "ms-test-service",
        Description = "Test service for integration testing"
    };
    
    // Act
    var response = await client.PostAsJsonAsync("/api/service", serviceDto);
    
    // Assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    
    // Verify persistence
    var getResponse = await client.GetAsync($"/api/service/{serviceDto.Id}");
    Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
}
```

### Manual Testing with Postman

Import the Postman collection from `docs/postman/` for manual testing.

### Testing with grpcurl

```bash
# Test CreateService
grpcurl -plaintext -d '{
  "Service": {
    "Id": "550e8400-e29b-41d4-a716-446655440000",
    "Name": "ms-test",
    "Description": "Test Service",
    "Controllers": []
  }
}' localhost:5001 Service.Service/CreateService

# Test GetService
grpcurl -plaintext -d '{
  "name": "ms-test"
}' localhost:5001 Service.Service/GetService
```

## 💡 Best Practices

### Service Registration

#### ✅ DO: Use meaningful service names
```csharp
// Good: Clear, kebab-case naming
var service = new CreateServiceDto
{
    Name = "ms-order-management",
    Description = "Order Management Microservice"
};
```

#### ✅ DO: Register services on startup
```csharp
public class Startup
{
    public async Task ConfigureAsync(IApplicationBuilder app)
    {
        // Register service in catalog on startup
        await RegisterServiceMetadataAsync();
        
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
```

#### ✅ DO: Keep service names unique per tenant
```csharp
// Enforce unique names in the domain
public static ServiceAggregate Create(Guid id, string name, string description)
{
    DomainGuard.IsNullOrEmpty(name, Errors.InvalidName);
    
    // Repository should enforce uniqueness:
    // if (await _repository.FindServiceByNameAsync(name) != null)
    //     throw new DuplicateServiceException(name);
    
    return new ServiceAggregate(id) { Name = name, Description = description };
}
```

#### ✅ DO: Use batch operations for multiple controllers
```csharp
// Efficient: Add multiple controllers at once
var controllers = new List<AddControllerDto>
{
    new() { Name = "OrderController", Description = "..." },
    new() { Name = "ProductController", Description = "..." }
};

await _httpClient.PostAsJsonAsync($"/api/service/{serviceId}/controllers", controllers);
```

#### ❌ DON'T: Register duplicate services
```csharp
// Bad: Will cause conflicts
await CreateServiceAsync(new CreateServiceDto { Name = "ms-orders" });
await CreateServiceAsync(new CreateServiceDto { Name = "ms-orders" }); // ❌ Duplicate
```

#### ❌ DON'T: Use generic names
```csharp
// Bad: Not descriptive
var service = new CreateServiceDto
{
    Name = "service1",  // ❌ Too generic
    Description = "A service"  // ❌ Not informative
};

// Good: Clear and descriptive
var service = new CreateServiceDto
{
    Name = "ms-order-management",
    Description = "Handles order creation, updates, and fulfillment workflows"
};
```

### Controller & Action Management

#### ✅ DO: Use consistent naming conventions
```csharp
// Follow REST conventions
var action = new AddActionDto
{
    Name = "CreateOrder",      // PascalCase for action names
    Description = "Creates a new order",
    HttpMethod = HttpMethod.POST
};
```

#### ✅ DO: Provide meaningful descriptions
```csharp
// Good: Clear description of what the action does
var action = new AddActionDto
{
    Name = "UpdateOrderStatus",
    Description = "Updates the status of an existing order (e.g., Pending → Shipped)",
    HttpMethod = HttpMethod.PUT
};
```

#### ❌ DON'T: Mix HTTP methods incorrectly
```csharp
// Bad: Using GET for state-changing operations
var action = new AddActionDto
{
    Name = "DeleteOrder",
    HttpMethod = HttpMethod.GET  // ❌ Should be DELETE
};
```

### Security

1. **Always require authentication** for service catalog modifications
2. **Use tenant isolation** to prevent cross-tenant access
3. **Validate input** using FluentValidation
4. **Audit changes** with CreatedBy/UpdatedBy tracking
5. **Implement rate limiting** on catalog endpoints
6. **Use HTTPS only** in production

### Error Handling

```csharp
// Handle service not found gracefully
try
{
    var service = await _mediator.Send(new GetServiceByIdQuery(serviceId));
    return Ok(service);
}
catch (ServiceNotFoundException ex)
{
    return Problem(
        title: "Service Not Found",
        detail: $"Service with ID {serviceId} does not exist in the catalog.",
        statusCode: 404
    );
}
```

## 🐛 Troubleshooting

### Common Issues

#### Issue: "Service name already exists" error
**Cause**: Attempting to create a service with a duplicate name within the same tenant.

**Solution**:
```bash
# Check existing services
GET /api/service?filter=name eq 'ms-orders'

# Use a unique name or update the existing service
PUT /api/service/{existingId}
```

#### Issue: Controller not found when adding actions
**Cause**: Trying to add an action to a controller that doesn't exist.

**Solution**:
```bash
# Verify controller exists
GET /api/service/{serviceId}

# Add controller first
POST /api/service/{serviceId}/controller
{
  "idController": "guid",
  "name": "OrderController",
  "description": "..."
}

# Then add actions
POST /api/service/{serviceId}/controller/{controllerId}/action
```

#### Issue: MongoDB connection timeout
**Cause**: MongoDB not accessible or wrong connection string.

**Solution**:
```bash
# Test MongoDB connectivity
mongosh "mongodb://localhost:27017"

# Check Docker containers
docker ps | grep mongo

# Verify connection string in appsettings.json
"Mongo": {
  "ConnectionString": "mongodb://localhost:27017",
  "Database": "db-ms-services"
}
```

#### Issue: Services not visible across tenants
**Cause**: Multi-tenancy isolation is working correctly (this is expected behavior).

**Solution**:
```bash
# Verify you're using the correct X-Tenant header
curl -H "X-Tenant: tenant-a-id" http://localhost:5000/api/service

# If you need to see services across tenants, query separately:
curl -H "X-Tenant: tenant-a-id" http://localhost:5000/api/service
curl -H "X-Tenant: tenant-b-id" http://localhost:5000/api/service
```

#### Issue: gRPC service not responding
**Cause**: gRPC endpoint not properly configured or firewall blocking port.

**Solution**:
```bash
# Test gRPC endpoint with grpcurl
grpcurl -plaintext localhost:5001 list

# Check if port 5001 is open
netstat -an | grep 5001

# Verify appsettings.json for gRPC
"Kestrel": {
  "Endpoints": {
    "Http2": {
      "Url": "http://*:5001",
      "Protocols": "Http2"
    }
  }
}
```

### Debug Mode

Enable detailed logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "CodeDesignPlus": "Trace",
      "CodeDesignPlus.Net.Microservice.Services": "Trace",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Health Checks

Check service health:
```bash
curl http://localhost:5000/health
```

Expected response:
```json
{
  "status": "Healthy",
  "checks": [
    { "name": "MongoDB", "status": "Healthy" },
    { "name": "Redis", "status": "Healthy" },
    { "name": "RabbitMQ", "status": "Healthy" }
  ]
}
```

## 🔍 Service Discovery

### Dynamic Service Discovery

The Services microservice enables dynamic service discovery for API Gateways, load balancers, and monitoring tools.

**Query all active services**:
```bash
GET /api/service?filter=isActive eq true
```

**Use cases**:
- **API Gateway**: Update routing tables dynamically
- **Load Balancer**: Discover available service instances
- **Monitoring**: Track registered services for health checks
- **Documentation**: Generate Swagger/OpenAPI specs from catalog

### Service Mesh Integration

Integrate with service meshes (e.g., Istio, Linkerd):

```yaml
# Kubernetes ServiceEntry generated from catalog
apiVersion: networking.istio.io/v1beta1
kind: ServiceEntry
metadata:
  name: ms-orders
spec:
  hosts:
  - ms-orders.default.svc.cluster.local
  ports:
  - number: 5000
    name: http
    protocol: HTTP
  resolution: DNS
```

## 📊 Domain Model

### ServiceAggregate

The `ServiceAggregate` is the root entity representing a microservice in the catalog.

**Properties**:
- `Id` (Guid) - Unique identifier
- `Name` (string) - Service name (unique per tenant)
- `Description` (string) - Service description
- `IsActive` (bool) - Active status
- `Controllers` (List<ControllerEntity>) - Registered controllers
- `CreatedAt` (Instant) - Creation timestamp
- `CreatedBy` (Guid) - User who created the service
- `UpdatedAt` (Instant?) - Last update timestamp
- `UpdatedBy` (Guid?) - User who last updated the service

**Methods**:
- `Create()` - Factory method to create a new service
- `Update()` - Update service metadata
- `AddController()` - Add a controller to the service
- `UpdateController()` - Update controller metadata
- `RemoveController()` - Remove a controller
- `AddAction()` - Add an action to a controller
- `UpdateAction()` - Update action metadata
- `RemoveAction()` - Remove an action
- `Delete()` - Soft delete the service

### ControllerEntity

Represents a controller (REST endpoint group) within a service.

**Properties**:
- `Id` (Guid) - Unique identifier
- `Name` (string) - Controller name
- `Description` (string) - Controller description
- `Actions` (List<ActionEntity>) - Available actions

### ActionEntity

Represents an action (HTTP endpoint) within a controller.

**Properties**:
- `Id` (Guid) - Unique identifier
- `Name` (string) - Action name
- `Description` (string) - Action description
- `HttpMethod` (HttpMethod) - HTTP verb (GET, POST, PUT, DELETE, PATCH)

### HttpMethod Enum

```csharp
public enum HttpMethod
{
    None,
    POST,
    PUT,
    DELETE,
    PATCH,
    GET
}
```

## 🔒 Security

### Authentication & Authorization

The microservice uses OAuth2/OpenID Connect with JWT Bearer tokens:

```http
Authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Required Claims**:
- `sub` (Subject) - User identifier
- `tenant` - Tenant identifier
- `scope` - Required scopes (e.g., `services:read`, `services:write`)

### Multi-Tenancy Isolation

All operations are scoped by the `X-Tenant` header:

```http
X-Tenant: 9588813a-7bc0-4be4-a169-293061881cc3
```

**Isolation Guarantees**:
- Services are filtered by tenant at repository level
- Cross-tenant queries are prevented
- Each tenant has an isolated service catalog

### Audit Logging

All state changes are tracked:
- `CreatedBy` - Who created the service/controller/action
- `UpdatedBy` - Who last modified it
- `CreatedAt` / `UpdatedAt` - When changes occurred

### Best Practices

1. **Rotate JWT signing keys** regularly
2. **Use HTTPS only** in production
3. **Implement rate limiting** to prevent abuse
4. **Validate tenant headers** on every request
5. **Monitor for suspicious activity** (e.g., rapid service registrations)
6. **Regular security audits** of service catalog integrity

## ❓ FAQ

### Q: Can I register services from any microservice?
**A**: Yes, any authenticated service can register itself or other services in the catalog. However, ensure proper authorization policies are in place.

### Q: What happens if a service is deleted?
**A**: Services are soft-deleted (marked as inactive). They remain in the database but won't appear in queries unless explicitly filtered.

### Q: How do I version services?
**A**: Include version information in the service name (e.g., `ms-orders-v1`, `ms-orders-v2`) or use the `Description` field to track versions.

### Q: Can I query services across tenants?
**A**: No, the multi-tenancy isolation prevents cross-tenant queries. Each tenant has a completely isolated service catalog.

### Q: What's the difference between gRPC and REST APIs?
**A**: 
- **REST**: Human-readable JSON, browser-compatible, easier debugging
- **gRPC**: Binary protocol buffers, faster, lower bandwidth, better for service-to-service communication

### Q: How do I handle service discovery in Kubernetes?
**A**: Query the catalog and generate Kubernetes `ServiceEntry` or `VirtualService` resources dynamically.

### Q: Can I add custom metadata to services?
**A**: Currently, the schema supports `Name` and `Description`. For custom metadata, extend the domain model or use the `Description` field to store JSON.

### Q: What happens if two services have the same name?
**A**: Within the same tenant, duplicate names are prevented by validation. Different tenants can have services with the same name (isolated).

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository** and create a feature branch
2. **Follow coding conventions** (Clean Architecture, DDD, CQRS)
3. **Write unit tests** for new features
4. **Update documentation** (README, API docs)
5. **Submit a pull request** with a clear description

### Code Style

- Use **C# 13** language features
- Follow **SOLID principles**
- Use **nullable reference types**
- Write **XML documentation** for public APIs
- Use **FluentValidation** for input validation

### Commit Messages

```
feat: Add batch controller registration endpoint
fix: Resolve duplicate service name validation
docs: Update API endpoint documentation
test: Add integration tests for gRPC service
```

## 📄 License

This project is licensed under the **GNU Lesser General Public License v3.0** - see the [LICENSE.md](LICENSE.md) file for details.

## 🔧 Utility Tools

The repository includes several utility scripts in the `tools/` directory:

### Convert Line Endings
```bash
./tools/convert-crlf-to-lf.sh
```

### Update NuGet Packages
```bash
cd tools/update-packages
./update-packages.sh
```

### Upgrade .NET Version
```bash
cd tools/upgrade-dotnet
./upgrade-assistant.sh
```

### Configure Vault Secrets
```bash
cd tools/vault
./config-vault.sh
```

### SonarQube Analysis
```bash
cd tools/sonarqube
./sonar.sh
```

## 📦 CodeDesignPlus Packages

This microservice uses the **CodeDesignPlus.Net.Sdk** package to simplify development. For more information:

- **Documentation**: [https://codedesignplus.github.io/](https://codedesignplus.github.io/)
- **NuGet Packages**: [https://www.nuget.org/profiles/CodeDesignPlus](https://www.nuget.org/profiles/CodeDesignPlus)
- **GitHub Organization**: [https://github.com/codedesignplus](https://github.com/codedesignplus)

## 📞 Support

- **Email**: support@codedesignplus.com
- **Website**: [https://codedesignplus.com](https://codedesignplus.com)
- **GitHub Issues**: [Report an issue](https://github.com/codedesignplus/CodeDesignPlus.Net.Microservice.Services/issues)

---

**Built with ❤️ by CodeDesignPlus**

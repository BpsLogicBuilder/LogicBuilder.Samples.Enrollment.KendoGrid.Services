# LogicBuilder.Samples.Enrollment.KendoGrid.Services

[![CI](https://github.com/BpsLogicBuilder/LogicBuilder.Samples.Enrollment.KendoGrid.Services/actions/workflows/ci.yml/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.Samples.Enrollment.KendoGrid.Services/actions/workflows/ci.yml)
[![CodeQL](https://github.com/BpsLogicBuilder/LogicBuilder.Samples.Enrollment.KendoGrid.Services/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.Samples.Enrollment.KendoGrid.Services/actions/workflows/github-code-scanning/codeql)
[![codecov](https://codecov.io/github/BpsLogicBuilder/LogicBuilder.Samples.Enrollment.KendoGrid.Services/graph/badge.svg?token=AYPBULI4MG)](https://codecov.io/github/BpsLogicBuilder/LogicBuilder.Samples.Enrollment.KendoGrid.Services)
[![Quality gate status](https://sonarcloud.io/api/project_badges/measure?project=BpsLogicBuilder_LogicBuilder.Samples.Enrollment.KendoGrid.Services&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=BpsLogicBuilder_LogicBuilder.Samples.Enrollment.KendoGrid.Services)

## Overview

This repository demonstrates a .NET microservices architecture for supporting Kendo UI Grid operations with a structured request-response pattern. The solution provides a scalable approach to handling complex data grid scenarios using Entity Framework Core and AutoMapper.

## Architecture

The solution follows a two-tier microservices architecture:

### 1. **API Layer** (`Enrollment.KendoGrid.Api`)
- Public-facing API service
- Handles client requests and forwards them to the BSL service
- Implements CORS policies for secure cross-origin access
- Uses Azure Key Vault for certificate management
- Configures HTTP client with mutual TLS authentication

### 2. **Business Service Layer (BSL)** (`Enrollment.KendoGrid.Bsl`)
- Internal service handling grid data requests
- Certificate-based authentication for secure API-to-BSL communication
- Processes `KendoGridDataRequest` objects supporting:
  - Filtering
  - Sorting
  - Paging
  - Grouping
  - Aggregation
- Returns `DataSourceResult` compatible with Kendo UI Grid

## Solution Structure
```
├── API/
│   └── Enrollment.KendoGrid.Api          # Public API service
├── BSL/
│   ├── Enrollment.BSL.AutoMapperProfiles # AutoMapper configurations
│   └── Enrollment.KendoGrid.Bsl          # Business service layer
├── Business/
│   ├── Enrollment.Domain                 # Domain models (.NET Standard 2.0)
│   └── Enrollment.Repositories           # Repository pattern implementations
├── EF/
│   ├── Enrollment.Contexts               # Entity Framework DbContext
│   ├── Enrollment.Data                   # Data models
│   └── Enrollment.Stores                 # Data access stores
└── Tests/
    ├── Enrollment.KendoGrid.Api.Tests    # API layer tests
    └── Enrollment.KendoGrid.Bsl.Tests    # BSL layer tests
```


## Key Features

- **Generic Grid Support**: Works with all types configured for EF Core and AutoMapper
- **Structured Requests**: Type-safe `KendoGridDataRequest` handling
- **Clean Architecture**: Separation of concerns with clear layer boundaries
- **Entity Framework Core**: SQL Server database integration with retry logic
- **AutoMapper Integration**: Automatic mapping between domain and data models
- **Docker Support**: Multi-container deployment with docker-compose
- **CI/CD Ready**: GitHub Actions workflows for continuous integration and deployment
- **Certificate-Based Security**: Mutual TLS between API and BSL layers
- **Azure Integration**: Key Vault for certificate management

## Technologies

- **.NET 10**: Modern .NET runtime
- **.NET Standard 2.0**: Domain layer for maximum compatibility
- **Entity Framework Core 10**: Data access and ORM
- **AutoMapper 16**: Object-to-object mapping
- **Kendo.Mvc.UI**: Kendo UI Grid integration
- **Azure Key Vault**: Secure credential storage
- **Docker**: Containerization support

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server
- Docker (optional, for containerized deployment)
- Azure subscription (for Key Vault integration)

### Configuration

Configure the following settings in `appsettings.json` or environment variables:

#### API Service
- `AllowedOrigins`: CORS allowed origins
- `keyVaultUrl`: Azure Key Vault URL
- `bslCertificateName`: Certificate name for BSL authentication
- `baseBslUrl`: Base URL of the BSL service

#### BSL Service
- `ConnectionStrings:DefaultConnection`: SQL Server connection string


## Deployment

The repository includes GitHub Actions workflows for CI/CD:
- `ci.yml`: Continuous integration
- `cd.yml`: Continuous deployment
- `check-for-ab-number.yml`: Azure Boards work item validation

## License

See `LICENSE.txt` for license information.

## Related Packages

This solution uses the following LogicBuilder packages:
- `LogicBuilder.App.KendoGrid.Bsl.Business`: Core business logic for Kendo Grid
- `LogicBuilder.App.KendoGrid.Bsl.Utils`: Utility functions for grid operations
- `LogicBuilder.App.Utils`: Common application utilities
- `LogicBuilder.Attributes`: Domain model attributes
- `LogicBuilder.Domain`: Domain base classes
- `LogicBuilder.EntityFrameworkCore.Mapping`: EF Core mapping extensions
- `LogicBuilder.EntityFrameworkCore.Repositories`: Repository pattern support
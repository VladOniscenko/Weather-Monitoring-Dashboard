# Database Migrations Guide

This project uses **Entity Framework Core** with **PostgreSQL**. Migrations are managed via the CLI and stored in the `Weather.Infrastructure` project.

## Prerequisites

1. Install EF Core tools:
   ```bash
   dotnet tool install --global dotnet-ef

    Ensure PostgreSQL is running and the connection string in
    src/Weather.API/appsettings.json is correct.

Common Commands

All commands must be run from the server/ root directory.
1. Create a New Migration

Run after modifying entities or configuration.
```
dotnet ef migrations add <MigrationName> \
  --project src/Weather.Infrastructure \
  --startup-project src/Weather.API \
  --output-dir Persistence/Migrations
```
Example:
```
dotnet ef migrations add AddUserPhoneColumn \
  --project src/Weather.Infrastructure \
  --startup-project src/Weather.API \
  --output-dir Persistence/Migrations
```
2. Update the Database

Apply all pending migrations:
```
dotnet ef database update \
  --project src/Weather.Infrastructure \
  --startup-project src/Weather.API
```
3. Remove the Last Migration

Only if the database has not been updated yet:
```
dotnet ef migrations remove \
  --project src/Weather.Infrastructure \
  --startup-project src/Weather.API
```
4. Drop the Database

Deletes the local database completely:
```
dotnet ef database drop \
  --project src/Weather.Infrastructure \
  --startup-project src/Weather.API
```
Workflow

    Modify domain entities in Weather.Domain/Entities

    (Optional) Update configurations in Weather.Infrastructure/Persistence/Configurations

    Add a migration

    Update the database

Troubleshooting

LINQ could not be translated

    Avoid complex logic not supported by PostgreSQL.

Connection refused

    Verify PostgreSQL is running.

    Check DefaultConnection in appsettings.json.


# TaskTracking

TaskTracking is an ASP.NET Core Web API with a React frontend. The backend currently uses Entity Framework Core with SQLite:

```csharp
options.UseSqlite("Data Source=tasks.db")
```

This repository also includes a helper script for running a local SQL Server instance in Docker. This is useful for learning T-SQL or preparing to switch the backend from SQLite to SQL Server later.

## Prerequisites

- Docker Desktop, OrbStack, or another Docker runtime
- .NET SDK compatible with the project target framework
- Node.js and npm for the frontend

Check Docker:

```bash
docker --version
docker ps
```

## Start SQL Server with Docker

From the repository root:

```bash
cd /Users/xianqiu/Projects/TaskTracking
./scripts/start-sqlserver.sh
```

The script will:

- create the `tasktracking-sqlserver` container if it does not exist
- start the existing container if it already exists
- skip creation if the container is already running
- wait until SQL Server is ready for client connections
- print the connection string

Default SQL Server settings:

```text
Container name: tasktracking-sqlserver
Image: mcr.microsoft.com/mssql/server:2022-latest
Port: 1433
User: sa
Password: YourStrong!Passw0rd
Database name: TaskTrackingDb
```

Connection string:

```text
Server=localhost,1433;Database=TaskTrackingDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
```

To use a different `sa` password when creating the container:

```bash
MSSQL_SA_PASSWORD='MyNewStrong!Passw0rd' ./scripts/start-sqlserver.sh
```

The password must meet SQL Server password complexity requirements.

## Manual Docker Command

The script wraps this Docker command:

```bash
docker run \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" \
  -e "MSSQL_PID=Developer" \
  -p 1433:1433 \
  --name tasktracking-sqlserver \
  --hostname tasktracking-sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

## Common Docker Commands

Check running containers:

```bash
docker ps
```

View SQL Server logs:

```bash
docker logs tasktracking-sqlserver
```

Stop SQL Server:

```bash
docker stop tasktracking-sqlserver
```

Start SQL Server again:

```bash
docker start tasktracking-sqlserver
```

Remove the SQL Server container:

```bash
docker rm -f tasktracking-sqlserver
```

## Connect with sqlcmd

After the container is running, connect with `sqlcmd` inside the container:

```bash
docker exec -it tasktracking-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost \
  -U sa \
  -P "YourStrong!Passw0rd" \
  -C
```

Test the connection:

```sql
SELECT @@VERSION;
GO
```

Create the application database manually:

```sql
CREATE DATABASE TaskTrackingDb;
GO
```

List databases:

```sql
SELECT name FROM sys.databases;
GO
```

Exit:

```sql
EXIT
```

## Switching the Backend to SQL Server

The backend currently uses SQLite. To switch to SQL Server, add the EF Core SQL Server provider:

```bash
cd /Users/xianqiu/Projects/TaskTracking/TaskTracking
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Add a connection string to `TaskTracking/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TaskTrackingDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
}
```

Update `TaskTracking/Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

The existing migrations were generated for SQLite. After switching providers, regenerate migrations for SQL Server:

```bash
dotnet ef migrations remove
dotnet ef migrations remove
dotnet ef migrations add InitialSqlServerCreate
dotnet ef database update
```

This project currently has two migrations, so removing twice may be necessary if you want to replace them with one SQL Server migration.

## Notes for Apple Silicon Macs

SQL Server container images are commonly x64-oriented. On Apple Silicon Macs, Docker may need emulation and the SQL Server container may be slower or less reliable. If local Docker does not work well, use Azure SQL Database as the easiest SQL Server-compatible alternative.

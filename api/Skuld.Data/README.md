Scaffold-DbContext "Server=(localdb)\mssqllocaldb;Database=Skuld;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -Project Skuld.Data -ContextDir ./ -Context SkuldContext -OutputDir Entities -Force

# Documentation

https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

## Pre requisites

- [Installing dotnet-ef tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- .NET 6
- Visual Studio 2022

### Configure target database before execute operations

Add this code to SkuldContext.cs

```csharp
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer ("YOUR_CONNECTION_STRING_HERE");
            }
        }
```

!!! WARNING !!! DO NOT COMMIT THIS WITH CONNECTION STRING, PLEASE !!!

## Update database

This will create a file in **Migrations** folder who contains c# code to apply to our database for our changes.

```powershell
dotnet ef migrations add InitialCreate  #InitialCreate is the migration name present in the generate filename
```

Afterwards, we apply migrations

```powershell
dotnet ef database update
```



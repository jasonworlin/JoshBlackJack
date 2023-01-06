dotnet ef database drop -c UserDb -f
rmdir .\Migrations\ -r
dotnet ef migrations add InitialCreate -c UserDb
dotnet ef database update -c UserDb
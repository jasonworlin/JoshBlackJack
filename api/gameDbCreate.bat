::.\createUSerDb.bat

dotnet ef migrations remove -f -c GameDb
dotnet ef migrations add InitialCreate -c GameDb
dotnet ef database update -c GameDb
1. Add migrations project
dotnet ef migrations add Initial_Identity --project Modules/Identity/VAlgo.Modules.Identity.csproj --startup-project VAlgo.API/VAlgo.API.csproj --output-dir Infrastructure/Persistence/Migrations

2. Update migrations project
dotnet ef database update --project Modules/ProblemClassification/VAlgo.Modules.ProblemClassification.csproj --startup-project VAlgo.API/VAlgo.API.csproj

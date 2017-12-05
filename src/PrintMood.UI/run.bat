@echo off
rem setx ASPNETCORE_ENVIRONMENT "Development"
setx ASPNETCORE_ENVIRONMENT "Production"
rem setx ASPNETCORE_ENVIRONMENT "Development" /M
rem dotnet run
rem dotnet run --configuration Debug
dotnet run --configuration Release --no-launch-profile



# DevsBank
A developer's bank

Code written in C# with .NET 6 and developed with Jetbrains Rider.

# Build
- after cloning the repository just build it with your IDE or use dotnet build command line.
- DevsBank.WebApi is the project hosting the API and it needs to be started for functional testing.

# Test
- tests, unit tests and one integration test, are written in DevsBank.WebApi.Tests.
- funcional testing can be done with Swagger UI once the api starts at url: https://localhost:7227/swagger/index.html
- for functional testing first call the https://localhost:7227/api/v1/Users endpoint in order to get one of the known customers and then use one of those Guids as clientId in the other API calls.

# Authentication
- there is none; out of scope for now. Multiple options available though.

# CI
- each PR will build and run the tests. Only if it is successful will it allow Merge.

# CD
- each push, via PR, into develop will deploy automatically to https://preview-devs-bank.azurewebsites.net/swagger/index.html (Azure WebApp on a free tier so limited to 60 minutes per day.)

- each push, via PR, into main will deploy automatically to https://devs-bank.azurewebsites.net/swagger/index.html (Azure WebApp on a free tier so limited to 60 minutes per day.)

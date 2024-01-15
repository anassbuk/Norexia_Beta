# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

ENV ASPNETCORE_ENVIRONMENT="Test"

# copy csproj and restore as distinct layers

COPY src/BE/Services/Core/Norexia.Core.Application/*.csproj ./src/BE/Services/Core/Norexia.Core.Application/
COPY src/BE/Services/Core/Norexia.Core.Domain/*.csproj ./src/BE/Services/Core/Norexia.Core.Domain/
COPY src/BE/Services/Core/Norexia.Core.Infrastructure/*.csproj ./src/BE/Services/Core/Norexia.Core.Infrastructure/
COPY src/BE/Services/Core/Norexia.Core.WebApi/*.csproj ./src/BE/Services/Core/Norexia.Core.WebApi/

RUN dotnet restore src/BE/Services/Core/Norexia.Core.Application/Norexia.Core.Application.csproj
RUN dotnet restore src/BE/Services/Core/Norexia.Core.Domain/Norexia.Core.Domain.csproj
RUN dotnet restore src/BE/Services/Core/Norexia.Core.Infrastructure/Norexia.Core.Infrastructure.csproj
RUN dotnet restore src/BE/Services/Core/Norexia.Core.WebApi/Norexia.Core.WebApi.csproj

#copy everything else and build app
COPY src/BE/Services/Core/Norexia.Core.Application/. ./src/BE/Services/Core/Norexia.Core.Application/
COPY src/BE/Services/Core/Norexia.Core.Domain/. ./src/BE/Services/Core/Norexia.Core.Domain/
COPY src/BE/Services/Core/Norexia.Core.Infrastructure/. ./src/BE/Services/Core/Norexia.Core.Infrastructure/
COPY src/BE/Services/Core/Norexia.Core.WebApi/. ./src/BE/Services/Core/Norexia.Core.WebApi/

WORKDIR /source/src/BE/Services/Core/Norexia.Core.WebApi
RUN dotnet publish -c release -o /app --no-restore

### final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Norexia.Core.WebApi.dll"]
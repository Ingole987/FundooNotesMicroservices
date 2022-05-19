#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FundooNotesMicroservices.AzureFunctions.Users/FundooNotesMicroservices.AzureFunctions.Users.csproj", "FundooNotesMicroservices.AzureFunctions.Users/"]
COPY ["FundooNotesMicroservices.Models/FundooNotesMicroservices.Models.csproj", "FundooNotesMicroservices.Models/"]
COPY ["FundooNotesMicroservices.Shared/FundooNotesMicroservices.Shared.csproj", "FundooNotesMicroservices.Shared/"]
RUN dotnet restore "FundooNotesMicroservices.AzureFunctions.Users/FundooNotesMicroservices.AzureFunctions.Users.csproj"
COPY . .
WORKDIR "/src/FundooNotesMicroservices.AzureFunctions.Users"
RUN dotnet build "FundooNotesMicroservices.AzureFunctions.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FundooNotesMicroservices.AzureFunctions.Users.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
# Use the official .NET runtime as a base image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Set the SDK image for building the app.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TotalHRInsightAPI/TotalHRInsightAPI.csproj", "TotalHRInsightAPI/"]
COPY ["TotalHRInsight.DAL/TotalHRInsight.DAL.csproj", "TotalHRInsight.DAL/"]
COPY ["TotalHRInsight.DTO/TotalHRInsight.DTO.csproj", "TotalHRInsight.DTO/"]
RUN dotnet restore "TotalHRInsightAPI/TotalHRInsightAPI.csproj"
COPY . .
WORKDIR "/src/TotalHRInsightAPI"
RUN dotnet build "TotalHRInsightAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TotalHRInsightAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TotalHRInsightAPI.dll"]

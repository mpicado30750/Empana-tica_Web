# Usar la imagen base de ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Usar la imagen SDK para la construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Asegúrate de copiar los archivos csproj en las rutas correctas y restaurar
COPY ["TotalHRInsight.API/TotalHRInsight.API.csproj", "TotalHRInsight.API/"]
COPY ["TotalHRInsight.API/TotalHRInsight.DAL/TotalHRInsight.DAL.csproj", "TotalHRInsight.API/TotalHRInsight.DAL/"]
COPY ["TotalHRInsight.API/TotalHRInsight.DTO/TotalHRInsight.DTO.csproj", "TotalHRInsight.API/TotalHRInsight.DTO/"]
RUN dotnet restore "TotalHRInsight.API/TotalHRInsight.API.csproj"

# Copiar todos los demás archivos del proyecto
COPY . .
WORKDIR "/src/TotalHRInsight.API"
RUN dotnet build "TotalHRInsight.API.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "TotalHRInsight.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Crear la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TotalHRInsight.API.dll"]

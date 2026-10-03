FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["StellarGear.API/StellarGear.API.csproj", "StellarGear.API/"]
COPY ["StellarGear.Application/StellarGear.Application.csproj", "StellarGear.Application/"]
COPY ["StellarGear.Domain/StellarGear.Domain.csproj", "StellarGear.Domain/"]
COPY ["StellarGear.Infrastructure/StellarGear.Infrastructure.csproj", "StellarGear.Infrastructure/"]
RUN dotnet restore "StellarGear.API/StellarGear.API.csproj"

COPY . .
WORKDIR /src/StellarGear.API
RUN dotnet publish "StellarGear.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StellarGear.API.dll"]

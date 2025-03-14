# Use the .NET 9 SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CharitySale.Api/CharitySale.Api.csproj", "CharitySale.Api/"]
RUN dotnet restore "CharitySale.Api/CharitySale.Api.csproj"

# Copy the rest of the code and build
COPY . .
WORKDIR "/src/CharitySale.Api"
RUN dotnet build "CharitySale.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CharitySale.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port 80
EXPOSE 80

ENTRYPOINT ["dotnet", "CharitySale.Api.dll"]
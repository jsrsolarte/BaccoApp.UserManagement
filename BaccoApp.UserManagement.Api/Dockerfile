FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BaccoApp.UserManagement.Api/BaccoApp.UserManagement.Api.csproj", "BaccoApp.UserManagement.Api/"]
RUN dotnet restore "BaccoApp.UserManagement.Api/BaccoApp.UserManagement.Api.csproj"
COPY . .
WORKDIR "/src/BaccoApp.UserManagement.Api"
RUN dotnet build "BaccoApp.UserManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaccoApp.UserManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaccoApp.UserManagement.Api.dll"]

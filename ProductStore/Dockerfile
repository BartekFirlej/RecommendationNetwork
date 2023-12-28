#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProductStore/ProductStore.csproj", "ProductStore/"]
RUN dotnet restore "ProductStore/ProductStore.csproj"
COPY . .
WORKDIR "/src/ProductStore"
RUN dotnet build "ProductStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductStore.dll"]
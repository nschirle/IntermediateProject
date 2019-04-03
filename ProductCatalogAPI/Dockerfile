FROM microsoft/dotnet:2.1-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
 
FROM microsoft/dotnet:2.1-sdk-stretch AS build
WORKDIR /src
COPY . ./
RUN dotnet restore ProductCatalogAPI.csproj


FROM build AS publish
RUN dotnet publish "ProductCatalogAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProductCatalogAPI.dll"]
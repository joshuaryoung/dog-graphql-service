FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./hotChocolateDemo.csproj" --disable-parallel
RUN dotnet publish "./hotChocolateDemo.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT [ "dotnet", "hotChocolateDemo.dll" ]
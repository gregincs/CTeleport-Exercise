FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY src/CTeleport.Exercise.Domain/CTeleport.Exercise.Domain.csproj ./src/CTeleport.Exercise.Domain/
COPY src/CTeleport.Exercise.Application/CTeleport.Exercise.Application.csproj ./src/CTeleport.Exercise.Application/
COPY src/CTeleport.Exercise.Infrastructure/CTeleport.Exercise.Infrastructure.csproj ./src/CTeleport.Exercise.Infrastructure/
COPY src/CTeleport.Exercise.Api/CTeleport.Exercise.Api.csproj ./src/CTeleport.Exercise.Api/
RUN dotnet restore ./src/CTeleport.Exercise.Api/*.csproj

COPY . ./
RUN dotnet publish ./src/CTeleport.Exercise.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_HTTP_PORT=https://+:5001
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "CTeleport.Exercise.Api.dll"]

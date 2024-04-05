
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TagsAPI.IntegrationTests/TagsAPI.IntegrationTests.csproj", "TagsAPI.IntegrationTests/"]
RUN dotnet restore "TagsAPI.IntegrationTests/TagsAPI.IntegrationTests.csproj"
COPY . .
WORKDIR "/src/TagsAPI.IntegrationTests"
RUN dotnet build "TagsAPI.IntegrationTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS testrunner
WORKDIR /src
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

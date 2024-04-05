
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TagsAPI.UnitTests/TagsAPI.UnitTests.csproj", "TagsAPI.UnitTests/"]
RUN dotnet restore "TagsAPI.UnitTests/TagsAPI.UnitTests.csproj"
COPY . .
WORKDIR "/src/TagsAPI.UnitTests"
RUN dotnet build "TagsAPI.UnitTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS testrunner
WORKDIR /src
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

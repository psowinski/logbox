FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./logbox.csproj", "logbox/"]
RUN dotnet restore "logbox/logbox.csproj"
COPY [".", "logbox/"]
WORKDIR "/src/logbox"
RUN dotnet build "logbox.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "logbox.csproj" -c Release -o /app/publish

FROM base AS final
LABEL author="github.com/psowinski"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "logbox.dll"]
# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . .
WORKDIR /src
RUN dotnet restore ./DataSetExplorer/DataSetExplorer.csproj
RUN dotnet build DataSetExplorer/DataSetExplorer.csproj -c Release

FROM build AS publish
RUN dotnet publish DataSetExplorer/DataSetExplorer.csproj -c Release -o /app/publish

ENV ASPNETCORE_URLS=http://*:$PORT
FROM base AS final
COPY --from=publish /app .
WORKDIR /app/publish
CMD ["dotnet", "DataSetExplorer.dll"]


FROM build as migration-base
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef --version 5.0.6

FROM migration-base AS execute-migration

ENV STARTUP_PROJECT=DataSetExplorer
ENV MIGRATION=init
ENV DATABASE_SCHEMA=""
ENV DATABASE_HOST=""
ENV DATABASE_PASSWORD=""
ENV DATABASE_USERNAME=""

CMD PATH="$PATH:/root/.dotnet/tools" \
    dotnet-ef migrations add "${MIGRATION}-dse" \
        -s "${STARTUP_PROJECT}/${STARTUP_PROJECT}.csproj" \
        -p "${STARTUP_PROJECT}/${STARTUP_PROJECT}.csproj" \
        -c "DataSetExplorerContext" \
        --configuration Release && \
    PATH="$PATH:/root/.dotnet/tools" \   
    dotnet-ef database update "${MIGRATION}-dse" \
        -s "${STARTUP_PROJECT}/${STARTUP_PROJECT}.csproj" \
        -p "${STARTUP_PROJECT}/${STARTUP_PROJECT}.csproj" \
        -c "DataSetExplorerContext" \
        --configuration Release
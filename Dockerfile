ARG ASPNET_VERSION=3.1
ARG SDK_VERSION=3.1

FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_VERSION} AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:${SDK_VERSION} as build
ARG PROJECT
WORKDIR /src
COPY . .
RUN dotnet restore "${PROJECT}/${PROJECT}.csproj" && \
    dotnet build "${PROJECT}/${PROJECT}.csproj" -c Release -o /app/build
 
FROM build AS publish
ARG PROJECT
RUN dotnet publish "${PROJECT}/${PROJECT}.csproj" -c Release -o /app/publish 

FROM base AS final
ARG PROJECT
ENV PROJECT=${PROJECT}
WORKDIR /app
COPY --from=publish /app .
WORKDIR /app/publish
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ${PROJECT}.dll

FROM build as migration
RUN PATH="$PATH:/root/.dotnet/tools"; \
    dotnet tool install --global dotnet-ef && \
    dotnet-ef dbcontext script -p "${PROJECT}/${PROJECT}.csproj" -o sql/init.sql
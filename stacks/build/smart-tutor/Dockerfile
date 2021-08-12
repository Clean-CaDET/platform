# syntax=docker/dockerfile:1

ARG ASPNET_VERSION=5.0
ARG SDK_VERSION=5.0

FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_VERSION} AS base
WORKDIR /app
 

FROM mcr.microsoft.com/dotnet/sdk:${SDK_VERSION} AS build
ENV PROJECT=SmartTutor
WORKDIR /src
COPY . .
RUN dotnet restore "${PROJECT}/${PROJECT}.csproj" && \
    dotnet build "${PROJECT}/${PROJECT}.csproj" -c Release
 

FROM build AS publish
ENV PROJECT=SmartTutor
RUN dotnet publish "${PROJECT}/${PROJECT}.csproj" -c Release -o /app/publish 


FROM base AS final
ENV ASPNETCORE_URLS=http://*:$PORT
COPY --from=publish /app .
WORKDIR /app/publish
CMD ["dotnet", "SmartTutor.dll"]


FROM build AS migration-base
RUN PATH="$PATH:/root/.dotnet/tools"; \
    dotnet tool install --global dotnet-ef


# Following stages require to be run in network where database is running
# and currently BuildKit does not support running container during build
# in a custom network: https://github.com/moby/moby/issues/40379.
# Workaround is to build image and run the container from that image
# in desired network.

FROM migration-base AS execute-migration

ENV PROJECT=SmartTutor
ENV MIGRATION=init
ENV DATABASE_HOST=""
ENV DATABASE_PASSWORD=""
ENV DATABASE_USERNAME=""
ENV DATABASE_SCHEMA=""

CMD PATH="$PATH:/root/.dotnet/tools"; \
    dotnet-ef migrations add ${MIGRATION} \
        -p "${PROJECT}/${PROJECT}.csproj" \
        --configuration Release && \ 
    dotnet-ef database update ${MIGRATION} \
        -p "${PROJECT}/${PROJECT}.csproj" \
        --configuration Release


FROM migration-base AS generate-schema
RUN PATH="$PATH:/root/.dotnet/tools"; \
    dotnet-ef dbcontext script \
        -p "${PROJECT}/${PROJECT}.csproj" \
        -o sql/init.sql

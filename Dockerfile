ARG ASPNET_VERSION=3.1
ARG SDK_VERSION=3.1

FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_VERSION} AS base
WORKDIR /app
 
FROM mcr.microsoft.com/dotnet/sdk:${SDK_VERSION} as build
ARG SRC_URL=https://github.com/Clean-CaDET/platform/archive/refs/heads/master.tar.gz
ARG PROJECT
WORKDIR /src
RUN apt update && apt install curl tar && \
    mkdir ../downloads && cd ../downloads && \
    curl -L ${SRC_URL} | tar -xz && \ 
    mv $(ls -d */|head -n 1) app && \
    mv app/* /src && cd /src \
    dotnet restore "${PROJECT}/${PROJECT}.csproj" && \
    dotnet build "${PROJECT}/${PROJECT}.csproj" -c Release -o /app/build
 
FROM build AS publish
ARG PROJECT
RUN dotnet publish "${PROJECT}/${PROJECT}.csproj" -c Release -o /app/publish 

FROM base AS final
ARG PROJECT
ENV PROJECT=${PROJECT}
COPY --from=publish /app .
WORKDIR /app/publish
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ${PROJECT}.dll

FROM build as migration
RUN PATH="$PATH:/root/.dotnet/tools"; \
    dotnet tool install --global dotnet-ef && \
    dotnet-ef dbcontext script -p "${PROJECT}/${PROJECT}.csproj" -o sql/init.sql
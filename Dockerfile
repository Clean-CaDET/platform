# syntax=docker/dockerfile:1

FROM python as python
RUN python -m venv /venv
RUN /venv/bin/python -m pip install networkx
RUN /venv/bin/python -m pip install infomap

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.12 AS base
WORKDIR /app
 
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ENV PROJECT=DataSetExplorer
WORKDIR /src
COPY . .
RUN dotnet restore "${PROJECT}/${PROJECT}.csproj" && \
    dotnet build "${PROJECT}/${PROJECT}.csproj" -c Release
 
FROM build AS publish
ENV PROJECT=DataSetExplorer
RUN dotnet publish "${PROJECT}/${PROJECT}.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=http://*:$PORT
COPY --from=publish /app .
WORKDIR /app/publish
COPY ./scripts/community_calculation.py ./Core/CommunityDetection/scripts
RUN apk add build-base
RUN apk add python3-dev
ENV PYTHONUNBUFFERED=1
RUN apk add --update --no-cache python3 && ln -sf python3 /usr/bin/python
RUN python3 -m ensurepip
RUN pip3 install --no-cache --upgrade pip setuptools
RUN pip3 install networkx
RUN pip3 install infomap
CMD ["dotnet", "DataSetExplorer.dll"]

FROM build AS migration-base
RUN PATH="$PATH:/root/.dotnet/tools"; \
    dotnet tool install --global dotnet-ef


# Following stages require to be run in network where database is running
# and currently BuildKit does not support running container during build
# in a custom network: https://github.com/moby/moby/issues/40379.
# Workaround is to build image and run the container from that image
# in desired network.

FROM migration-base AS execute-migration

ENV PROJECT=DataSetExplorer
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
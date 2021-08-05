# Setup

This document provides steps that needs to be taken in order to successfully setup the infrastructure using Docker and Docker Compose.

Setup contains 4 crutial steps:
 - Building Images
 - Running Services 
 - Data Migration
 - Data Ingest

Initial setup requires all 4 steps to be perform. Every other setup requires only second step (Running Services) to be performed.

**Note**: All commands require to be executed in `stacks` directory!

## Building Images

First step is to build images for Smart Tutor and Gateway services. Build configuration is set in `config/env.conf` file. 

```shell
docker-compose --env-file config/env.conf build
```

Default configuration creates `cleancadet/smart-tutor:lates` and `cleancadet/gateway:latest` images.

Also build Smart Tutor migration image that creates required database schema for Smart Tutor service.

```shell
docker-compose --env-file config/env.conf --file docker-compose.migration.yml build
```

Previos command creates `cleancadet/smart-tutor:migration-latest` image.

## Running Services

When images are built, next step is to run services. Runtime configuration is also set in `config/env.conf` file.

```shell
docker-compose --env-file config/env.conf up
```

**Note**: If `KEYCLOAK_DATABASE_SCHEMA` variable is updated make sure to also update `config/database-init/keycloak.sql` script.

## Data Migration

Run the migration script for Smart Tutor service:

```shell
docker-compose --env-file config/env.conf --file docker-compose.migration.yml run smart-tutor-migration
```
 

The services are accessable on http://127.0.0.1:8080 address. By default database schema for Smart Tutor service is empty. If data for that service needs to be ingested with alredy prepared data, take a look at Data Ingest section.

## Data Ingest

The next command ingest data into database used by Smart Tutor service:

```shell
docker-compose --env-file config/env.conf exec --user postgres database psql -f /tmp/smart-tutor-init.sql
```

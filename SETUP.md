# Setup

This document provides steps that needs to be taken in order to successfully setup the infrastructure using Docker and Docker Compose.
The infrastructure can be setup with or without Keycloak service (which provides SSO). In that matter there are two separate section 
which describes infrastructure setup weather the Keycloak service is used.

**Note**: For quick setup it is recommended to setup the infrastructure without
SSO and Keycloak service because it needs to be configured manually with instructions
provided in _KEYCLOAK_SETUP.md_ document.

## 1. Setup without SSO

Setup contains 4 crucial steps:
 - Building Images
 - Running Services 
 - Data Migration
 - Data Ingest

Initial setup requires all 4 steps to be perform. Every other setup requires only second step (Running Services) to be performed.

**Note**: All commands require to be executed in `stacks` directory!

**Note**: All configuration for running and building services is set 
in `stacks/config/env.conf`!

### 1.1. Building Images

First step is to build images for Smart Tutor and Gateway services. 

```shell
docker-compose --env-file config/env.conf build
```

Default configuration creates `cleancadet/smart-tutor:latest` and `cleancadet/gateway:latest` images.

Also build Smart Tutor migration image that creates required database schema for Smart Tutor service.

```shell
docker-compose --env-file config/env.conf \
  --file docker-compose.migration.yml \
  build
```

Previous command creates `cleancadet/smart-tutor:migration-latest` image.

### 1.2. Running Services

When images are built, next step is to run services.

```shell
docker-compose --env-file config/env.conf up
```

### 1.3. Data Migration

Run the migration script for Smart Tutor service:

```shell
docker-compose --env-file config/env.conf \
  --file docker-compose.migration.yml \
  run smart-tutor-migration
```

The services are accessible on http://127.0.0.1:8080 address. By default database schema for Smart Tutor service is empty. If data for that service needs to be ingested with alredy prepared data, take a look at Data Ingest section.

### 1.4. Data Ingest

The next command ingest data into database used by Smart Tutor service:

```shell
docker-compose --env-file config/env.conf \
  exec \
    --user postgres \
    database \
    psql -f /tmp/smart-tutor-init.sql
```

### 1.5. Destroy Infrastructure

In order to remove the previously created infrastructure, the following command should be executed:

```shell
docker-compose --env-file config/env.conf down -v
```

Previous command completely destroys infrastructure. If data needs to be kept remove the `-v` option.
Keep in mind that if data are not removed there is no need to run migration and data ingestion.

## 2. Setup with SSO

Setup contains 4 crucial steps:
- Building Images
- Running Services
- Data Migration
- Data Ingest

Initial setup requires all 4 steps to be perform. Every other setup requires only second step (Running Services) to be performed.

**Note**: All commands require to be executed in `stacks` directory!

**Note**: All configuration for running and building services is set
in `stacks/config/keycloak-env.conf`!

### 2.1. Building Images

First step is to build images for Smart Tutor and Gateway services.

```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.yml \
  --file docker-compose.keycloak.yml \
  build
```
Default configuration creates `cleancadet/smart-tutor:latest` and `cleancadet/gateway:keycloak-latest` images.

Also build Smart Tutor migration image that creates required database schema for Smart Tutor service.

```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.migration.yml \
  build
```

### 2.2. Running Services

When images are built, next step is to run services.

```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.yml \
  --file docker-compose.keycloak.yml \
  up
```

**Note**: If `KEYCLOAK_DATABASE_SCHEMA` variable is updated make sure to also update `config/database-init/keycloak.sql` script.

### 2.3. Data Migration

Run the migration script for Smart Tutor service:

```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.migration.yml \
  run smart-tutor-migration
```

The services are accessible on http://127.0.0.1:8080 address. By default database schema for Smart Tutor service is empty. If data for that service needs to be ingested with alredy prepared data, take a look at Data Ingest section.

### 2.4. Data Ingest

The next command ingest data into database used by Smart Tutor service:

```shell
docker-compose --env-file config/keycloak-env.conf \
  exec \
    --user postgres \
    database \
    psql -f /tmp/smart-tutor-init.sql
```

### 2.5. Destroy 

```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.yml \
  --file docker-compose.keycloak.yml \
  down -v
```

Previous command completely destroys infrastructure. If data needs to be kept remove the `-v` option.
Keep in mind that if data are not removed there is no need to run migration and data ingestion.
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
docker-compose --env-file config/env.conf up --detach
```

**Note**: Make sure that database service is ready to be used by checking 
database logs.

#### Database Logs
Run the following command:
```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.yml \
  --file docker-compose.keycloak.yml \
  logs database
```

Last lines of the output should be like this:

```
database_1     | 2021-08-31 11:42:52.832 UTC [1] LOG:  listening on IPv4 address "0.0.0.0", port 5432
database_1     | 2021-08-31 11:42:52.832 UTC [1] LOG:  listening on IPv6 address "::", port 5432
database_1     | 2021-08-31 11:42:52.838 UTC [1] LOG:  listening on Unix socket "/var/run/postgresql/.s.PGSQL.5432"
database_1     | 2021-08-31 11:42:52.846 UTC [85] LOG:  database system was shut down at 2021-08-31 11:42:52 UTC
database_1     | 2021-08-31 11:42:52.852 UTC [1] LOG:  database system is ready to accept connections
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
  up --detach
```
**Note**: If `KEYCLOAK_DATABASE_SCHEMA` variable is updated make sure to also update `config/database-init/keycloak.sql` script.

**Note**: Make sure that keycloak and database service are ready to be used
by checking their logs.

#### Database Logs
Run the following command:
```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.yml \
  --file docker-compose.keycloak.yml \
  logs database
```

Last lines of the output should be like this:

```
database_1     | 2021-08-31 11:42:52.832 UTC [1] LOG:  listening on IPv4 address "0.0.0.0", port 5432
database_1     | 2021-08-31 11:42:52.832 UTC [1] LOG:  listening on IPv6 address "::", port 5432
database_1     | 2021-08-31 11:42:52.838 UTC [1] LOG:  listening on Unix socket "/var/run/postgresql/.s.PGSQL.5432"
database_1     | 2021-08-31 11:42:52.846 UTC [85] LOG:  database system was shut down at 2021-08-31 11:42:52 UTC
database_1     | 2021-08-31 11:42:52.852 UTC [1] LOG:  database system is ready to accept connections
```

#### Keycloak Logs
Run the following command:
```shell
docker-compose --env-file config/keycloak-env.conf \
  --file docker-compose.yml \
  --file docker-compose.keycloak.yml \
  logs keycloak
```

Last lines of the output should be like this:
```
keycloak_1     | 11:43:25,372 INFO  [org.jboss.as.server] (Controller Boot Thread) WFLYSRV0212: Resuming server
keycloak_1     | 11:43:25,375 INFO  [org.jboss.as] (Controller Boot Thread) WFLYSRV0025: Keycloak 13.0.1 (WildFly Core 15.0.1.Final) started in 21965ms - Started 693 of 978 services (686 services are lazy, passive or on-demand)
keycloak_1     | 11:43:25,377 INFO  [org.jboss.as] (Controller Boot Thread) WFLYSRV0060: Http management interface listening on http://127.0.0.1:9990/management
keycloak_1     | 11:43:25,378 INFO  [org.jboss.as] (Controller Boot Thread) WFLYSRV0051: Admin console listening on http://127.0.0.1:9990
```


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
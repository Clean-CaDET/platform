[![Gitter](https://badges.gitter.im/Clean-CaDET/community.svg)](https://gitter.im/Clean-CaDET/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# Clean CaDET Overview
The vision and idea behind Clean CaDET is described in this [overview video](https://www.youtube.com/watch?v=fBENFfjC49A).

Clean CaDET started as a project funded by the [Science Fund of the Republic of Serbia](http://fondzanauku.gov.rs/?lang=en). It hopes to grow into an active open-source community dedicated to software engineers' growth and their pursuit to build sustainable, high-quality software.

For an overview of the platform and its composing elements, check the [wiki pages](https://github.com/Clean-CaDET/platform/wiki).

### Requirements
- Docker
- Docker Compose
- libjson-xs-perl (Ubuntu package) 

### Environment Setup

In order to run infrastructure the following environment variables needs to be set:
```shell
set +o history # (disable bash history in order to protect sensitive data)
export DATABASE_PASSWORD=<value>
export DATABASE_SCHEMA=<value>
export DATABASE_USERNAME=<value>
export KEYCLOAK_DATABASE_PASSWORD=<value> # (same value as DATABASE_PASSWORD)
export KEYCLOAK_DATABASE_USER=<value> # (same value as DATABASE_USERNAME)
export KEYCLOAK_PASSWORD=<value>
export KEYCLOAK_USER=<value> # (admin is preferable)
set -o history # (enable bash history when work with sensitive data is done)
```
    
### Building Services

Running the complete infrastructure requires several container images need to be set first 
(skip this section if images have been already build).

#### Smart Tutor

In order to build **Smart Tutor** service, the following commands needs to be executed:
```shell
pushd smart-tutor/stacks/application/smart-tutor
./build.sh # (provide --help option for documentation)
popd
```

If no arguments are provided, by default **cleancadet/smart-tutor** image is created.

#### API Gateway

In order to build **API Gateway** service, the following commands needs to be executed:
```shell
pushd smart-tutor/stacks/public/gateway/build.sh
./build.sh # (provide --help option for documentation)
popd
```

If no arguments are provided, by default **cleancadet/gateway** image is created.

### Infrastructure Setup

Infrastructure setup requires environment setup and services to already being build 
(take a look at previous sections). In order to set up the complete infrastructure,
the following commands needs to be executed:
```shell
pushd smart-tutor
./deploy-all-stacks.sh
popd
``` 

The infrastructure contains of following services:
- PostgreSQL 
- Keycloak
- Smart Tutor
- API Gateway

By default, Smart Tutor application is exposed on `127.0.0.1:8080` endpoint.
Currently, Keycloak configuration needs to be done manually. Also, data that 
is used by services needs to be ingested manually (take a 
look ath Database Setup section).


In order to remove the infrastructure, run the following command:
```shell
pushd smart-tutor
./destroy-all-stacks.sh
popd
```

### Database Setup

Infrastructure setup creates necessary services, schemas and data needs to be ingested
separately.  

Keycloak by default requires `keycloak` schema in the database that needs 
to be created manually.  

Smart Tutor requires migration to be executed. In order to run migraion on
the database execute following commands:
```shell
pushd smart-tutor/stacks/application/smart-tutor
./migration.sh \ 
  -m migration \
  -d database \
  -u <value from DATABASE_USERNAME variable> \
  -p <value from DATABASE_PASSWORD variable> \
  -S smart-tutor \
  -n clean_cadet_persistence_dev
# migration.sh script accept many environment variables 
# that can be used to store sensitive data
# run migration.sh --help for documentation
popd
```
   



    
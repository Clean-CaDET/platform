#! /bin/bash

show_help() {
    echo "********** OPTIONS **********"
    printf "  %s\n" "-s|--stage                    stage/environment"
    printf "  %s\n" "-e|--environment-file         path to configuration template file which utilizes environment variables, default: ../env.conf.template"
    printf "  %s\n" "-c|--compose-file             path to docker compose file, default: ../application.yml"
    printf "  %s\n" "--keycloak-database-user      keycloak database username that must be specified by using this option or by setting KEYCLOAK_DATABASE_USER variable"
    printf "  %s\n" "--keycloak-database-password  keycloak database password that must be specified by using this option or by setting KEYCLOAK_DATABASE_PASSWORD variable"
    printf "  %s\n" "--keycloak-user               keycloak username that must be specified by using this option or by setting KEYCLOAK_USER variable"
    printf "  %s\n" "--keycloak-password           keycloak password that must be specified by using this option or by setting KEYCLOAK_PASSWORD variable"
}


POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -e|--environment-file)
    ENVIRONMENT_TEMPLATE_FILE="$2"
    shift 2
    ;;
    -s|--stage)
    STAGE="$2"
    shift 2
    ;;
    -c|--compose-file)
    COMPOSE_FILE="$2"
    shift 2
    ;;
    --keycloak-database-password)
    KEYCLOAK_DATABASE_PASSWORD="$2"
    shift 2
    ;;
    --keycloak-database-user)
    KEYCLOAK_DATABASE_USER="$2"
    shift 2
    ;;
    --keycloak-password)
    KEYCLOAK_PASSWORD="$2"
    shift 2
    ;;
    --keycloak-user)
    KEYCLOAK_USER="$2"
    shift 2
    ;;
    -h|--help)
    shift 2
    show_help
    exit 0
    ;;
    *)    
    POSITIONAL+=("$1")
    shift
    ;;
esac
done
set -- "${POSITIONAL[@]}"


KEYCLOAK_DATABASE_PASSWORD=${KEYCLOAK_DATABASE_PASSWORD}
KEYCLOAK_DATABASE_USER=${KEYCLOAK_DATABASE_USER}
KEYCLOAK_USER=${KEYCLOAK_USER}
KEYCLOAK_PASSWORD=${KEYCLOAK_PASSWORD}
export STAGE=${STAGE:-dev}
STACK_NAME="clean_cadet_application_${STAGE}"
COMPOSE_FILE=${COMPOSE_FILE:-../application.yml}
ENVIRONMENT_TEMPLATE_FILE=${ENVIRONMENT_TEMPLATE_FILE:-../env.conf.template}
ENVIRONMENT_FILE=./env.${STAGE}.conf

if [[ -z ${KEYCLOAK_DATABASE_PASSWORD} ]]; then
    echo "Keycloak database password must be set!"
    echo "Use --keycloak-database-password option or set KEYCLOAK_DATABASE_PASSWORD variable."
    exit 1
fi

if [[ -z ${KEYCLOAK_DATABASE_USER} ]]; then
    echo "Keycloak database user must be set!"
    echo "Use --keycloak-database-user option or set KEYCLOAK_DATABASE_USER variable."
    exit 1
fi

if [[ -z ${KEYCLOAK_USER} ]]; then
    echo "Keycloak user must be set!"
    echo "Use --keycloak-user option or set KEYCLOAK_USER variable."
    exit 1
fi

if [[ -z ${KEYCLOAK_PASSWORD} ]]; then
    echo "Keycloak password must be set!"
    echo "Use --keycloak-password option or set KEYCLOAK_PASSWORD variable."
    exit 1
fi

echo "********** CONFIGURATION **********"
echo "STAGE                     | ${STAGE}"
echo "ENVIRONMENT TEMPLATE FILE | ${ENVIRONMENT_TEMPLATE_FILE}"
echo "STACK NAME                | ${STACK_NAME}"
echo "COMPOSE FILE              | ${COMPOSE_FILE}"

printf "%s" "${KEYCLOAK_DATABASE_PASSWORD}" \
    | docker secret create "clean_cadet_keycloak_database_password_${STAGE}" - > /dev/null || exit
printf "%s" "${KEYCLOAK_DATABASE_USER}" \
    | docker secret create "clean_cadet_keycloak_database_user_${STAGE}" - > /dev/null || exit
printf "%s" "${KEYCLOAK_USER}" \
    | docker secret create "clean_cadet_keycloak_user_${STAGE}" - > /dev/null || exit
printf "%s" "${KEYCLOAK_PASSWORD}" \
    | docker secret create "clean_cadet_keycloak_password_${STAGE}" - > /dev/null || exit


envsubst < "${ENVIRONMENT_TEMPLATE_FILE}" > "${ENVIRONMENT_FILE}"
docker-compose --env-file "${ENVIRONMENT_FILE}" \
               --file "${COMPOSE_FILE}" config \
               | docker stack deploy -c - "${STACK_NAME}"
rm "${ENVIRONMENT_FILE}"

#! /bin/bash

show_help() {
    echo "********** OPTIONS **********"
    printf "  %s\n" "-s|--stage              stage/environment"
    printf "  %s\n" "-e|--environment-file   path to configuration template file which utilizes environment variables, default: ../env.conf.template"
    printf "  %s\n" "-c|--compose-file       path to docker compose file, default: ../persistence.yml"
    printf "  %s\n" "-u|--database-username  database username, required"
    printf "  %s\n" "-p|--database-password  database password, required"
    printf "  %s\n" "-S|--database-schema    database schema, required"
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
    -p|--database-password)
    DATABASE_PASSWORD="$2"
    shift 2
    ;;
    -u|--database-username)
    DATABASE_USERNAME="$2"
    shift 2
    ;;
    -S|--database-schema)
    DATABASE_SCHEMA="$2"
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


DATABASE_PASSWORD=${DATABASE_PASSWORD}
DATABASE_USERNAME=${DATABASE_USERNAME}
DATABASE_SCHEMA=${DATABASE_SCHEMA}
export STAGE=${STAGE:-dev}
STACK_NAME="clean_cadet_persistence_${STAGE}"
COMPOSE_FILE=${COMPOSE_FILE:-../persistence.yml}
ENVIRONMENT_TEMPLATE_FILE=${ENVIRONMENT_TEMPLATE_FILE:-../env.conf.template}
ENVIRONMENT_FILE=./env.${STAGE}.conf

if [[ -z ${DATABASE_PASSWORD} ]]; then
    echo "Database password must be set!"
    echo "Use -p|--database-password option or set DATABASE_PASSWORD variable."
    exit 1
fi

if [[ -z ${DATABASE_USERNAME} ]]; then
    echo "Database username must be set!"
    echo "Use -u|--database-username option or set DATABASE_USERNAME variable."
    exit 1
fi

if [[ -z ${DATABASE_SCHEMA} ]]; then
    echo "Database server must be set!"
    echo "Use -S|--database-schema option or set DATABASE_SCHEMA variable."
    exit 1
fi

echo "********** CONFIGURATION **********"
echo "STAGE                     | ${STAGE}"
echo "ENVIRONMENT TEMPLATE FILE | ${ENVIRONMENT_TEMPLATE_FILE}"
echo "STACK NAME                | ${STACK_NAME}"
echo "COMPOSE FILE              | ${COMPOSE_FILE}"

printf "%s" "${DATABASE_PASSWORD}" \
    | docker secret create "clean_cadet_database_password_${STAGE}" - > /dev/null || exit
printf "%s" "${DATABASE_USERNAME}" \
    | docker secret create "clean_cadet_database_username_${STAGE}" - > /dev/null || exit
printf "%s" "${DATABASE_SCHEMA}" \
    | docker secret create "clean_cadet_database_schema_${STAGE}" - > /dev/null || exit


envsubst < "${ENVIRONMENT_TEMPLATE_FILE}" > "${ENVIRONMENT_FILE}"

docker-compose --env-file "${ENVIRONMENT_FILE}" \
               --file "${COMPOSE_FILE}" config \
               | docker stack deploy -c - "${STACK_NAME}"
rm "${ENVIRONMENT_FILE}"

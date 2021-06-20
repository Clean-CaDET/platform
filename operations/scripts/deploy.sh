#! /bin/bash

POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -e|--environment-file)
    ENVIRONMENT_TEMPLATE_FILE="$2"
    shift
    shift
    ;;
    -s|--stage)
    STAGE="$2"
    shift
    shift
    ;;
    -c|--compose-file)
    COMPOSE_FILE="$2"
    shift
    shift
    ;;
    *)    
    POSITIONAL+=("$1")
    shift
    ;;
esac
done
set -- "${POSITIONAL[@]}"


export STAGE=${STAGE:-dev}
STACK_NAME="operations_${STAGE}"
COMPOSE_FILE=${COMPOSE_FILE:-../operations.yml}
ENVIRONMENT_TEMPLATE_FILE=${ENVIRONMENT_TEMPLATE_FILE:-../env.conf.template}
ENVIRONMENT_FILE=./env.${STAGE}.conf


echo "********** CONFIGURATION **********"
echo "STAGE                     | ${STAGE}"
echo "ENVIRONMENT TEMPLATE FILE | ${ENVIRONMENT_TEMPLATE_FILE}"
echo "STACK NAME                | ${STACK_NAME}"
echo "COMPOSE FILE              | ${COMPOSE_FILE}"


envsubst < "${ENVIRONMENT_TEMPLATE_FILE}" > "${ENVIRONMENT_FILE}"
docker-compose --env-file "${ENVIRONMENT_FILE}" \
               --file "${COMPOSE_FILE}" config \
               | docker stack deploy -c - "${STACK_NAME}"
rm "${ENVIRONMENT_FILE}"


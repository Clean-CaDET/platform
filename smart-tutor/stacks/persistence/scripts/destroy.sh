#! /bin/bash

POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -s|--stage)
    STAGE="$2"
    shift 2
    ;;
    *)    
    POSITIONAL+=("$1")
    shift
    ;;
esac
done
set -- "${POSITIONAL[@]}"

STAGE=${STAGE:-dev}
STACK_NAME="clean_cadet_persistence_${STAGE}"

docker stack rm "${STACK_NAME}"
docker secret rm "clean_cadet_database_password_${STAGE}"
docker secret rm "clean_cadet_database_username_${STAGE}"
docker secret rm "clean_cadet_database_schema_${STAGE}"
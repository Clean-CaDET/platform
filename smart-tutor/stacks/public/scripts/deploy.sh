#! /bin/bash

show_help() {
    echo "********** OPTIONS **********"
    printf "  %s\n" "-s|--stage              stage/environment"
    printf "  %s\n" "-e|--environment-file   path to configuration template file which utilizes environment variables, default: ../env.conf.template"
    printf "  %s\n" "-c|--compose-file       path to docker compose file, default: ../public.yml"
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


export STAGE=${STAGE:-dev}
STACK_NAME="clean_cadet_public_${STAGE}"
COMPOSE_FILE=${COMPOSE_FILE:-../public.yml}
ENVIRONMENT_TEMPLATE_FILE=${ENVIRONMENT_TEMPLATE_FILE:-../env.conf.template}
ENVIRONMENT_FILE=./env.${STAGE}.conf


echo "********** CONFIGURATION **********"
echo "STAGE                     | ${STAGE}"
echo "ENVIRONMENT TEMPLATE FILE | ${ENVIRONMENT_TEMPLATE_FILE}"
echo "STACK NAME                | ${STACK_NAME}"
echo "COMPOSE FILE              | ${COMPOSE_FILE}"

envsubst < "${ENVIRONMENT_TEMPLATE_FILE}" > "${ENVIRONMENT_FILE}"

# Get a list of the keys, names and files of configs and secrets
json_xs -f yaml -t json < "${COMPOSE_FILE}" | \
  jq --raw-output '(.configs,.secrets) | to_entries | map(select(.value | has("file")) | .key, .value.name, .value.file)[]' \
  > configs_and_secrets.txt
# Iterate over each three-tuple
while read -r entry; read -r name; read -r file
do
  pushd "$(dirname "${COMPOSE_FILE}")" > /dev/null || exit
  # Sanitize the variable name for the digest
  sanitized_filename="$(basename "$file" | sed 's/[./ ]/_/g')"
  echo "$entry.name: $sanitized_filename"
  # Get the part of the name without any variable references
  name_without_references="$(env -i envsubst <<< "${name}")"
  remainder=$(( 64 - ${#name_without_references} ))
  # Export a variable with the digest, truncate to a total of 64 characters
  full_name="${sanitized_filename}"_DIGEST="$(sha512sum "$file" | awk '{print $1}' | cut -c -${remainder})"
  popd > /dev/null || exit
  echo "$full_name" >> "${ENVIRONMENT_FILE}"
  echo "Use variable ${sanitized_filename}_DIGEST for $entry"
done < configs_and_secrets.txt
rm configs_and_secrets.txt

docker-compose --env-file "${ENVIRONMENT_FILE}" \
               --file "${COMPOSE_FILE}" config \
               | docker stack deploy --prune -c - "${STACK_NAME}"
rm "${ENVIRONMENT_FILE}"

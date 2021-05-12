#! /bin/bash

show_help() {
    echo "********** OPTIONS **********"
    printf "  %s\n" "-s|--stage        stage/environment"
    printf "  %s\n" "-m|--mode         mode: context or migration, default: context"
    printf "  %s\n" "-d|--database     database host, required in migration mode"
    printf "  %s\n" "-u|--username     database username, required in migration mode"
    printf "  %s\n" "-p|--password     database password, required in migration mode"
    printf "  %s\n" "-S|--schema       database schema, required in migration mode"
    printf "  %s\n" "-n|--network      database network, required in migration mode"
    printf "  %s\n" "-o|--output-dir   output directory where script will be generated when context mode is used, default: ./sql"
    printf "  %s\n" "-f|--output-file  output file (combined with output directory) generated when context mode is used, default: init.sql"
}


POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -s|--stage)
    STAGE="$2"
    shift
    shift
    ;;
    -p|--password)
    PASSWORD="$2"
    shift
    shift
    ;;
    -u|--username)
    USERNAME="$2"
    shift
    shift
    ;;
    -S|--schema)
    SCHEMA="$2"
    shift
    shift
    ;;
    -d|--database)
    DATABASE="$2"
    shift
    shift
    ;;
    -n|--network)
    NETWORK="$2"
    shift
    shift
    ;;
    -o|--output-dir)
    OUTPUT_DIR="$2"
    shift
    shift
    ;;
    -f|--output-file)
    OUTPUT_FILE="$2"
    shift
    shift
    ;;
    -m|--mode)
    MODE="$2"
    shift
    shift
    ;;
    -h|--help)
    shift
    shift
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

if [[ -z ${MODE} ]]; then
    MODE="context"
elif [[ ${MODE} != "context" && ${MODE} != "migration" ]]; then
    echo "Allowed values for mode are: context and migration!"
    exit 1
fi

if [[ ${MODE} == "migration" && -z ${USERNAME} ]]; then
    echo "Database username must be set when using migration mode!"
    echo "Use -u|--username option or set USERNAME variable."
    exit 1
fi

if [[ ${MODE} == "migration" && -z ${PASSWORD} ]]; then
    echo "Database password must be set when using migration mode!"
    echo "Use -p|--password option or set PASSWORD variable."
    exit 1
fi

if [[ ${MODE} == "migration" && -z ${SCHEMA} ]]; then
    echo "Database schema must be set when using migration mode!"
    echo "Use -S|--schema option or set SCHEMA variable."
    exit 1
fi

if [[ ${MODE} == "migration" && -z ${DATABASE} ]]; then
    echo "Database host must be set when using migration mode!"
    echo "Use -d|--database option or set DATABASE variable."
    exit 1
fi

if [[ ${MODE} == "migration" && -z ${NETWORK} ]]; then
    echo "Database network must be set when using migration mode!"
    echo "Use -n|--network option or set NETWORK variable."
    exit 1
fi

export STAGE=${STAGE:-dev}
OUTPUT_DIR=${OUTPUT_DIR:-sql}
OUTPUT_FILE=${OUTPUT_FILE:-init.sql}
USERNAME=${USERNAME}
PASSWORD=${PASSWORD}
SCHEMA=${SCHEMA}
DATABASE=${DATABASE}
NETWORK=${NETWORK}



if [[ ${MODE} == "migration" ]]; then
    IMAGE="cleancadet/smart-tutor:migration-${STAGE}"

    echo "********** CONFIGURATION **********"
    echo "STAGE                     | ${STAGE}"
    echo "MODE                      | ${MODE}"
    echo "IMAGE                     | ${IMAGE}"

    docker build \
        -t "${IMAGE}" \
        --target execute-migration \
        --no-cache .
        # sredi ovo jebeno kesiranje

     docker run --rm \
        -e "DATABASE_HOST=${DATABASE}" \
        -e "DATABASE_USERNAME=${USERNAME}" \
        -e "DATABASE_PASSWORD=${PASSWORD}" \
        -e "DATABASE_SCHEMA=${SCHEMA}" \
        --network "${NETWORK}" \
        "${IMAGE}"

elif [[ ${MODE} == "context" ]]; then
    IMAGE="cleancadet/smart-tutor:context-${STAGE}"

    echo "********** CONFIGURATION **********"
    echo "STAGE                     | ${STAGE}"
    echo "MODE                      | ${MODE}"
    echo "IMAGE                     | ${IMAGE}"
    echo "OUTPUT DIR                | ${OUTPUT_DIR}"
    echo "OUTPUT FILE               | ${OUTPUT_FILE}"

    docker build \
        -t "${IMAGE}" \
        --target generate-schema \
        --no-cache .

    id=$(docker create "${IMAGE}")
    mkdir -p -- "$OUTPUT_DIR"
    docker cp "${id}:/src/sql/init.sql" "${OUTPUT_DIR}/${OUTPUT_FILE}"
    docker rm -v "$id"
fi



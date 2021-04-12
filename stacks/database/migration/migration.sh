DOCKERFILE=${1:-../../..}
PROJECT=${2:-SmartTutor}
OUTPUT_DIR=${3:-sql}
OUTPUT_FILE=${4:-init.sql}

docker build -t cleancadet/smart-tutor-migration --build-arg PROJECT="${PROJECT}" --target migration "${DOCKERFILE}"

id=$(docker create cleancadet/smart-tutor-migration)
mkdir -p -- "$OUTPUT_DIR"
docker cp "${id}:/src/sql/init.sql" "${OUTPUT_DIR}/${OUTPUT_FILE}"
docker rm -v "$id"
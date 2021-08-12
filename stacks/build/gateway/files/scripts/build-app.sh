SMART_TUTOR_API_URL=$1
KEYCLOAK_ON=$2
KEYCLOAK_AUTH_URL=$3
KEYCLOAK_REALM=$4
KEYCLOAK_AUDIENCE=$5

cd app || exit
export API_HOST=${SMART_TUTOR_API_URL}
export KEYCLOAK_ON=${KEYCLOAK_ON}
export KEYCLOAK_AUTH=${KEYCLOAK_AUTH_URL}
export REALM=${KEYCLOAK_REALM}
export AUDIENCE=${KEYCLOAK_AUDIENCE}
envsubst < environment.ts.template > ./src/environments/environment.ts || exit
npm run build --prod && \
cd dist && \
mv "$(find . -maxdepth 1 -type d | tail -n 1)" /app
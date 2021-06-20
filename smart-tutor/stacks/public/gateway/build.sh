#! /bin/bash

show_help() {
    echo "********** OPTIONS **********"
    printf "  %s\n" "-n|--name               image name, default: cleancadet/gateway"
    printf "  %s\n" "-t|--target             target stage in Dockerfile, default: gatewayWithFront"
    printf "  %s\n" "--smart_tutor_api_url   base URL path for Smart Tutor service, default: http://127.0.0.1:8080/smart-tutor/api/"
    printf "  %s\n" "--keycloak_auth_url     Keycloak authorization URL path, default: http://127.0.0.1:8080/keycloak/auth"
    printf "  %s\n" "--keycloak_on           enable Keycloak service, default: true"
    printf "  %s\n" "--keycloak_realm        Keycloak realm, default: master"
    printf "  %s\n" "--keycloak_audience     Keycloak audience, default: demo-app"
    printf "  %s\n" "--frontend_app_src_url  Smart Tutor frontend application source code URL, default: https://github.com/Clean-CaDET/platform-tutor-ui-web/archive/refs/heads/keycloak-login-deploy.tar.gz"
}

POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -n|--name)
    NAME="$2"
    shift 2
    ;;
    -t|--target)
    TARGET="$2"
    shift 2
    ;;
    --smart_tutor_api_url)
    SMART_TUTOR_API_URL="$2"
    shift 2
    ;;
    --keycloak_auth_url)
    KEYCLOAK_AUTH_URL="$2"
    shift 2
    ;;
    --keycloak_on)
    KEYCLOAK_ON="$2"
    shift 2
    ;;
    --keycloak_realm)
    KEYCLOAK_REALM="$2"
    shift 2
    ;;
    --keycloak_audience)
    KEYCLOAK_AUDIENCE="$2"
    shift 2
    ;;
    --frontend_app_src_url)
    FRONTEND_APP_SRC_URL="$2"
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

NAME=${NAME:-cleancadet/gateway}
TARGET=${TARGET:-gatewayWithFront}
SMART_TUTOR_API_URL=${SMART_TUTOR_API_URL:-http://127.0.0.1:8080/smart-tutor/api/}
KEYCLOAK_AUTH_URL=${KEYCLOAK_AUTH_URL:-http://127.0.0.1:8080/keycloak/auth}
KEYCLOAK_ON=${KEYCLOAK_ON:-true}
KEYCLOAK_REALM=${KEYCLOAK_REALM:-master}
KEYCLOAK_AUDIENCE=${KEYCLOAK_AUDIENCE:-demo-app}
FRONTEND_APP_SRC_URL=${FRONTEND_APP_SRC_URL:-https://github.com/Clean-CaDET/platform-tutor-ui-web/archive/refs/heads/keycloak-login-deploy.tar.gz}

docker build -t "${NAME}" \
    --file Dockerfile \
    --target "${TARGET}" \
    --build-arg "SMART_TUTOR_API_URL=${SMART_TUTOR_API_URL}" \
    --build-arg "KEYCLOAK_AUTH_URL=${KEYCLOAK_AUTH_URL}" \
    --build-arg "KEYCLOAK_ON=${KEYCLOAK_ON}" \
    --build-arg "KEYCLOAK_REALM=${KEYCLOAK_REALM}" \
    --build-arg "KEYCLOAK_AUDIENCE=${KEYCLOAK_AUDIENCE}" \
    --build-arg "FRONTEND_APP_SRC_URL=${FRONTEND_APP_SRC_URL}" \
    --no-cache files
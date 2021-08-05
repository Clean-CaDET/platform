# syntax=docker/dockerfile:1

FROM node:15.14.0-alpine as frontBuild
ARG FRONTEND_APP_SRC_URL=https://github.com/Clean-CaDET/platform-tutor-ui-web/archive/refs/heads/master.tar.gz
WORKDIR /src
SHELL ["/bin/ash", "-o", "pipefail", "-c"]
RUN apk --update --no-cache add \
        curl=7.67.0-r4 \
        tar=1.32-r2 \
        gettext=0.20.1-r2 && \
    curl -L ${FRONTEND_APP_SRC_URL} | tar -xz && \
    mv $(ls -d */|head -n 1) app && \
    cd app && \
    npm install
COPY ./files/front/environment.ts.template ./app
ARG SMART_TUTOR_API_URL
ARG KEYCLOAK_AUTH_URL
ARG KEYCLOAK_ON
ARG KEYCLOAK_REALM
ARG KEYCLOAK_AUDIENCE
RUN cd app && \
    export API_HOST=${SMART_TUTOR_API_URL} && \
    export KEYCLOAK_AUTH=${KEYCLOAK_AUTH_URL} && \
    export KEYCLOAK_ON=${KEYCLOAK_ON} && \
    export REALM=${KEYCLOAK_REALM} && \
    export AUDIENCE=${KEYCLOAK_AUDIENCE} && \
    envsubst < environment.ts.template > ./src/environments/environment.ts && \
    npm run build --prod && \
    cd dist && \
    mv $(ls -d */|head -n 1) /app

FROM nginx:1.19.8-alpine as gatewayWithFront
COPY --from=frontBuild /app /usr/share/nginx/html/app
COPY ./files/config/nginx.conf /etc/nginx/nginx.conf
COPY ./files/config/api_gateway.conf /etc/nginx/api_gateway.conf

FROM nginx:1.19.8-alpine as gateway
COPY ./files/config/nginx.conf /etc/nginx/nginx.conf
COPY ./files/config/api_gateway.conf /etc/nginx/api_gateway.conf
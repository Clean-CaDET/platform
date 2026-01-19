# syntax=docker/dockerfile:1

FROM node:15.14.0-alpine AS frontBuild
WORKDIR /src
COPY ./files/scripts/install-dependencies.sh .
ARG FRONTEND_APP_SRC_URL=https://github.com/Clean-CaDET/platform-tutor-ui-web/archive/refs/heads/master.tar.gz
SHELL ["/bin/ash", "-o", "pipefail", "-c"]
RUN chmod +x ./install-dependencies.sh && \
    ./install-dependencies.sh ${FRONTEND_APP_SRC_URL}
COPY ./files/front/environment.ts.template ./app
COPY ./files/scripts/build-app.sh .
ARG SMART_TUTOR_API_URL
ARG KEYCLOAK_AUTH_URL
ARG KEYCLOAK_ON
ARG KEYCLOAK_REALM
ARG KEYCLOAK_AUDIENCE
RUN chmod +x ./build-app.sh && \
    ./build-app.sh ${SMART_TUTOR_API_URL} \
        ${KEYCLOAK_ON} \
        ${KEYCLOAK_AUTH_URL} \
        ${KEYCLOAK_REALM} \
        ${KEYCLOAK_AUDIENCE}

FROM nginx:1.19.8-alpine AS gatewayWithFront
COPY --from=frontBuild /app /usr/share/nginx/html/app
COPY ./files/config/nginx.conf /etc/nginx/nginx.conf
COPY ./files/config/api_gateway.conf /etc/nginx/api_gateway.conf

FROM nginx:1.19.8-alpine AS gatewayWithKeycloak
COPY --from=frontBuild /app /usr/share/nginx/html/app
COPY ./files/config/nginx.conf /etc/nginx/nginx.conf
COPY ./files/config/api_gateway_keycloak.conf /etc/nginx/api_gateway.conf

FROM nginx:1.19.8-alpine AS gateway
COPY ./files/config/nginx.conf /etc/nginx/nginx.conf
COPY ./files/config/api_gateway.conf /etc/nginx/api_gateway.conf
FROM node:15.14.0-alpine as frontBuild
ARG SRC_URL=https://github.com/Clean-CaDET/platform-tutor-ui-web/archive/refs/heads/master.tar.gz
WORKDIR /src
RUN apk --update --no-cache add curl tar && \
    curl -L ${SRC_URL} | tar -xz && \
    mv $(ls -d */|head -n 1) app && \
    cd app && \
    npm install
COPY ./front/environment.ts ./app/src/environments/environment.ts
ARG GATEWAY_URL
ARG KEYCLOAK_AUTH
ARG KEYCLOAK_ON
ARG REALM
ARG AUDIENCE
RUN cd app && \
    sed -i \
      -e "s/{API_HOST}/${GATEWAY_URL}/g" \
      -e "s/{KEYCLOAK_AUTH}/${KEYCLOAK_AUTH}/g" \
      -e "s/{KEYCLOAK_ON}/${KEYCLOAK_ON}/g" \
      -e "s/{REALM}/${REALM}/g" \
      -e "s/{AUDIENCE}/${AUDIENCE}/g" \
      ./src/environments/environment.ts && \
    npm run build --prod && \
    cd dist && \
    mv $(ls -d */|head -n 1) /app

FROM nginx:1.19.8-alpine as gatewayWithFront
COPY --from=frontBuild /app /usr/share/nginx/html/app
RUN cd usr && \
    cd share && \
    cd nginx && \
    cd html && \
    cd app && \
    ls
COPY ./config/nginx.conf /etc/nginx/nginx.conf
COPY ./config/api_gateway.conf /etc/nginx/api_gateway.conf

FROM nginx:1.19.8-alpine as gateway
COPY ./config/nginx.conf /etc/nginx/nginx.conf
COPY ./config/api_gateway.conf /etc/nginx/api_gateway.conf

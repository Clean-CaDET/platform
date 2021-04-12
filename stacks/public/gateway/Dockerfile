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
RUN cd app && \
    sed -i \
      -e "s/{API_HOST}/${GATEWAY_URL}/g" \
      ./src/environments/environment.ts && \
    npm run build --prod && \
    cd dist && \
    mv $(ls -d */|head -n 1) /app

FROM nginx:1.19.8-alpine as gatewayWithFront
COPY --from=frontBuild /app /usr/share/nginx/html/app
COPY ./config/nginx.conf /etc/nginx/nginx.conf
COPY ./config/api_gateway.conf /etc/nginx/api_gateway.conf


FROM nginx:1.19.8-alpine as gateway
COPY ./config/nginx.conf /etc/nginx/nginx.conf
COPY ./config/api_gateway.conf /etc/nginx/api_gateway.conf

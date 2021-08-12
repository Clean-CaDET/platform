FRONTEND_APP_SRC_URL=$1

apk --update --no-cache add \
  curl=7.67.0-r5 \
  tar=1.32-r2 \
  gettext=0.20.1-r2 && \
curl -L "${FRONTEND_APP_SRC_URL}" | tar -xz && \
mv "$(find . -maxdepth 1 -type d | tail -n 1)" app && \
cd app && \
npm install

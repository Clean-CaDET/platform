#! /bin/bash

docker build -t gateway \
    --file Dockerfile \
    --target gateway \
    files

#! /bin/bash

SERVICE=${1:-clean-cadet_smart-tutor}

docker service update --force "${SERVICE}"
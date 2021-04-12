#! /bin/bash
cd ..
echo $(pwd)
docker-compose --env-file config/env.conf config | docker stack deploy -c - clean-cadet
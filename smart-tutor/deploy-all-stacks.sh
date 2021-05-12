#! /bin/bash

echo "********** DEPLOYING PERSISTENCE STACK **********"
pushd stacks/persistence/scripts/ > /dev/null || exit
./deploy.sh
popd > /dev/null || exit

echo "********** DEPLOYING APPLICATION STACK **********"
pushd stacks/application/scripts/ > /dev/null || exit
./deploy.sh
popd > /dev/null || exit

echo "********** DEPLOYING PUBLIC STACK **********"
pushd stacks/public/scripts/ > /dev/null || exit
./deploy.sh
popd > /dev/null || exit


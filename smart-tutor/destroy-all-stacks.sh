#! /bin/bash

echo "********** DESTROYING PUBLIC STACK **********"
./stacks/public/scripts/destroy.sh

echo "********** DESTROYING APPLICATION STACK **********"
./stacks/application/scripts/destroy.sh

echo "********** DESTROYING PERSISTENCE STACK **********"
./stacks/persistence/scripts/destroy.sh
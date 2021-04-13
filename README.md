[![Gitter](https://badges.gitter.im/Clean-CaDET/community.svg)](https://gitter.im/Clean-CaDET/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# Clean CaDET Overview
The vision and idea behind Clean CaDET is described in this [overview video](https://www.youtube.com/watch?v=fBENFfjC49A).

Clean CaDET started as a project funded by the [Science Fund of the Republic of Serbia](http://fondzanauku.gov.rs/?lang=en). It hopes to grow into an active open-source community dedicated to software engineers' growth and their pursuit to build sustainable, high-quality software.

For an overview of the platform and its composing elements, check the [wiki pages](https://github.com/Clean-CaDET/platform/wiki).

In order to build SmartTutor service run following command:
`docker build -t cleancadet/smart-tutor --build-arg PROJECT=SmartTutor --target final .`

In order to build Gateway service run following command (add --no-cache option if layers are chached):
`docker build -t cleancadet/gateway --target gatewayWithFront --build-arg 'GATEWAY_URL=http:\/\/localhost:8080\/smart-tutor\/api\/' .`

In order to build complete infrasturece run following command in `stacks/scripts` directory:
`./start.sh`

In order to update specefic service run following command in `stacks/scripts` directory:
`./update.sh [clean-cadet_gateway|clean-cadet_smart-tutor]`
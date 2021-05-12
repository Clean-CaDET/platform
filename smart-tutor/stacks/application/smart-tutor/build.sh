show_help() {
    echo "********** OPTIONS **********"
    printf "  %s\n" "-n|--name               image name, default: cleancadet/smart-tutor"
    printf "  %s\n" "-t|--target             target stage in Dockerfile, default: final"
    printf "  %s\n" "--asp_version           ASP.NET Core version, default: 5.0"
    printf "  %s\n" "--sdk_version           .NET SDK version, default: 5.0"
    printf "  %s\n" "--src_url               Smart Tutor application source code URL, default: https://github.com/Clean-CaDET/platform/archive/refs/heads/master.tar.gz"
    printf "  %s\n" "-h|--help               display this help end exit"
}

POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -n|--name)
    NAME="$2"
    shift 2
    ;;
    -t|--target)
    TARGET="$2"
    shift 2
    ;;
    --asp)
    ASPNET_VERSION="$2"
    shift 2
    ;;
    --sdk)
    SDK_VERSION="$2"
    shift 2
    ;;
    --src_url)
    SRC_URL="$2"
    shift 2
    ;;
    -h|--help)
    shift 2
    show_help
    exit 0
    ;;
    *)
    POSITIONAL+=("$1")
    shift
    ;;
esac
done
set -- "${POSITIONAL[@]}"

NAME=${NAME:-cleancadet/smart-tutor}
TARGET=${TARGET:-final}
ASPNET_VERSION=${ASPNET_VERSION:5.0}
SDK_VERSION=${SDK_VERSION:5.0}
SRC_URL=${SRC_URL:-https://github.com/Clean-CaDET/platform/archive/refs/heads/master.tar.gz}

docker build -t "${NAME}" \
    --target "${TARGET}" \
    --build-arg "ASPNET_VERSION=${ASPNET_VERSION}" \
    --build-arg "SDK_VERSION=${SDK_VERSION}" \
    --build-arg "SRC_URL=${SRC_URL}" .
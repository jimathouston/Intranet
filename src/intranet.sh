#!/usr/bin/env bash


if [ "$1" = 'help' ]
then
    echo ""
    echo ' ______             __                                              __     '
    echo '|      \           |  \                                            |  \    '
    echo ' \$$$$$$ _______  _| $$_     ______   ______   _______    ______  _| $$_   '
    echo '  | $$  |       \|   $$ \   /      \ |      \ |       \  /      \|   $$ \  '
    echo '  | $$  | $$$$$$$\\$$$$$$  |  $$$$$$\ \$$$$$$\| $$$$$$$\|  $$$$$$\\$$$$$$  '
    echo '  | $$  | $$  | $$ | $$ __ | $$   \$$/      $$| $$  | $$| $$    $$ | $$ __ '
    echo ' _| $$_ | $$  | $$ | $$|  \| $$     |  $$$$$$$| $$  | $$| $$$$$$$$ | $$|  \'
    echo '|   $$ \| $$  | $$  \$$  $$| $$      \$$    $$| $$  | $$ \$$     \  \$$  $$'
    echo ' \$$$$$$ \$$   \$$   \$$$$  \$$       \$$$$$$$ \$$   \$$  \$$$$$$$   \$$$$ '
    echo ""
    echo "Available commands:"
    echo ""
    # Docker-Compose
    # TODO: when tests from compose implemented, don't forget to update help
    echo "Full application in docker-compose:"
    echo "./intranet.ps1 test all       : build and test intranet in docker-compose (NOT IMPLEMENTED YET)"
    echo "./intranet.ps1 run all        : build and start intranet in docker-compose"
    echo "./intranet.ps1 stop all       : stops intranet and removes containers. (docker-compose down)"
    echo ""
    # Docker
    echo "API and Web separately in docker:"
    echo "./intranet.ps1 run api        : build and start api in docker"
    echo "./intranet.ps1 test api       : build and test api in docker"
    echo "./intranet.ps1 bash api       : build api and open interactive shell in docker"
    echo "./intranet.ps1 run web        : build and start web in docker"
    echo "./intranet.ps1 test web       : build and test web in docker"
    echo "./intranet.ps1 bash web       : build web and open interactive shell in docker"
    echo ""
    # Utilities
    echo "Utilities:"
    echo "./intranet.ps1 kill           : stops all containers and removes them. (docker stop and docker rm)"
    echo "./intranet.ps1 help           : prints this message"
    echo ""
    # Return to shell
    exit 0
fi

if [ "$1" = "kill" ]
then
  docker stop $(docker ps -a -q)
  docker rm $(docker ps -a -q)
  exit 0
fi

# Docker-Compose - Entire Intranet
if [ "$1" = "run" ] && [ "$2" = "all" ]
then
  docker-compose build
  docker-compose up
elif [ "$1" = "test" ] && [ "$2" = "all" ]
then
  # TODO: when test via docker-compose is implemented, add command here
  echo "Not implemented yet"
elif [ "$1" = "stop" ] && [ "$2" = "all" ]
then
  docker-compose down
  # Return to shell
  exit 0
fi

# Docker - API
if [ "$1" = "run" ] && [ "$2" = "api" ]
then
  docker build -t api -f Dockerfile-api .
  docker run -v `pwd`:/app -p 3000:80 api run
elif [ "$1" = "test" ] && [ "$2" = "api" ]
then
  docker build -t api -f Dockerfile-api .
  docker run -v `pwd`:/app -p 3000:80 api test
elif [ "$1" = "bash" ] && [ "$2" = "api" ]
then
  docker build -t api -f Dockerfile-api .
  docker run -v `pwd`:/app -p 3000:80 -it api /bin/bash
fi

# Docker - Web
if [ "$1" = "run" ] && [ "$2" = "web" ]
then
  docker build -t web -f Dockerfile-web .
  docker run -v `pwd`:/app -p 5000:80 web run
elif [ "$1" = "test" ] && [ "$2" = "web" ]
then
  docker build -t web -f Dockerfile-web .
  docker run -v `pwd`:/app -p 5000:80 web test
elif [ "$1" = "bash" ] && [ "$2" = "web" ]
then
  docker build -t web -f Dockerfile-web .
  docker run -v `pwd`:/app -p 5000:80 -it web /bin/bash
fi

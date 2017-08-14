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
    echo "./intranet.sh         : Builds intermediate container which runs Publish task in Cake."
    echo "                        Production container is built from published artifacts and starts the Intranet."
    echo "./intranet.sh build   : Builds intermediate container which runs Publish task in Cake."
    echo "                        Production container is built from published artifacts."
    echo "./intranet.sh run     : Starts the production container."
    echo "./intranet.sh kill    : Stops all containers and removes them. (docker stop and docker rm)"
    echo "./intranet.sh help    : Prints this message."
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

if [ "$1" = "build" ]
then
  docker build -t buildweb -f Dockerfile-build .
  docker run -v `pwd`:/app -p 80:80 buildweb
  docker build -t web -f Dockerfile .
elif [ "$1" = "run" ]
then
  docker run -p 80:80 web
else
  docker build -t buildweb -f Dockerfile-build .
  docker run -v `pwd`:/app -p 80:80 buildweb
  docker build -t web -f Dockerfile .
  docker run -d -p 80:80 web
fi

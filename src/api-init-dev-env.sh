#!/usr/bin/env bash

# Set env vars
source development.env
# CAKE target API:Publish
bash build.sh --target API:Publish

set -e

if [ "$1" = 'run' ]
then
  cd Intranet.API
  cd Intranet.API
  dotnet run
fi

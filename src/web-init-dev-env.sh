#!/usr/bin/env bash

# Set env vars
source development.env
# CAKE target Web:Publish
bash build.sh --target Web:Publish

set -e

if [ "$1" = 'run' ]
then
  cd Intranet.Web
  cd Intranet.Web
  dotnet run --watch
fi

# args default is empty string
param([string]$command = "", [string]$target = "")

If ($command -eq "help")
{
  Write-Host ""
  Write-Host ' ______             __                                              __     '
  Write-Host '|      \           |  \                                            |  \    '
  Write-Host ' \$$$$$$ _______  _| $$_     ______   ______   _______    ______  _| $$_   '
  Write-Host '  | $$  |       \|   $$ \   /      \ |      \ |       \  /      \|   $$ \  '
  Write-Host '  | $$  | $$$$$$$\\$$$$$$  |  $$$$$$\ \$$$$$$\| $$$$$$$\|  $$$$$$\\$$$$$$  '
  Write-Host '  | $$  | $$  | $$ | $$ __ | $$   \$$/      $$| $$  | $$| $$    $$ | $$ __ '
  Write-Host ' _| $$_ | $$  | $$ | $$|  \| $$     |  $$$$$$$| $$  | $$| $$$$$$$$ | $$|  \'
  Write-Host '|   $$ \| $$  | $$  \$$  $$| $$      \$$    $$| $$  | $$ \$$     \  \$$  $$'
  Write-Host ' \$$$$$$ \$$   \$$   \$$$$  \$$       \$$$$$$$ \$$   \$$  \$$$$$$$   \$$$$ '
  Write-Host ""
  Write-Host "Available commands:"
  Write-Host ""
  # Docker-Compose
  Write-Host "Full application in docker-compose:"
  Write-Host "./intranet.ps1 debug all      : build and test intranet with local images in docker-compose (NOT IMPLEMENTED YET)"
  Write-Host "./intranet.ps1 run all        : build and start intranet with production images in docker-compose"
  Write-Host "./intranet.ps1 stop all       : stops intranet and removes containers. (docker-compose down)"
  Write-Host ""
  # Docker
  Write-Host "API and Web separately in docker:"
  Write-Host "./intranet.ps1 run api        : build and start dev image of api in docker"
  Write-Host "./intranet.ps1 build api      : build production image from binaries compiled in dev image"
  Write-Host "./intranet.ps1 bash api       : open interactive shell in already running dev image"
  Write-Host "./intranet.ps1 run web        : build and start dev image of web in docker"
  Write-Host "./intranet.ps1 build web      : build production image from binaries compiled in dev image"
  Write-Host "./intranet.ps1 bash web       : open interactive shell in already running dev image"
  Write-Host ""
  # Utilities
  Write-Host "Utilities:"
  Write-Host "./intranet.ps1 kill           : stops all containers and removes them. (docker stop and docker rm)"
  Write-Host "./intranet.ps1 help           : prints this message"
  Write-Host ""
  # Return to shell
  Return
}

If ($command -eq "kill") {
  docker stop $(docker ps -a -q)
  docker rm $(docker ps -a -q)
  # Return to shell
  Return
}

# Docker-Compose - Entire Intranet
If ($command -eq "debug" -and $target -eq "all") {
  docker-compose build
  docker-compose up
} Elseif ($command -eq "run" -and $target -eq "all") {
  docker-compose -f docker-compose.prod.yml up
} Elseif ($command -eq "stop" -and $target -eq "all") {
  docker-compose down
  # Return to shell
  Return
}

# Docker - API
If ($command -eq "run" -and $target -eq "api") {
  docker build -t apidev -f Dockerfile-api .
  docker run -v ${pwd}:/app -p 3000:80 apidev run
} Elseif ($command -eq "build" -and $target -eq "api") {
  docker build -t apidev -f Dockerfile-api .
  docker run -v ${pwd}:/app -p 3000:80 apidev
  docker build -t api -f Dockerfile-api-prod .
  # TODO: tag and push
} Elseif ($command -eq "bash" -and $target -eq "api") {
  docker exec -it apidev /bin/bash
}

# Docker - Web
If ($command -eq "run" -and $target -eq "web") {
  docker build -t webdev -f Dockerfile-web .
  docker run -v ${pwd}:/app -p 5000:80 webdev run
} Elseif ($command -eq "build" -and $target -eq "web") {
  docker build -t webdev -f Dockerfile-web .
  docker run -v ${pwd}:/app -p 5000:80 webdev
  docker build -t web -f Dockerfile-api-prod .
  # TODO: tag and push
} Elseif ($command -eq "bash" -and $target -eq "web") {
  docker exec -it webdev /bin/bash
}

# Docker - nginx
If ($command -eq "build" -and $target -eq "nginx") {
  docker build -t nginx -f nginx/Dockerfile .
}

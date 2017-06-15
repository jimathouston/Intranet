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
  # TODO: when tests from compose implemented, don't forget to update help
  Write-Host "Full application in docker-compose:"
  Write-Host "./intranet.ps1 test all       : build and test intranet in docker-compose (NOT IMPLEMENTED YET)"
  Write-Host "./intranet.ps1 run all        : build and start intranet in docker-compose"
  Write-Host "./intranet.ps1 stop all       : stops intranet and removes containers. (docker-compose down)"
  Write-Host ""
  # Docker
  Write-Host "API and Web separately in docker:"
  Write-Host "./intranet.ps1 run api        : build and start api in docker"
  Write-Host "./intranet.ps1 test api       : build and test api in docker"
  Write-Host "./intranet.ps1 bash api       : build api and open interactive shell in docker"
  Write-Host "./intranet.ps1 run web        : build and start web in docker"
  Write-Host "./intranet.ps1 test web       : build and test web in docker"
  Write-Host "./intranet.ps1 bash web       : build web and open interactive shell in docker"
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
If ($command -eq "run" -and $target -eq "all") {
  docker-compose build
  docker-compose up
} Elseif ($command -eq "test" -and $target -eq "all") {
  # TODO: when test via docker-compose is implemented, add command here
  Write-Host "Not implemented yet"
} Elseif ($command -eq "stop" -and $target -eq "all") {
  docker-compose down
  # Return to shell
  Return
}

# Docker - API
If ($command -eq "run" -and $target -eq "api") {
  docker build -t api -f Dockerfile-api .
  docker run -v ${pwd}:/app -p 3000:80 api run
} Elseif ($command -eq "test" -and $target -eq "api") {
  docker build -t api -f Dockerfile-api .
  docker run -v ${pwd}:/app -p 3000:80 api test
} Elseif ($command -eq "bash" -and $target -eq "api") {
  docker build -t api -f Dockerfile-api .
  docker run -v ${pwd}:/app -p 3000:80 -it api /bin/bash
}

# Docker - Web
If ($command -eq "run" -and $target -eq "web") {
  docker build -t web -f Dockerfile-web .
  docker run -v ${pwd}:/app -p 5000:80 web run
} Elseif ($command -eq "test" -and $target -eq "web") {
  docker build -t web -f Dockerfile-web .
  docker run -v ${pwd}:/app -p 5000:80 web test
} Elseif ($command -eq "bash" -and $target -eq "web") {
  docker build -t web -f Dockerfile-web .
  docker run -v ${pwd}:/app -p 5000:80 -it web /bin/bash
}

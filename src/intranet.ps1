# args default is empty string
param([string]$command = "")

clear

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
  Write-Host "./intranet.ps1         : Builds intermediate container which runs Publish task in Cake."
  Write-Host "                         Production container is built from published artifacts and starts the Intranet."
  Write-Host "./intranet.ps1 build   : Builds intermediate container which runs Publish task in Cake. "
  Write-Host "                         Production container is built from published artifacts."
  Write-Host "./intranet.ps1 run     : Starts the production container."
  Write-Host "./intranet.ps1 kill    : Stops all containers and removes them. (docker stop and docker rm)"
  Write-Host "./intranet.ps1 help    : Prints this message."
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

If ($command -eq "build") {
  docker build -t buildweb -f Dockerfile-build .
  docker run -v ${pwd}:/app -p 80:80 buildweb
  docker build -t web -f Dockerfile .
} Elseif ($command -eq "run") {
  docker run -p 80:80 web
} Else {
  docker build -t buildweb -f Dockerfile-build .
  docker run -v ${pwd}:/app -p 80:80 buildweb
  docker build -t web -f Dockerfile .
  docker run -p 80:80 web
}

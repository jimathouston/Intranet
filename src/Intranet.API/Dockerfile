FROM microsoft/aspnetcore-build
COPY . /app
WORKDIR /app/Intranet.API.Domain
RUN ["dotnet", "restore"]
WORKDIR /app/Intranet.API
RUN ["dotnet", "restore"]
EXPOSE 3000
#ENTRYPOINT ["dotnet", "run"]

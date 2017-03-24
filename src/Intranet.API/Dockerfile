FROM microsoft/aspnetcore-build
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
EXPOSE 3000
ENTRYPOINT ["dotnet", "run"]

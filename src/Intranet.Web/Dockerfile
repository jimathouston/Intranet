FROM microsoft/aspnetcore-build
COPY . /app
WORKDIR /app/Intranet.Web
RUN ["dotnet", "restore"]
RUN ["npm", "install"]
# FIX for: https://github.com/aspnet/JavaScriptServices/issues/363
RUN ["npm", "install", "-g", "webpack"]
RUN ["webpack", "--config", "webpack.config.vendor.js"]
RUN ["webpack"]
EXPOSE 5000
ENTRYPOINT ["dotnet", "run"]

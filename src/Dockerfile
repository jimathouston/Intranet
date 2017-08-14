FROM microsoft/aspnetcore-build
WORKDIR /app
EXPOSE 80
COPY init-web.sh /app
RUN chmod +x ./init-web.sh
COPY ./Intranet.Web/bin/Debug/netcoreapp1.1/publish /app
ENTRYPOINT ["./init-web.sh"]

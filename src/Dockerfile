FROM microsoft/aspnetcore-build
WORKDIR /app
EXPOSE 80
COPY init-web.sh /app
RUN chmod +x ./init-web.sh
COPY Artifacts/Release/ /app/
ENTRYPOINT ["./init-web.sh"]

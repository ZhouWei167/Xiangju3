FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY publish/ .
ENV ASPNETCORE_URLS=http://*:5000
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "在线授权系统.dll"]
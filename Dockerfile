FROM microsoft/dotnet:1.1.0-sdk-projectjson

# Set the Working Directory
WORKDIR /app

# Configure the listening port to 80
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80

# Copy the app
COPY bin/Debug/netcoreapp1.1/publish /app
COPY appsettings.json /app/

# Start the app
CMD dotnet dotnet_service.dll

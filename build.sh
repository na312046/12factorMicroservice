#!/bin/sh
CONFIGURATION=Release
DOCKER_TAG=kmbulebu/dotnet_microservice
dotnet restore
dotnet build -c $CONFIGURATION
dotnet publish -c $CONFIGURATION --no-build
dotnet pack -c $CONFIGURATION --no-build
docker build --rm --tag=$DOCKER_TAG .

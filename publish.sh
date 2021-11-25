#!/usr/bin/env bash
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x

VERSION="1.40"

mkdir ./release
for RUNTIME in osx-x64; do
    # Server
    dotnet publish Arrowgene.Ez2Off.CLI/Arrowgene.Ez2Off.CLI.csproj /p:Version=$VERSION /p:FromMSBuild=true --runtime $RUNTIME --configuration Release --output ../publish/$RUNTIME-$VERSION/Server
    # Starter
    cp -r ./Starter/. ./publish/$RUNTIME-$VERSION/
done 
 
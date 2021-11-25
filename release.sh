#!/usr/bin/env bash
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x

VERSION="3.00"

mkdir ./release
for RUNTIME in win-x86 win-x64 linux-x64 osx-x64; do
    # Server
    dotnet publish Arrowgene.Ez2Off.CLI/Arrowgene.Ez2Off.CLI.csproj /p:Version=$VERSION /p:FromMSBuild=true --runtime $RUNTIME --configuration Release --output ../publish/$RUNTIME-$VERSION/Server
    # Starter
    cp -r ./Starter/. ./publish/$RUNTIME-$VERSION/
    # Pack
    tar cjf ./release/$RUNTIME-$VERSION.tar.gz ./publish/$RUNTIME-$VERSION
done 
 
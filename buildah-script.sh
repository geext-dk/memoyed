#!/bin/bash
# exit if one of the commands fail
set -e

CONFIGURATION=Release

build_con=$(buildah from --pull mcr.microsoft.com/dotnet/core/sdk:3.1)
buildah run $build_con mkdir -p /app
buildah config --workingdir /app $build_con

echo "COPYING .csproj FILES"
buildah copy $build_con src/DomainFramework/DomainFramework.csproj \
    /app/DomainFramework/DomainFramework.csproj
buildah copy $build_con src/Domain.Cards/Domain.Cards.csproj \
    /app/Domain.Cards/Domain.Cards.csproj
buildah copy $build_con src/Domain.Users/Domain.Users.csproj \
    /app/Domain.Users/Domain.Users.csproj
buildah copy $build_con src/Application/Application.csproj \
    /app/Application/Application.csproj
buildah copy $build_con src/WebApi/WebApi.csproj \
    /app/WebApi/WebApi.csproj

echo "RESTORING"
buildah run $build_con dotnet restore /app/WebApi/WebApi.csproj

echo "COPYING SOURCE CODE"
buildah copy $build_con src/DomainFramework/ /app/DomainFramework
buildah copy $build_con src/Domain.Cards/ /app/Domain.Cards
buildah copy $build_con src/Domain.Users/ /app/Domain.Users
buildah copy $build_con src/Application/ /app/Application
buildah copy $build_con src/WebApi/ /app/WebApi
buildah config --workingdir /app/WebApi $build_con

echo "PUBLISHING"
buildah run $build_con dotnet publish -c $CONFIGURATION -o out

build_mount=$(buildah mount $build_con)

run_con=$(buildah from --pull mcr.microsoft.com/dotnet/core/aspnet:3.1)

echo "CHANGING WORKING DIRECTORY"
buildah config --workingdir /app $run_con

echo "MOUNTING RUN CONTAINER"
run_mount=$(buildah mount $run_con)

echo "COPYING PUBLISHED APP"
cp -r $build_mount/app/WebApi/out $run_mount/app

buildah umount $build_con
buildah umount $run_con

buildah config --entrypoint '["dotnet", "Memoyed.WebApi.dll"]' \
    --port 80 --port 443 $run_con

echo "COMMITTING"
buildah commit $run_con registry.gitlab.com/geext/memoyed

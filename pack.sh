#!/usr/bin/env bash
set -e
dotnet pack ./NotifyPropertyChanged.Verifier/NotifyPropertyChanged.Verifier.csproj --configuration Release --no-restore --no-build --output nupkgs
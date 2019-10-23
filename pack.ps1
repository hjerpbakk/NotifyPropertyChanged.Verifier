Remove-Item ./CoverageResults -Force -Recurse -ErrorAction SilentlyContinue
dotnet pack ./NotifyPropertyChanged.Verifier/NotifyPropertyChanged.Verifier.csproj --configuration Release --no-restore --no-build --output nupkgs

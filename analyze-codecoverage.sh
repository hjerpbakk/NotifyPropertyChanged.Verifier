#!/usr/bin/env bash
set -e
./build.sh
./test.sh
dotnet ~/.nuget/packages/reportgenerator/4.3.0/tools/netcoreapp2.1/ReportGenerator.dll -reports:./CoverageResults/coverage.opencover.xml -targetdir:./CoverageResults
open ./CoverageResults/index.htm
#!/usr/bin/env pwsh
#echo "Install dotnet-sonarscanner ----------------------------------------------------------------------------------------------------------------------"
#dotnet tool install --global dotnet-sonarscanner 

Write-Host "Start Sonarscanner -------------------------------------------------------------------------------------------------------------------------------"

$org = "codedesignplus"
$key = "CodeDesignPlus.Net.Microservice.Services"
$csproj = "CodeDesignPlus.Net.Microservice.Services.sln"
$report = "tests/**/coverage.opencover.xml"
$server = "http://localhost:9000"
$token = "sqa_7092fd9e276f95ccb8fedf689f538ac4aa43dc3c"

Set-Location ..\..\
dotnet test $csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet sonarscanner begin /o:$org /k:$key /d:sonar.host.url=$server /d:sonar.coverage.exclusions="**Tests*.cs,**/tests/load/*.js" /d:sonar.cs.opencover.reportsPaths=$report /d:sonar.login=$token
dotnet build
dotnet sonarscanner end /d:sonar.login=$token
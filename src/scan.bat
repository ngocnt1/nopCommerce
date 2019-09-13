dotnet-sonarscanner begin /k:"Test" /d:sonar.host.url="http://192.168.162.6:9000" /d:sonar.login="fef996f96cda538a4c0bef2e30eebee71c95f7b5"
dotnet build Spacedadi.Portal.sln /t:Rebuild
dotnet-sonarscanner end /d:sonar.login="fef996f96cda538a4c0bef2e30eebee71c95f7b5"
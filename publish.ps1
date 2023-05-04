# get git tag
$git_tag = $(git describe --tags --abbrev=0)


# dotnet pack version and specify project
$mycmd = "dotnet pack -c Release -p:PackageVersion=$git_tag -p:Project=/YamlFormatter/YamlFormatter.csproj"
$mycmd
Invoke-Expression $mycmd


# Upload to nuget
$mycmd = "dotnet nuget push ./YamlFormatter/bin/Release/Katasec.AspNet.YamlFormatter.$git_tag.nupkg -k $env:NUGET_API_KEY -s https://api.nuget.org/v3/index.json"
$mycmd
Invoke-Expression $mycmd

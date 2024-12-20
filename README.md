# miniapi - an asp.net web api generator

`Swagger` `NLog` `JWT`

### Requirements

* .NETFramework,Version>=v4.5

### Usage
```powershell
miniapi TestApi
```

#### Output
```powershell
Release file: D:\Source\Test\TestApi\Utils\Microsoft.IdentityModel.Tokens\ITokenProvider.cs
Release file: D:\Source\Test\TestApi\Utils\Microsoft.IdentityModel.Tokens\TokenInfo.cs
Release file: D:\Source\Test\TestApi\Utils\System.IdentityModel.Tokens.Jwt\JwtProvider.cs
Release file: D:\Source\Test\TestApi\Utils\System.Web.Http\AuthAttribute.cs
Release file: D:\Source\Test\TestApi\Utils\System.Web.Http\Extensions.cs
Release file: D:\Source\Test\TestApi\App_Start\Startup.cs
Release file: D:\Source\Test\TestApi\Controllers\TestController.cs
Release file: D:\Source\Test\TestApi\Program.cs
Release file: D:\Source\Test\TestApi\Properties\AssemblyInfo.cs
Release file: D:\Source\Test\TestApi\Utils\System.Net.Http\HttpExtensions.cs
Release file: D:\Source\Test\TestApi\Utils\System.Net.Http\HttpResult.cs
Release file: D:\Source\Test\TestApi\Utils\System.Web.Http\LoggerConfig.cs
Release file: D:\Source\Test\TestApi\WebAppService.cs
Release file: D:\Source\Test\TestApi\WebAppService.Designer.cs
Release file: D:\Source\Test\TestApi\App.config
Release file: D:\Source\Test\TestApi\NLog.config
Release file: D:\Source\Test\TestApi\packages.config
Release file: D:\Source\Test\TestApi\TestApi.csproj
Release file: D:\Source\Test\TestApi\Properties\app.manifest
Done
```

#### Example
```powershell
dotnet new sln -n Test
miniapi TestApi
dotnet sln add TestApi\TestApi.csproj
nuget restore
msbuild
```

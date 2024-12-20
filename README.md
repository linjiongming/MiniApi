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
Release file: X:\path\to\your\solution\TestApi\Utils\Microsoft.IdentityModel.Tokens\ITokenProvider.cs
Release file: X:\path\to\your\solution\TestApi\Utils\Microsoft.IdentityModel.Tokens\TokenInfo.cs
Release file: X:\path\to\your\solution\TestApi\Utils\System.IdentityModel.Tokens.Jwt\JwtProvider.cs
Release file: X:\path\to\your\solution\TestApi\Utils\System.Web.Http\AuthAttribute.cs
Release file: X:\path\to\your\solution\TestApi\Utils\System.Web.Http\Extensions.cs
Release file: X:\path\to\your\solution\TestApi\App_Start\Startup.cs
Release file: X:\path\to\your\solution\TestApi\Controllers\TestController.cs
Release file: X:\path\to\your\solution\TestApi\Program.cs
Release file: X:\path\to\your\solution\TestApi\Properties\AssemblyInfo.cs
Release file: X:\path\to\your\solution\TestApi\Utils\System.Net.Http\HttpExtensions.cs
Release file: X:\path\to\your\solution\TestApi\Utils\System.Net.Http\HttpResult.cs
Release file: X:\path\to\your\solution\TestApi\Utils\System.Web.Http\LoggerConfig.cs
Release file: X:\path\to\your\solution\TestApi\WebAppService.cs
Release file: X:\path\to\your\solution\TestApi\WebAppService.Designer.cs
Release file: X:\path\to\your\solution\TestApi\App.config
Release file: X:\path\to\your\solution\TestApi\NLog.config
Release file: X:\path\to\your\solution\TestApi\packages.config
Release file: X:\path\to\your\solution\TestApi\TestApi.csproj
Release file: X:\path\to\your\solution\TestApi\Properties\app.manifest
Done
```

#### Example
```powershell
dotnet new sln -n Test
```
```powershell
miniapi TestApi
```
```powershell
dotnet sln add TestApi\TestApi.csproj
```
```powershell
nuget restore
```
```powershell
msbuild
```

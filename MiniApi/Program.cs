using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

namespace MiniApi
{
    class Program
    {
        static readonly Random _random = new Random();
        static readonly List<int> _usedPorts = new List<int>();
        static readonly Dictionary<string, string> _specialPaths = new Dictionary<string, string>
        {
            ["Utils.Microsoft.IdentityModel.Tokens.ITokenProvider.cs"] = @"Utils\Microsoft.IdentityModel.Tokens\ITokenProvider.cs",
            ["Utils.Microsoft.IdentityModel.Tokens.TokenInfo.cs"] = @"Utils\Microsoft.IdentityModel.Tokens\TokenInfo.cs",
            ["Utils.System.IdentityModel.Tokens.Jwt.JwtProvider.cs"] = @"Utils\System.IdentityModel.Tokens.Jwt\JwtProvider.cs",
            ["Utils.System.Net.Http.HttpExtensions.cs"] = @"Utils\System.Net.Http\HttpExtensions.cs",
            ["Utils.System.Net.Http.HttpResult.cs"] = @"Utils\System.Net.Http\HttpResult.cs",
            ["Utils.System.Web.Http.AuthAttribute.cs"] = @"Utils\System.Web.Http\AuthAttribute.cs",
            ["Utils.System.Web.Http.Extensions.cs"] = @"Utils\System.Web.Http\Extensions.cs",
            ["Utils.System.Web.Http.LoggerConfig.cs"] = @"Utils\System.Web.Http\LoggerConfig.cs",
            ["WebAppService.Designer.cs"] = "WebAppService.Designer.cs",
        };

        static int Main(string[] args)
        {
            try
            {
                string projectName = GetProjectName(args);
                Guid projectGuid = Guid.NewGuid();
                int freePort = NextFreePort(8000, 9000);
                string jwtKey = RandomBase64(32);
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyName = assembly.GetName().Name;
                string resourcePrefix = $"{assemblyName}.src.";
                string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), projectName);
                foreach (string resourceName in assembly.GetManifestResourceNames())
                {
                    if (!resourceName.StartsWith(resourcePrefix)) continue;
                    string srcName = resourceName.Substring(resourcePrefix.Length);
                    string[] srcNameParts = srcName.Split('.');
                    string folder, filename;
                    if (_specialPaths.ContainsKey(srcName))
                    {
                        folder = Path.GetDirectoryName(_specialPaths[srcName]);
                        filename = Path.GetFileName(_specialPaths[srcName]);
                    }
                    else if (srcNameParts.Length > 2)
                    {
                        int folderDeep = srcNameParts.Length - 2;
                        folder = string.Join("\\", srcNameParts.Take(folderDeep));
                        filename = string.Join(".", srcNameParts.Skip(folderDeep));
                    }
                    else
                    {
                        folder = string.Empty;
                        filename = srcName;
                    }
                    filename = filename.Replace(assemblyName, projectName);
                    string path = Path.Combine(baseDirectory, folder, filename);
                    using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(resourceStream, Encoding.UTF8))
                    {
                        string content = reader.ReadToEnd();
                        content = content.Replace(assemblyName, projectName);
                        if (filename == projectName + ".csproj")
                        {
                            content = content.Replace("{ProjectGuid}", projectGuid.ToString().ToUpper());
                        }
                        else if (filename == "App.config")
                        {
                            content = content.Replace("{HostingPort}", freePort.ToString());
                            content = content.Replace("{JwtKey}", jwtKey);
                        }
                        else if (filename == "AssemblyInfo.cs")
                        {
                            content = content.Replace("{ProjectGuid}", projectGuid.ToString().ToLower());
                        }
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        File.WriteAllText(path, content);
                        Console.WriteLine("Release file: " + path);
                    }
                }
                Console.WriteLine("Done");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        static string GetProjectName(string[] args)
        {
            string projectName;
            if (args.Length < 1)
            {
                Console.WriteLine("Please input project name:");
                projectName = Console.ReadLine();
            }
            else
            {
                projectName = args[0];
            }
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new Exception("Project name cannot be empty");
            }
            projectName = projectName.Replace(" ", "_");
            return projectName;
        }

        static int NextFreePort(int min, int max)
        {
            if (_usedPorts.Count == 0)
            {
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                _usedPorts.AddRange(ipProperties
                    .GetActiveTcpConnections()
                    .Where(connection => connection.State != TcpState.Closed)
                    .Select(connection => connection.LocalEndPoint)
                    .Concat(ipProperties.GetActiveTcpListeners())
                    .Concat(ipProperties.GetActiveUdpListeners())
                    .Select(endpoint => endpoint.Port));
            }
            int port;
            do
            {
                port = _random.Next(8000, 9000);
            }
            while (_usedPorts.Contains(port));
            _usedPorts.Add(port);
            return port;
        }

        static string RandomBase64(int length)
        {
            byte[] bytes = new byte[length];
            _random.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}

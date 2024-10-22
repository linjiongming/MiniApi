using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MiniApi
{
    class Program
    {
        static readonly Dictionary<string, string> _specialPaths = new Dictionary<string, string>
        {
            ["WebAppService.Designer.cs"] = "WebAppService.Designer.cs",
            ["Utils.System.Web.Http.LogTraceConfig.cs"] = @"Utils\System.Web.Http\LogTraceConfig.cs",
            ["Utils.System.Net.Http.HttpResult.cs"] = @"Utils\System.Net.Http\HttpResult.cs",
            ["Utils.System.Net.Http.HttpExtensions.cs"] = @"Utils\System.Net.Http\HttpExtensions.cs",
        };

        static int Main(string[] args)
        {
            try
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

                Guid projectGuid = Guid.NewGuid();

                int freePort = FreePorts.Next(8000, 9000);

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
    }
}

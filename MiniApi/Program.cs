using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MiniApi
{
    class Program
    {
        static readonly string[] _multiExtFiles = new string[] { "Global.asax.cs", "MiniApi.csproj.user" };

        static void Main(string[] args)
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
            Guid projectGuid = Guid.NewGuid();
            int devPort = FreeTcpPort();
            int iisPort = FreeTcpPort();
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), projectName);
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyName = assembly.GetName().Name;
            string resourcePrefix = $"{assemblyName}.src.";
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (!resourceName.StartsWith(resourcePrefix)) continue;
                string srcName = resourceName.Substring(resourcePrefix.Length);
                string[] array = srcName.Split('.');
                string folder, filename;
                if (_multiExtFiles.Contains(srcName))
                {
                    folder = string.Empty;
                    filename = srcName;
                }
                else if (array.Length > 2)
                {
                    int folderDeep = array.Length - 2;
                    folder = string.Join("\\", array.Take(folderDeep));
                    filename = string.Join(".", array.Skip(folderDeep));
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
                        content = content.Replace("{DevPort}", devPort.ToString());
                        content = content.Replace("{IISPort}", iisPort.ToString());
                    }
                    else if (filename == "AssemblyInfo.cs")
                    {
                        content = content.Replace("{ProjectGuid}", projectGuid.ToString().ToLower());
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.WriteAllText(path, content);
                    Console.WriteLine("File created: " + path);
                }
            }
            Console.WriteLine("Done");
        }

        static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}

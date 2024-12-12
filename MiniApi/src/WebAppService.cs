using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;

namespace MiniApi
{
    public partial class WebAppService : ServiceBase
    {
        private IDisposable _app;

        public Uri Uri { get; }
        public string HostingUrl => Uri.Host.Equals("0.0.0.0") ? Uri.ToString().Replace("0.0.0.0", "+") : Uri.ToString();
        public string LocalhostUrl => new UriBuilder(Uri.Scheme, "localhost", Uri.Port, Uri.PathAndQuery).Uri.ToString();
        public string CertHash { get; }

        public WebAppService()
        {
            var url = ConfigurationManager.AppSettings.Get("Hosting:Url");
            Uri = new Uri(url.Replace("+", "0.0.0.0").Replace("*", "0.0.0.0"));
            CertHash = ConfigurationManager.AppSettings.Get("Hosting:CertHash");
            InitializeComponent();
        }

        public void Start(string[] args) { OnStart(args); }
        protected override void OnStart(string[] args)
        {
            if (Uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(CertHash))
                {
                    throw new ArgumentNullException("Hosting:CertHash");
                }
                RegisterSslOnPortIfNotRegistered(Uri.Port, CertHash);
            }
            _app = WebApp.Start<Startup>(HostingUrl);
        }

        protected override void OnStop()
        {
            if (_app != null)
            {
                _app.Dispose();
                _app = null;
            }
        }

        public static void RegisterSslOnPortIfNotRegistered(int port, string certThumbprint)
        {
            // 检查证书是否已经绑定到端口
            string checkCommand = $"http show sslcert ipport=0.0.0.0:{port}";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", checkCommand)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(procStartInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // 如果输出中不包含证书的哈希值，则注册证书
                if (!output.Contains(certThumbprint))
                {
                    var appId = Guid.NewGuid();
                    string arguments = $"http add sslcert ipport=0.0.0.0:{port} certhash={certThumbprint} appid={{{appId}}}";
                    ProcessStartInfo addProcStartInfo = new ProcessStartInfo("netsh", arguments)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var addProcess = Process.Start(addProcStartInfo))
                    {
                        while (!addProcess.StandardOutput.EndOfStream)
                        {
                            string line = addProcess.StandardOutput.ReadLine();
                            Console.WriteLine(line);
                        }

                        addProcess.WaitForExit();
                    }
                }
            }
        }
    }
}

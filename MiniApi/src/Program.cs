using NLog;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace MiniApi
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new NLogTraceListener());
            using (WebAppService webAppService = new WebAppService())
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("Debug Mode. Explan: input g to collect garbage, input q to quit.");
                    Console.WriteLine("================================================================");
                    webAppService.Start(args);
                    Process.Start(webAppService.LocalhostUrl.TrimEnd('/') + "/swagger").Dispose();
                    int read;
                    while ((read = Console.Read()) != 'q')
                    {
                        if (read == 'g') GC.Collect();
                    }
                    webAppService.Stop();
                    Console.ReadLine();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                        webAppService
                    };
                    ServiceBase.Run(ServicesToRun);
                }
            }
            LogManager.Shutdown();
        }
    }
}

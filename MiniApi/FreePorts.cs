using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

public class FreePorts
{
    private static Random random = new Random();
    private static IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

    public static List<int> UsedPorts = ipProperties
        .GetActiveTcpConnections()
        .Where(connection => connection.State != TcpState.Closed)
        .Select(connection => connection.LocalEndPoint)
        .Concat(ipProperties.GetActiveTcpListeners())
        .Concat(ipProperties.GetActiveUdpListeners())
        .Select(endpoint => endpoint.Port)
        .ToList();

    public static int Next(int min, int max)
    {
        int port;
        do
        {
            port = random.Next(8000, 9000);
        }
        while (UsedPorts.Contains(port));
        UsedPorts.Add(port);
        return port;
    }
}


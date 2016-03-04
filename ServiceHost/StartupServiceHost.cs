using System;
using System.ServiceModel;
using Delver.Interface;
using Delver.Service;

namespace Delver.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var duplex = new ServiceHost(typeof (ServerWCallbackImpl));
            duplex.AddServiceEndpoint(typeof (IServerWithCallback), new NetTcpBinding(),
                "net.tcp://localhost:9080/DataService");
            //duplex.AddServiceEndpoint(typeof(IServerWithCallback), new NetHttpBinding(), "http://localhost:9080/DataService");
            duplex.Open();
            Console.WriteLine("Listening according to configuration");
            Console.ReadLine();
            Console.WriteLine("Exiting...");
            Console.ReadLine();
        }
    }
}
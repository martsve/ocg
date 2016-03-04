using System;
using System.ServiceModel;
using Delver.Interface;

namespace TestClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cf =
                new DuplexChannelFactory<IServerWithCallback>(
                    new CallbackImpl(),
                    new NetTcpBinding(),
                    new EndpointAddress("net.tcp://localhost:9080/DataService"));
            var srv = cf.CreateChannel();

            Console.WriteLine("Console App running...");
            Console.WriteLine("Trying to start new server...");
            srv.StartNewServer();

            /*
            for (int i = 0; i < 1000; i++)
                srv.StartNewServer();
            */

            Console.WriteLine("Send command: <game> <player> <command>");
            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd.ToLower() == "exit") break;

                var w = cmd.Split(' ');

                try
                {
                    /*
                    int game = int.Parse(w[0]);
                    int player = int.Parse(w[1]);
                    string command = string.Join(" ", w.Skip(2));
                    srv.SendCommand(game, player, command);
                    */

                    srv.SendCommand(1, 0, cmd);
                }
                catch
                {
                }
            }

            Console.WriteLine("Exiting!");
            Console.ReadLine();
        }

        private class CallbackImpl : IGameCallback
        {
            public void SendDataPacket(string data)
            {
                Console.WriteLine("Reply: " + data);
            }
        }
    }
}
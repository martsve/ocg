using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Delver.Interface;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication1
{
    public static class Backchannel
    {
        private static IServerWithCallback _srv;

        private static Dictionary<string, int> users { get; set; } = new Dictionary<string, int>();

        public static void Setup()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            var cf =
                new DuplexChannelFactory<IServerWithCallback>(
                    new CallbackImpl(context),
                    new NetTcpBinding(),
                    new EndpointAddress("net.tcp://localhost:9080/DataService"));

            _srv = cf.CreateChannel();

            ;
        }

        public static void Map(string id, string playerId)
        {
            int playerNum;
            if (int.TryParse(playerId, out playerNum))
            {
                if (users.ContainsKey(id))
                {
                    users[id] = playerNum;
                 }
                else
                {
                    users.Add(id, playerNum);
                }
            }
        }

        public static void Send(string playerId, string cmd)
        {
            var userid = -1;

            if (users.ContainsKey(playerId))
            {
                userid = users[playerId];
            }

            _srv.SendCommand(1, userid, cmd);
        }

        public static void StartNew()
        {
            _srv.StartNewServer();
        }

        private class CallbackImpl : IGameCallback
        {
            private IHubContext Context { get; set; }

            public CallbackImpl(IHubContext context)
            {
                Context = context;
            }

            public void SendDataPacket(int player, string data)
            {
                if (player == -1)
                    Context.Clients.All.broadcastMessage("TO_ALL", data);

                else {
                    foreach (var connectionId in Backchannel.users.Where(x => x.Value == player).Select(x => x.Key))
                    {
                        Context.Clients.Client(connectionId).broadcastMessage("TO_YOU", data);
                    }
                }
                
            }
        }
    }
}
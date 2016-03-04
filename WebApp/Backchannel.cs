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

        private static int msgNum = 0;

        public static void Setup()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            var cf =
                new DuplexChannelFactory<IServerWithCallback>(
                    new CallbackImpl(context),
                    new NetTcpBinding(),
                    new EndpointAddress("net.tcp://localhost:9080/DataService"));

            _srv = cf.CreateChannel();
        }

        public static void Send(string cmd)
        {
            if (msgNum == 0)
                _srv.StartNewServer();

            else
                _srv.SendCommand(1, 0, cmd);

            msgNum++;
        }

        private class CallbackImpl : IGameCallback
        {
            private IHubContext Context { get; set; }

            public CallbackImpl(IHubContext context)
            {
                Context = context;
            }

            public void SendDataPacket(string data)
            {
                Context.Clients.All.broadcastMessage("SERVER", data);
            }
        }
    }
}
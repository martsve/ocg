using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Delver.Interface;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public static class Backchannel
    {
        private static IServerWithCallback _srv;

        public static void Setup()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            var cf =
                new DuplexChannelFactory<IServerWithCallback>(
                    new CallbackImpl(context),
                    new NetTcpBinding(),
                    new EndpointAddress("net.tcp://localhost:9080/DataService"));

            _srv = cf.CreateChannel();

            _srv.StartNewServer();
        }

        public static void Send(string cmd)
        {
            _srv.SendCommand(1, 0, cmd);
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

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Backchannel.Setup();
            app.MapSignalR();
            ConfigureAuth(app);
        }


    }
}
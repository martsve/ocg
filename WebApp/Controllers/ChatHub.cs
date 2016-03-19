using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.ServiceModel;
using Delver.Interface;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication1
{
    // http://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-server
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            var id = Context.ConnectionId;

            // Call the broadcastMessage method to update clients.
            //Clients.All.broadcastMessage(name, message);
            Backchannel.Send(id, message);
        }
        public void startNew()
        {
            Backchannel.StartNew();
        }
        public void setPlayer(string playerId)
        {
            // Call the broadcastMessage method to update clients.
            //Clients.All.broadcastMessage(name, message);

            var caller = Clients.Caller;
            var id = Context.ConnectionId;
            Backchannel.Map(id, playerId);

            Groups.Add(Context.ConnectionId, Context.ConnectionId);
        }
    }

}
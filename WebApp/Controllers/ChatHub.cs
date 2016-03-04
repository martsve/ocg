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
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            //Clients.All.broadcastMessage(name, message);
            Backchannel.Send(message);
        }
    }

}
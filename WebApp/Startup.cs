using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ;
            // Set up WCF to ServiceHost
            Backchannel.Setup();

            // Set up SignalR 
            app.MapSignalR();

            // Configure some WebApp stuff..?
            ConfigureAuth(app);
        }
    }
}
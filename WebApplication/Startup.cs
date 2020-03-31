﻿using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(WebApplication.Startup))]
namespace WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }

        public void Configure(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}

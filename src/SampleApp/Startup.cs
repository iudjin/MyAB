﻿using Microsoft.Owin;
using Owin;
using SampleApp;

[assembly: OwinStartup(typeof(Startup))]
namespace SampleApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

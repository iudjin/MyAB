using Microsoft.Owin;
using MyAB.SampleApp;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace MyAB.SampleApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

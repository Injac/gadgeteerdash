using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iotdash.Startup))]
namespace iotdash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}

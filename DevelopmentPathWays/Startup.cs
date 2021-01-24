using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevelopmentPathWays.Startup))]
namespace DevelopmentPathWays
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

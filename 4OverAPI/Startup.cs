using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_4OverAPI.Startup))]
namespace _4OverAPI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

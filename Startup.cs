using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FacebookReadMVC.Startup))]
namespace FacebookReadMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

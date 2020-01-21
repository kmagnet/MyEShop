using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyEShop.WebUI.Startup))]
namespace MyEShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

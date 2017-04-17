using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bh.Web.Startup))]
namespace Bh.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

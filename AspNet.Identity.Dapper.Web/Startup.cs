using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNet.Identity.Dapper.Web.Startup))]
namespace AspNet.Identity.Dapper.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

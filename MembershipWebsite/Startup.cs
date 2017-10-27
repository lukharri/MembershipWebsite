using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MembershipWebsite.Startup))]
namespace MembershipWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

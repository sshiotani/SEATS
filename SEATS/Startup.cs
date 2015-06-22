using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SEATS.Startup))]
namespace SEATS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CcaRegistrationDf.Startup))]
namespace CcaRegistrationDf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

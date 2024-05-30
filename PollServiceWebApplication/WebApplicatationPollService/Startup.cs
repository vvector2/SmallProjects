using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplicatationPollService.Startup))]
namespace WebApplicatationPollService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IT_Airlines.Startup))]
namespace IT_Airlines
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

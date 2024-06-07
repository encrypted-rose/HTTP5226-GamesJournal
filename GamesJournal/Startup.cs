using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamesJournal.Startup))]
namespace GamesJournal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

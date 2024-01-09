using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecipeBook.Startup))]
namespace RecipeBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

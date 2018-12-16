using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TransactionQuery.CSharp.Startup))]
namespace TransactionQuery.CSharp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}

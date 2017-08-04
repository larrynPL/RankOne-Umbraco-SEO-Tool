using Umbraco.Core;
using Umbraco.Web;

namespace RankOne.IOC
{
    public class RankOneContext
    {
        public DatabaseContext DatabaseContext;

        public RankOneContext()
        {
            DatabaseContext = UmbracoContext.Current.Application.DatabaseContext;
        }
    }
}

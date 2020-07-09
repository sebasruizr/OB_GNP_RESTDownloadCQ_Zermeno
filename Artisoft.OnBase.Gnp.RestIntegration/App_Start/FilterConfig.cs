using System.Web;
using System.Web.Mvc;

namespace Artisoft.OnBase.Gnp.RestIntegration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

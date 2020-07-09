using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Artisoft.OnBase.Gnp.RestIntegration.Bindings;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using Artisoft.OnBase.Gnp.RestIntegration.Services;
using log4net;

namespace Artisoft.OnBase.Gnp.RestIntegration.Controllers
{
    [RoutePrefix("api/custom-queries")]
    public class CustomQueryController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IOnBaseService _onBaseService;


        public CustomQueryController(IOnBaseService onBaseService)
        {
            _onBaseService = onBaseService;
        }
    }
}
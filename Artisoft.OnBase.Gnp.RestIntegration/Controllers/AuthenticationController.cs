using System;
using System.Web.Http;
using Artisoft.OnBase.Gnp.RestIntegration.Bindings;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using Artisoft.OnBase.Gnp.RestIntegration.Services;
using log4net;

namespace Artisoft.OnBase.Gnp.RestIntegration.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IOnBaseService _onBaseService;


        public AuthenticationController(IOnBaseService onBaseService)
        {
            _onBaseService = onBaseService;
        }

        [HttpPost]
        [Route("login")]
        public AuthenticationResponse Login(AuthenticationRequest request)
        {
            var sessionId = _onBaseService.Login(request.Username, request.Password);
            return new AuthenticationResponse
            {
                AuthToken = sessionId,
                CreationDate = DateTime.Now
            };
        }

        [HttpPost]
        [Route("logout")]
        public void Logout([FromHeader("x-gnp-session-token")] string sessionId)
        {
            Log.Debug($"Logout for sessionId: {sessionId}");
            _onBaseService.Logout(sessionId);
        }
    }
}
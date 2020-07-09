using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Artisoft.OnBase.Gnp.RestIntegration.Exceptions;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using log4net;

namespace Artisoft.OnBase.Gnp.RestIntegration.Filters
{
    public class RestIntegrationExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext context)
        {
            switch (context.Exception)
            {
                case AuthenticationException e:
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message);
                    break;
                case ValidationException e:
                    HttpStatusCode validationStatusCode;
                    string validationMessage;
                    switch (e.ErrorCode)
                    {
                        case ErrorCode.ValidationInvalidDocId:
                            validationStatusCode = HttpStatusCode.NotFound;
                            validationMessage = "Resource not found";
                            break;
                        default:
                            validationStatusCode = HttpStatusCode.BadRequest;
                            validationMessage = e.Message;
                            break;
                    }

                    context.Response = context.Request.CreateErrorResponse(validationStatusCode, validationMessage);
                    break;
                case ServiceException e:
                    HttpStatusCode errorStatusCode;
                    switch (e.ErrorCode)
                    {
                        default:
                            errorStatusCode = HttpStatusCode.InternalServerError;
                            break;
                    }

                    context.Response = context.Request.CreateErrorResponse(errorStatusCode, e.Message);
                    break;
                default:
                    Log.Error("Unexpected error", context.Exception);
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unexpected error");
                    break;
            }
        }
    }
}
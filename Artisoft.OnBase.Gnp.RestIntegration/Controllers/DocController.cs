using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Artisoft.OnBase.Gnp.RestIntegration.Bindings;
using Artisoft.OnBase.Gnp.RestIntegration.Exceptions;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using Artisoft.OnBase.Gnp.RestIntegration.Services;
using log4net;

namespace Artisoft.OnBase.Gnp.RestIntegration.Controllers
{
    [System.Web.Http.RoutePrefix("api/docs")]
    public class DocController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IOnBaseService _onBaseService;

        public DocController(IOnBaseService onBaseService)
        {
            _onBaseService = onBaseService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{id}")]
        public HttpResponseMessage show(long id, [FromHeader("x-gnp-session-token")] string sessionId)
        {
            File file = null;
            try
            {
                Log.Debug($"docId: {id}, sessionId: {sessionId}");
                file = _onBaseService.GetDocumentAsFile(id, sessionId);

                HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new ByteArrayContent(file.Content.ToArray());
                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = file.Name;
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return httpResponseMessage;
            }
            catch (RestIntegrationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                //fixme hacer bienlael manejo de excepciones
                Log.Error($"Error getting file with id: {id}", e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (file != null)
                    DisposeResource(file.Content);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route]
        public HttpResponseMessage index(long queryId, [ModelBinder] List<KeywordFilter> filter,
            [FromHeader("x-gnp-session-token")] string sessionId)
        {
            Log.Debug($"queryId: {queryId}, sessionId: {sessionId}");
            Validate(filter, "filter");
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            filter.ForEach(f => Log.Debug($"Filter: {f.KeywordId} {f.Operator.ToString()} {f.Value}"));
            return Request.CreateResponse(_onBaseService.QueryAsDocumentResponse(sessionId, queryId, filter));
        }

        private void DisposeResource(IDisposable resource)
        {
            try
            {
                if (resource != null)
                    resource.Dispose();
            }
            catch (Exception e)
            {
            }
        }
    }
}
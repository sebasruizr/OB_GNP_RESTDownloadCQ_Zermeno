using System.Collections.Generic;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using Hyland.Unity;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services
{
    public interface IOnBaseService
    {
        string Login(string username, string password);
        QueryResult Query(string sessionId, long queryId, List<KeywordFilter> filters);

        IEnumerable<DocumentResponse> QueryAsDocumentResponse(string sessionId, long queryId, List<KeywordFilter> filters);
        File GetDocumentAsFile(long id, string sessionId);

        void Logout(string sessionId);
    }
}
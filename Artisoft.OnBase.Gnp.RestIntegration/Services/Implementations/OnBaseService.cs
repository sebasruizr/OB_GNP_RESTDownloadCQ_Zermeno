using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Artisoft.OnBase.Gnp.RestIntegration.Exceptions;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using Artisoft.OnBase.Gnp.RestIntegration.Services.Configs.OnBase;
using Hyland.Unity;
using log4net;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using File = Artisoft.OnBase.Gnp.RestIntegration.Models.File;
using ValidationException = Artisoft.OnBase.Gnp.RestIntegration.Exceptions.ValidationException;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services.Implementations
{
    public class OnBaseService : IOnBaseService
    {
        private readonly OnBaseConfig _config;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OnBaseService()
        {
            _config = (OnBaseConfig) ConfigurationManager.GetSection("OnBaseConfig");
        }

        public string Login(string username, string password)
        {
            Application app = null;
            try
            {
                var authenticationProperties =
                    Application.CreateOnBaseAuthenticationProperties(_config.Endpoint, username, password, _config.DataSource);
                authenticationProperties.LicenseType = LicenseType.Default;
                authenticationProperties.IsDisconnectEnabled = false;
                Log.Debug("OnBaseService:Login:Connect starts");
                app = Application.Connect(authenticationProperties);
                Log.Debug("OnBaseService:Login:Connect ends");
                if (app == null)
                    throw new AuthenticationException($"Session creation returned null for username: {username}",
                        ErrorCode.AuthenticationSessionNull);
                return app.SessionID;
            }
            catch (AuthenticationFailedException e)
            {
                throw new AuthenticationException($"Authentication error for {username}", ErrorCode.AuthenticationFailed);
            }
            catch (RestIntegrationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                Log.Error("There was an error creating an OnBase session. OnBaseService:Login", e);
                throw new ServiceException("Error in OnBaseService:Login", ErrorCode.ErrorUnexpected, e);
            }
        }

        public void Logout(string sessionId)
        {
            Application app = null;
            try
            {
                app = GetApplication(sessionId, false);
            }
            catch (Exception e)
            {
                Log.Error($"There was an error disconnecting the OnBase session {sessionId}. OnBaseService:Logout", e);
            }
            finally
            {
                app?.Disconnect();
            }
        }

        private Application GetApplication(string sessionId, bool keepConnection = true)
        {
            return Application.Connect(Application.CreateSessionIDAuthenticationProperties(_config.Endpoint, sessionId, !keepConnection));
        }

        private Keyword BuildKeyword(KeywordType type, string value)
        {
            try
            {
                switch (type.DataType)
                {
                    case KeywordDataType.Numeric9:
                    case KeywordDataType.Numeric20:
                        return type.CreateKeyword(Int64.Parse(value));
                    case KeywordDataType.AlphaNumeric:
                        return type.CreateKeyword(value);
                    case KeywordDataType.Currency:
                    case KeywordDataType.SpecificCurrency:
                    case KeywordDataType.FloatingPoint:
                        return type.CreateKeyword(Decimal.Parse(value));
                    case KeywordDataType.Date:
                    case KeywordDataType.DateTime:
                        return type.CreateKeyword(DateTime.ParseExact(value, _config.KeywordDateTimeFormat, CultureInfo.InvariantCulture));
                    default:
                        return type.CreateKeyword(value);
                }
            }
            catch (Exception e)
            {
                throw new ValidationException($"The provided value {value} is invalid for keyword type {type.DataType.ToString()}", ErrorCode.ValidationInvalidKeywordValue);
            }
        }

        public QueryResult Query(string sessionId, long queryId, List<KeywordFilter> filters)
        {
            Application app = null;
            try
            {
                app = GetApplication(sessionId);
                var queryType = app.Core.CustomQueries.Find(queryId);
                if (queryType == null)
                    throw new ValidationException($"The requested query is invalid, id: {queryId}", ErrorCode.ValidationInvalidQueryId);
                var query = app.Core.CreateDocumentQuery().AddCustomQuery(queryType);
                filters?.ForEach(filter =>
                {
                    var kwt = app.Core.KeywordTypes.Find(filter.KeywordId);
                    if(kwt== null)
                        throw new ValidationException($"The keyword type with id: {filter.KeywordId} doesn't exist", ErrorCode.ValidationInvalidKeywordTypeId);
                    query.AddKeyword(BuildKeyword(kwt, filter.Value), filter.Operator.Value, filter.Relation.Value);
                });
                var queryConfig = _config.Queries[queryId];
                for (int index = 0; index < queryConfig.Results.Count; index++)
                {
                    query.AddDisplayColumn(app.Core.KeywordTypes.Find(queryConfig.Results[index].Id));
                }

                return query.ExecuteQueryResults(query.ExecuteCount());
            }
            catch (RestIntegrationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                Log.Error("There was an error querying OnBase. OnBaseService:Query", e);
                throw new ServiceException("Error in OnBaseService:Query", ErrorCode.ErrorUnexpected, e);
            }
        }

        public IEnumerable<DocumentResponse> QueryAsDocumentResponse(string sessionId, long queryId, List<KeywordFilter> filters)
        {
            try
            {
                var results = Query(sessionId, queryId, filters);
                return results.QueryResultItems.Select(item =>
                {
                    var doc = new DocumentResponse(id: item.Document.ID, name: item.Document.Name);
                    foreach (var column in item.DisplayColumns)
                    {
                        if (column.IsBlank)
                            doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading, null));
                        else
                            switch (column.Configuration.DataType)
                            {
                                case KeywordDataType.Numeric9:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        column.Numeric9Value));
                                    break;
                                case KeywordDataType.Numeric20:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        column.Numeric20Value));
                                    break;
                                case KeywordDataType.AlphaNumeric:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        column.AlphaNumericValue));
                                    break;
                                case KeywordDataType.Currency:
                                case KeywordDataType.SpecificCurrency:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        column.CurrencyValue));
                                    break;
                                case KeywordDataType.FloatingPoint:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        column.FloatingPointValue));
                                    break;
                                case KeywordDataType.Date:
                                case KeywordDataType.DateTime:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        column.DateTimeValue));
                                    break;
                                default:
                                    doc.Keywords.Add(new DocumentKeywordResponse(column.Configuration.KeywordType.ID, column.Configuration.Heading,
                                        null));
                                    break;
                            }
                    }
                    return doc;
                });
            }
            catch (RestIntegrationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                Log.Error("There was an error querying OnBase. OnBaseService:QueryAsDocumentResponse", e);
                throw new ServiceException("Error in OnBaseService:QueryAsDocumentResponse", ErrorCode.ErrorUnexpected, e);
            }
        }

        public File GetDocumentAsFile(long id, string sessionId)
        {
            MemoryStream ms = null;
            PageData pd = null;
            Application app = null;
            try
            {
                var file = new File();
                app = GetApplication(sessionId);
                var doc = app.Core.GetDocumentByID(id);
                if(doc==null)
                    throw new ValidationException($"The requested document doesn't exist, id: {id}", ErrorCode.ValidationInvalidDocId);
                pd = app.Core.Retrieval.Native.GetDocument(doc.DefaultRenditionOfLatestRevision);
                ms = new MemoryStream();
                pd.Stream.CopyTo(ms);
                file.Content = ms;
                file.Name = $"{doc.Name}.{pd.Extension}";
                return file;
            }
            catch (RestIntegrationException e)
            {
                DisposeResource(ms);
                throw;
            }
            catch (Exception e)
            {
                DisposeResource(ms);
                Log.Error($"There was an error getting file {id}. OnBaseService:GetDoc", e);
                throw new ServiceException("Error in OnBaseService:GetDoc", ErrorCode.ErrorUnexpected, e);
            }
            finally
            {
                // DisposeResource(pd);
            }
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
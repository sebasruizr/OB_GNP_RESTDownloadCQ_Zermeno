using System;
using Artisoft.OnBase.Gnp.RestIntegration.Models;

namespace Artisoft.OnBase.Gnp.RestIntegration.Exceptions
{
    public class ServiceException : RestIntegrationException
    {
        public ServiceException(string message, ErrorCode errorCode) : base(message, errorCode)
        {
        }

        public ServiceException(string message, ErrorCode errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}
using System;
using Artisoft.OnBase.Gnp.RestIntegration.Models;

namespace Artisoft.OnBase.Gnp.RestIntegration.Exceptions
{
    public class RestIntegrationException : Exception
    {
        public ErrorCode ErrorCode { get; set; }

        public RestIntegrationException(string message, ErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public RestIntegrationException(string message, ErrorCode errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
using System;
using Artisoft.OnBase.Gnp.RestIntegration.Models;

namespace Artisoft.OnBase.Gnp.RestIntegration.Exceptions
{
    public class ValidationException : RestIntegrationException
    {
        public ValidationException(string message, ErrorCode errorCode) : base(message, errorCode)
        {
        }

        public ValidationException(string message, ErrorCode errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}
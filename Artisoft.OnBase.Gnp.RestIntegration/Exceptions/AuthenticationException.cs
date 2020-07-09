using System;
using Artisoft.OnBase.Gnp.RestIntegration.Models;

namespace Artisoft.OnBase.Gnp.RestIntegration.Exceptions
{
    public class AuthenticationException : RestIntegrationException
    {
        public AuthenticationException(string message, ErrorCode errorCode) : base(message, errorCode)
        {
        }

        public AuthenticationException(string message, ErrorCode errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}
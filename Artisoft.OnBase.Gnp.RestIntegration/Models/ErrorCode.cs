namespace Artisoft.OnBase.Gnp.RestIntegration.Models
{
    public enum ErrorCode
    {
        AuthenticationSessionNull = 1000,
        AuthenticationFailed = 1001,
        
        ValidationInvalidQueryId = 2000,
        ValidationInvalidKeywordValue = 2001,
        ValidationInvalidKeywordTypeId = 2002,
        ValidationInvalidDocId = 2003,
        
        ErrorUnexpected = 9000
    }
}
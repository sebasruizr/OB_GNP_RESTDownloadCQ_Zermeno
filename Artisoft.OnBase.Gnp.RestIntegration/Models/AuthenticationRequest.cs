using System.ComponentModel.DataAnnotations;

namespace Artisoft.OnBase.Gnp.RestIntegration.Models
{
    public class AuthenticationRequest
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
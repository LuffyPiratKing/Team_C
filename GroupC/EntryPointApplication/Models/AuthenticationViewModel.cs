using IdentityServer.Models;

namespace EntryPointApplication.Models
{
    public class AuthenticationViewModel
    {        
        public bool IsAuthenticatedUser { get; set; }
        public int TimeoutPeriod { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
        public Tokens? Tokens { get; set; }
    }
}

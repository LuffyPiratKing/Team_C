using IdentityServer.Models;

namespace IdentityServer.Interfaces
{
    public interface IJWTManager
    {
        HttpResponseMessage Authenticate (Users users);
    }
}

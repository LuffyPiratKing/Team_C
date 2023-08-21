using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace EntryPointApplication.Interfaces
{
    interface IRequestManager
    {
        Task<HttpResponseMessage> AuthenticateAsync(Users users);

        bool ValidateCurrentToken(string token);

        Task<HttpResponseMessage> RegenerateJwtTokenURIAsync(Tokens tokens);
    }
}

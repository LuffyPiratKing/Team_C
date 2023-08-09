using EntryPointApplication.Interfaces;
using IdentityServer.Models;
using System.Net.Http.Headers;
using EntryPointApplication.Constants;

namespace EntryPointApplication.ServiceClasses
{
    public class RequestManager : IRequestManager
    {
        public async Task<HttpResponseMessage> AuthenticateAsync(Users users)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(EntryPointApplicationConstants.IdentityServerBaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync(
                   EntryPointApplicationConstants.IdentityServerURI, users);

                return response;
            }
        }
    }
}

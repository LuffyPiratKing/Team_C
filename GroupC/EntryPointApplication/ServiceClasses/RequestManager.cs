using EntryPointApplication.Interfaces;
using IdentityServer.Models;
using System.Net.Http.Headers;
using EntryPointApplication.Constants;
using System.Net;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityServer.ServiceClasses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EntryPointApplication.ServiceClasses
{
    public class RequestManager : IRequestManager
    {
        private readonly IConfiguration _configuration;
        public RequestManager(IConfiguration configuration)
        {          
            _configuration = configuration;
        }

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

        public bool ValidateCurrentToken(string token)
        {
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var mySecurityKey = new SymmetricSecurityKey(tokenKey);
            var myIssuer = _configuration["Jwt:Issuer"];
            var myAudience = _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }            
            return true;
        }
    }
}

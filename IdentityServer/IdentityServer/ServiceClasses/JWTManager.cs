using IdentityServer.Interfaces;
using IdentityServer.Models;
using DatabaseAccessLayer.Classes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Net.Http.Json;
using System.Net;

namespace IdentityServer.ServiceClasses
{
    public class JWTManager : IJWTManager
    {
        private readonly IConfiguration _configuration;

        public JWTManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpResponseMessage Authenticate(Users users)    
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            Repository repository = new Repository();
            bool isUserAuthentic = repository.Authenticate(users.Name, users.Password);
            if (isUserAuthentic)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, users.Name)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenToBeSent = new Tokens { Token = tokenHandler.WriteToken(token) };

                JsonContent jsonContent = JsonContent.Create(tokenToBeSent);
                responseMessage.Content = jsonContent;
                responseMessage.StatusCode = HttpStatusCode.OK;             
                return responseMessage;
            }

            responseMessage.Content = null;
            responseMessage.StatusCode = HttpStatusCode.NoContent;
            return responseMessage;
        }
    }
}

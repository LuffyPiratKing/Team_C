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
using DatabaseAccessLayer.Models;
using Newtonsoft.Json.Linq;

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
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

                //create token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, users.Name)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                // create refresh token
                var refreshTokenHandler = new JwtSecurityTokenHandler();
                var refreshTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, "Refresh_" + users.Name)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var refreshToken = refreshTokenHandler.CreateToken(refreshTokenDescriptor);

                // prepare 'Tokens' object which will be sent to the client
                var tokenToBeSent = new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = refreshTokenHandler.WriteToken(refreshToken), UserName = users.Name };
                JsonContent jsonContent = JsonContent.Create(tokenToBeSent);

                //create the HTTP response message to be sent to the client when user exists
                responseMessage.Content = jsonContent;
                responseMessage.StatusCode = HttpStatusCode.OK;
                return responseMessage;              
            }

            //create the HTTP response message to be sent to the client when user does not exist
            responseMessage.Content = null;
            responseMessage.StatusCode = HttpStatusCode.NoContent;
            return responseMessage;
        }

        public HttpResponseMessage RegenerateJwtToken(Tokens tokens)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();            

            //create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, tokens.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // prepare 'Tokens' object which will be sent to the client
            var tokenToBeSent = new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = tokens.RefreshToken, UserName = tokens.UserName};
            JsonContent jsonContent = JsonContent.Create(tokenToBeSent);

            //create the HTTP response message to be sent to the client when user exists
            responseMessage.Content = jsonContent;
            responseMessage.StatusCode = HttpStatusCode.OK;
            return responseMessage;
        }
    }
}

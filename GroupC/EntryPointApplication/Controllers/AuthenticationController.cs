using EntryPointApplication.Models;
using Microsoft.AspNetCore.Mvc;
using IdentityServer.Models;
using IdentityServer.ServiceClasses;
using IdentityServer.Controllers;
using System.Diagnostics;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using System.Net;
using EntryPointApplication.Interfaces;
using EntryPointApplication.ServiceClasses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace EntryPointApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private bool ShouldRedirectToIndex { get; set; }

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
            ShouldRedirectToIndex = false;
        }
        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Authenticate(Users users)
        {
            IRequestManager requestManager = new RequestManager(_configuration);
            var response = requestManager.AuthenticateAsync(users).Result;
            var viewModel = PrepareAuthenticationViewModel(response);
            if (ShouldRedirectToIndex)
            {
                ShouldRedirectToIndex = false;
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }
        }
        
        public IActionResult CheckAndGenerateNewToken(string refreshTokenExpirationTimeString, string token, string refreshToken, string userName)
        {
            if(!string.IsNullOrEmpty(refreshTokenExpirationTimeString))
            {                
                var refreshTokenExpirationTime = Convert.ToDateTime(refreshTokenExpirationTimeString);
                var currentDateTime = DateTime.UtcNow;       
                
                if(refreshTokenExpirationTime > currentDateTime)
                {
                    Tokens tokens = new Tokens() { Token = token, RefreshToken = refreshToken, UserName = userName };
                    IRequestManager requestManager = new RequestManager(_configuration);
                    var response = requestManager.RegenerateJwtTokenURIAsync(tokens).Result;
                    var viewModel = PrepareAuthenticationViewModel(response);
                    if (ShouldRedirectToIndex)
                    {
                        ShouldRedirectToIndex = false;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Authenticate", viewModel);
                    }                    
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        private AuthenticationViewModel PrepareAuthenticationViewModel(HttpResponseMessage response)
        {
            AuthenticationViewModel viewModel = new AuthenticationViewModel();
            IRequestManager requestManager = new RequestManager(_configuration);
            DateTime currentDateTime = DateTime.UtcNow;

            if (response.StatusCode == HttpStatusCode.OK)
            {

                //extract the Tokens object from the response
                var tokensDoubleSerializedString = response.Content.ReadAsStringAsync().Result;
                var tokensJsonString = JsonConvert.DeserializeObject(tokensDoubleSerializedString).ToString();
                var tokensObject = JsonConvert.DeserializeObject<Tokens>(tokensJsonString);


                //send the extracted token string for validation
                var isTokenValid = requestManager.ValidateCurrentToken(tokensObject.Token);

                if (isTokenValid)
                {
                    //get the expiration time of the token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decryptedToken = tokenHandler.ReadToken(tokensObject.Token as string);
                    var expirationOfToken = decryptedToken.ValidTo;

                    //get the expiration time of the refresh token
                    var refreshTokenHandler = new JwtSecurityTokenHandler();
                    var decryptedRefreshToken = refreshTokenHandler.ReadToken(tokensObject.RefreshToken as string);
                    var expirationOfRefreshToken = decryptedRefreshToken.ValidTo;

                    if (expirationOfToken < currentDateTime)
                    {
                        ShouldRedirectToIndex = true;
                    }
                    else
                    {
                        viewModel.IsAuthenticatedUser = true;
                        
                        var dateDifference = expirationOfToken - currentDateTime;
                        viewModel.TimeoutPeriod = (int)dateDifference.TotalMilliseconds;

                        viewModel.RefreshTokenExpirationTime = expirationOfRefreshToken;

                        viewModel.Tokens = tokensObject;
                    }
                }
                else
                {
                    viewModel.IsAuthenticatedUser = false;
                    viewModel.TimeoutPeriod = 0;
                    viewModel.RefreshTokenExpirationTime = null;
                    viewModel.Tokens = new Tokens();
                }
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                viewModel.IsAuthenticatedUser = false;
                viewModel.TimeoutPeriod = 0;
                viewModel.RefreshTokenExpirationTime = null;
                viewModel.Tokens = new Tokens();
            }
                    
            return viewModel;
        }

       
    }
}

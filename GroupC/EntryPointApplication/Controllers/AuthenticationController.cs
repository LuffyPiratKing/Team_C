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

namespace EntryPointApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(Users users)
        {
            AuthenticationViewModel viewModel = new AuthenticationViewModel();
            IRequestManager requestManager = new RequestManager(_configuration);
            var response = requestManager.AuthenticateAsync(users).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {

                //extract the Tokens object from the response
                var tokensDoubleSerializedString = response.Content.ReadAsStringAsync().Result;
                var tokensJsonString = JsonConvert.DeserializeObject(tokensDoubleSerializedString).ToString();
                var tokensObject = JsonConvert.DeserializeObject<Tokens>(tokensJsonString);


                //send the extracted token string for validation
                var isTokenValid = requestManager.ValidateCurrentToken(tokensObject.Token);
                
                viewModel.IsAuthenticatedUser = isTokenValid ? true: false;
                
            }
            else if(response.StatusCode == HttpStatusCode.NoContent)
            {
                viewModel.IsAuthenticatedUser = false;
            }
            return View(viewModel);
        }
       
    }
}

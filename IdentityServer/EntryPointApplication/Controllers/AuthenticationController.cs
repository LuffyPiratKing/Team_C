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
        public IActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(Users users)
        {
            AuthenticationViewModel viewModel = new AuthenticationViewModel();
            IRequestManager requestManager = new RequestManager();

            var response = requestManager.AuthenticateAsync(users).Result;            
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                viewModel.IsAuthenticatedUser = true;
                
            }
            else if(response.StatusCode == HttpStatusCode.NoContent)
            {
                viewModel.IsAuthenticatedUser = false;
            }
            return View(viewModel);
        }
       
    }
}

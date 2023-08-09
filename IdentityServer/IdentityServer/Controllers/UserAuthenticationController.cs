using Microsoft.AspNetCore.Mvc;
using IdentityServer.ServiceClasses;
using IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace IdentityServer.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserAuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("api/UserAuthentication/AuthenticateUser")]
        public IActionResult AuthenticateUser([FromBody]Users users)
        {             
            JWTManager jWTManager = new JWTManager(_configuration);
            HttpResponseMessage response = jWTManager.Authenticate(users);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response);
            }
            else if(response.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}

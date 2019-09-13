using IntegrationTestSample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntegrationTestSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHostingEnvironment _env;

        public AccountController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel user)
        {
            ClaimsPrincipal claimsPrincipal = null;

            // Return a test user when environment is our Test environment
            if (_env.EnvironmentName == "Testing")
            {
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }, "Cookies");

                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            }
            else
            {
                // you should validate the userName and password here and load the specified user
                // We'll just return a default user to keep things simple...
            }
            await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
            return NoContent();
        }
    }
}

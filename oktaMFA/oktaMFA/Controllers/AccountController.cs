
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using oktaMFA.Service;
using System.Security.Claims;
using oktaMFA.Service;

namespace oktaMFA.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ITokenService _tokenService;

        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("Get Token")]
        public async Task<IActionResult> SignInAndGetToken(string username, string password)
        {
            var OktaToken = await _tokenService.GetToken(username, password);
            if (OktaToken != null)
            {
                return Ok(OktaToken);
            }
            return null;
        }
      

    }
}

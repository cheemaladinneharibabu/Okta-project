using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using oktaMFA.Models;
using oktaMFA.Service;
using System.Net.Http.Headers;

namespace oktaMFA.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ITokenService _tokenService;

        public UserController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _tokenService.GetUsers();
                return Ok(users);
            }
            catch (Exception error)
            {
                
                throw error;
            }
        }

    }

}


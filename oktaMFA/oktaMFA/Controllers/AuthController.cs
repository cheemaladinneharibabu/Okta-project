using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using oktaMFA.Models;
using oktaMFA.Service;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;

namespace oktaMFA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<OktaTokenSetting> _options;
       private readonly IMfaService _mfaService;
        public AuthController(IHttpClientFactory httpClientFactory, IOptions<OktaTokenSetting> options, IMfaService mfaservice)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _mfaService = mfaservice;
        }

        [HttpGet("list-of-factors")]
        public async Task<IActionResult>GetFactors(string userId)
        {
            try
            {
                var factorsResponse = await _mfaService.ListofFactor(userId);

                // Check if the factors response contains at least one factor
                if (factorsResponse != null && factorsResponse.Count > 0)
                {
                    // Return the Id of the first factor in the list
                    var factorId = factorsResponse[0].id;
                    return Ok(factorsResponse);
                }
                else
                {
                    return Ok(factorsResponse);
                }
            }
            catch (Exception error)
            {

                throw error;
            }
        }

        [HttpPost("authenticate-user")]
        public async Task<IActionResult> PrimaryLogin([FromBody] OktaAuthnRequest request)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var domain = _options.Value.Domain;
                var url = $"{domain}/api/v1/authn";

                var requestBody = JsonConvert.SerializeObject(new
                {
                    username = request.Username,
                    password = request.Password,
                    options = new
                    {
                        multiOptionalFactorEnroll = request.Options.MultiOptionalFactorEnroll,
                        warnBeforePasswordExpired = request.Options.WarnBeforePasswordExpired
                    }
                });

                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<OktaAuthnResponse>(responseContent);
                    return Ok(responseObject);
                }
                else
                {

                    var errorResponseContent = await response.Content.ReadAsStringAsync();
                   var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseContent);
                    return BadRequest($"Failed to login. Error: {errorResponseContent}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("enroll-user-factors")]
        public async Task<IActionResult> EnrollUserFactors(string userId, FactorPayload factorPayload)
        {
            try
            {
                var enrolledUser = await _mfaService.EnrollUser(userId , factorPayload);
                if(enrolledUser != null)
                {

                    return Ok(enrolledUser);
                }
                else
                {
                    return BadRequest("User is already enrolled"); 
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("activate-otp")]
        public async Task<IActionResult> ActivateFactor([FromBody] PasscodeRequest passcodeRequest)

        {
           
            var  otp = await _mfaService.ActivateOtp(passcodeRequest);
            if(otp != null)
            {
                return Ok(otp);
            }
            return null;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyFactor(string userId, string factorId, string passcode)
        {
            try
            {
                var verified = await _mfaService.VerifyOtp(userId, factorId, passcode);
                if (verified != null)
                {
                    return Ok(verified);
                }
                return null;

            }
            catch (HttpRequestException ex)
            {
              
                return BadRequest($"Failed to verify factor. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}


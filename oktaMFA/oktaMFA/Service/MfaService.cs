using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using oktaMFA.Models;
using oktaMFA.Service;

namespace oktaMFA.Service
{
    public class MfaService : IMfaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OktaTokenSetting _options;

        public MfaService(IOptions<OktaTokenSetting> options, IHttpClientFactory httpClientFactory)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PasscodeResponse> ActivateOtp(PasscodeRequest request)
        {
            var otp = new PasscodeResponse();
            var apiKey = _options.Api_key;
            var domain = _options.Domain;

            try
            {
                if (string.IsNullOrEmpty(request.factorId) || string.IsNullOrEmpty(request.userId) || string.IsNullOrEmpty(request.passcode.otp))
                {
                    throw new ArgumentException("Invalid or null parameters");
                }

                using (var client = _httpClientFactory.CreateClient())
                {
                    var url = $"{domain}/api/v1/users/{request.userId}/factors/{request.factorId}/lifecycle/activate";
                    var passCode = request.passcode.otp;
                     //set header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", apiKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   
                     var urlRequest = new HttpRequestMessage(HttpMethod.Post, url);

                    urlRequest.Content = new StringContent(JsonConvert.SerializeObject(new { passCode }), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(urlRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        otp = JsonConvert.DeserializeObject<PasscodeResponse>(json);
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(error);
                        if(errorResponse?.ErrorCode== "E0000001")
                        {
                            throw new Exception("Factor is already active");  
                        }
                        else
                        {
                            throw new ApplicationException(error);
                        }
                        
                    }
                }
                return otp;
            }
            catch (Exception ex)
            {
                // exception details here
                throw new ApplicationException("Failed to activate OTP", ex);
            }
        }

        public async Task<FactorEnrollResponse> EnrollUser(string userId, FactorPayload factorPayload)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Invalid user ID provided.");
            }
            var apiKey = _options.Api_key;
            var domain = _options.Domain;
            var httpClient = _httpClientFactory.CreateClient();
            var userFactorsUrl = $"{domain}/api/v1/users/{userId}/factors";
            // Set the Authorization header
            var factorPayloads = JsonConvert.SerializeObject(factorPayload);
            var content = new StringContent(factorPayloads, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", apiKey);
            var factorsResponse = await httpClient.PostAsync(userFactorsUrl, content);
            // Added API key as a header
            if (factorsResponse.IsSuccessStatusCode)
            {
                var factorsResponseContent = await factorsResponse.Content.ReadAsStringAsync();
                var factorResponse = JsonConvert.DeserializeObject<FactorEnrollResponse>(factorsResponseContent);
                return factorResponse;
            }
            else
            {   var errorResponseContent = await factorsResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseContent);
                if (errorResponse?.ErrorCode == "E0000001" )
                { //return BadRequest("Factor is already active.");
                    throw new Exception("Factor is already active.");
                }
                throw new ArgumentException($"Failed to enroll user factors. Error: {errorResponseContent}");
            }
        }

        public async Task<List<ListOfFactorResponse>>ListofFactor(string userId)
        {
            var apiKey = _options.Api_key;
            var domain = _options.Domain;
            //var httpClient = _httpClientFactory.CreateClient();

            var FactorUrl = $"{domain}/api/v1/users/{userId}/factors";
            using (var httpClient = new HttpClient())
            {
                // Set authorization header 
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", apiKey);
                var response = await httpClient.GetAsync(FactorUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var factors = JsonConvert.DeserializeObject<List<ListOfFactorResponse>>(json);

                    return factors;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to get users from Okta API. Status: {response.StatusCode}. Message: {errorMessage}");
                }
            }
        }

        public async Task<FactorResponse> VerifyOtp(string userId, string factorId, string passcode)
        {
            var apiKey =_options.Api_key;
           
            var oktaDomain = _options.Domain;
            var httpClient = _httpClientFactory.CreateClient();

            var verifyFactorUrl = $"{oktaDomain}/api/v1/users/{userId}/factors/{factorId}/verify";
            
            var verifyPayload = new { passCode = passcode };
            var content = new StringContent(JsonConvert.SerializeObject(verifyPayload), Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", apiKey);

            var response = await httpClient.PostAsync(verifyFactorUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var verifyResponse = JsonConvert.DeserializeObject<FactorResponse>(responseContent);

                // Handle success as needed
                return verifyResponse;
            }
            else
            {
                
                var errorResponseContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseContent);
                if(errorResponse.ErrorCode== "E0000068")
                {
                    throw new Exception(errorResponse.ErrorSummary);
                }
                // Handle error response appropriately
                // return BadRequest($"Failed to verify factor. Error: {}");
                throw new Exception(errorResponseContent);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using oktaMFA.Models;
using System.Net.Http.Headers;
using System.Text;

namespace oktaMFA.Service
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<OktaTokenSetting> _options;
        public TokenService(IOptions<OktaTokenSetting> options)
        {
            _options = options;
        }
        public async Task<OktaResponse> GetToken(string username, string password)
        {
            //logic to get the token based on username and password
            var token = new OktaResponse();
            var client = new HttpClient();
            var client_id = _options.Value.ClientId;
            var client_secret = _options.Value.ClientSecret;
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "password");
            postMessage.Add("username", username);
            postMessage.Add("password", password);
            postMessage.Add("scope", "openid");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.Domain}/oauth2/default/v1/token")
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerSetting = new JsonSerializerSettings();
                var json = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<OktaResponse>(json, jsonSerializerSetting);
                token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
            return token;
        }

      
        public async Task<List<UserResponse>> GetUsers()
        {

            var oktaDomain = _options.Value.Domain;
            var apiKey = _options.Value.Api_key; 

            // Okta API endpoint URL
            var oktaApiUrl = $"{oktaDomain}/api/v1/users";

           
            using (var httpClient = new HttpClient())
            {
               
                //var content = new StringContent( Encoding.UTF8, "application/json");

                // Set authorization header 
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SSWS", apiKey);
                var response = await httpClient.GetAsync(oktaApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<UserResponse>>(json);

                    return users; 
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to get users from Okta API. Status: {response.StatusCode}. Message: {errorMessage}");
                }
            }
        }

    }
}

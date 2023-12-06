using Newtonsoft.Json;

namespace oktaMFA.Models
{
    public class OktaAuthnResponse
    {
        public string ExpiresAt { get; set; }
        public string Status { get; set; }
        public string SessionToken { get; set; }
        [JsonProperty("_embedded")]
        public EmbeddedData Embedded { get; set; }   
    }

    public class EmbeddedData
    {    public UserDetail user { get; set; }
    }

    public class UserDetail
    {
        public string id { get; set; }
        public DateTime passwordChanged { get; set; }
        public UserProfile profile { get; set; }
    }
    public class UserProfile
    {
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string locale { get; set; }
        public string timeZone { get; set; }
    }

    

}

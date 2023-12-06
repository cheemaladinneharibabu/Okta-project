namespace oktaMFA.Models
{
    public class OktaTokenSetting
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationServerId { get; set; }
        public string Audience { get; set; }
        public string Domain { get; set; }
        public string Api_key { get; set; }
    }
}

using Newtonsoft.Json;

namespace oktaMFA.Models
{
    public class QRCodes
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}

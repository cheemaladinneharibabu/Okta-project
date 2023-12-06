using Newtonsoft.Json;

namespace oktaMFA.Models
{
    public class FactorEnrollResponse
    {
       
        public string Id { get; set; }
            public string FactorType { get; set; }
            public string Provider { get; set; }
            public string VendorName { get; set; }
            public string Status { get; set; }
            public DateTime Created { get; set; }
            public DateTime LastUpdated { get; set; }
           [JsonProperty(PropertyName = "_embedded")]
            public _Embedded Embedded { get; set; }
                                      
    }
        public class _Embedded
        {
        [JsonProperty(PropertyName = "activation")]
        public Activation Activation { get; set; }
        }
    public class Activation
    {
        public int TimeStep { get; set; }
        public string SharedSecret { get; set; }
        public string Encoding { get; set; }
        public int KeyLength { get; set; }
        public string FactorResult { get; set; }
        [JsonProperty(PropertyName = "_links")]
        public Links links { get; set; }
    }
    public class Links
    {  [JsonProperty(PropertyName = "qrcode")]
        public QRCode QRCode { get; set; }
    }
    public class QRCode
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }




}

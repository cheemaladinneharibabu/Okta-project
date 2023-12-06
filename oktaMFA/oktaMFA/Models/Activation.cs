//using Newtonsoft.Json;
//using System.Text.Json.Serialization;

//namespace oktaMFA.Models
//{
//    public class Activation
//    {
//        public int TimeStep { get; set; }
//        public string SharedSecret { get; set; }
//        public string Encoding { get; set; }
//        public int KeyLength { get; set; }
//        public string FactorResult { get; set; }
//        [JsonProperty (PropertyName = "_links")]
//        public Links links { get; set; }
//    }
//    public class Links
//    {
//        [JsonProperty(PropertyName = "qrcode")]
//        public QRCode QRCode { get; set; }
//    }
//    public class QRCode
//    {
//        [JsonProperty(PropertyName = "href")]
//        public string Href { get; set; }
//        [JsonProperty(PropertyName = "type")]
//        public string Type { get; set; }
//    }
//}

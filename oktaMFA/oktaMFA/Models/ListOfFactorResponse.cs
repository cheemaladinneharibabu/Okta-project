using System.IdentityModel.Tokens.Jwt;

namespace oktaMFA.Models
{
    public class ListOfFactorResponse
    {
        
            public string id { get; set; }
            public string factorType { get; set; }
            public string provider { get; set; }
            public string vendorName { get; set; }
            public string status { get; set; }
            public DateTime created { get; set; }
            public DateTime lastUpdated { get; set; }
            public Profile profile { get; set; }
        
    }
    
}

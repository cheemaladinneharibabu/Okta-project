using Newtonsoft.Json.Linq;

namespace oktaMFA.Models
{
    public class EnrollFactorRequest
    {
        public string UserId { get; set; }
        public FactorPayload FactorPayload { get; set; }


    }
}

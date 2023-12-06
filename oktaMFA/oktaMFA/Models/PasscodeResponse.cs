namespace oktaMFA.Models
{
    public class PasscodeResponse
    {
        public string Id { get; set; }
        public string factorType { get; set; }
        public string provider { get; set; }
        public string vendorName { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public Profile profile  { get; set; }


    }
    public class Profile
    {
        public string credentialId { get; set; }
    }
}

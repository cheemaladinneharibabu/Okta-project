namespace oktaMFA.Models
{
    public class OktaAuthnRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public OktaAuthnOptions Options { get; set; }
    }

    public class OktaAuthnOptions
    {
        public bool MultiOptionalFactorEnroll { get; set; }
        public bool WarnBeforePasswordExpired { get; set; }
    }
}

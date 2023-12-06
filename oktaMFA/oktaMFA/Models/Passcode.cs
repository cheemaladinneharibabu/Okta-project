namespace oktaMFA.Models
{
    public class PasscodeRequest
    {
       public  Passcode passcode { get; set; }
        public string factorId { get; set; }
        public string userId { get; set; }
    }

    public class Passcode
    {
        public string otp { get; set; }
    }

}

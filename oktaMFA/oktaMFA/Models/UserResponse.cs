namespace oktaMFA.Models
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public UserType Type { get; set; }
        public UserProfile Profile { get; set; }

        public class UserType
        {
            public string Id { get; set; }
        }

        public class UserProfile
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MobilePhone { get; set; }
            public string SecondEmail { get; set; }
            public string Login { get; set; }
            public string Email { get; set; }
        }

 
    }
}

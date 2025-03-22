namespace ePizza.Models.Response
{
    public class UserResponseModel
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<string> Roles { get; set; }
    }


    public class ValidateUserResponse
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int UserId { get; set; }
        public List<string> Roles { get; set; } 

    }
}

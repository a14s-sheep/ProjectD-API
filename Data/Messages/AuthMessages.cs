namespace ProjectD_API.Data.Messages
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}

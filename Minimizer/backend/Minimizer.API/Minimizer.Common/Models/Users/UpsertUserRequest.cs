namespace Minimizer.Common.Models.Users
{
    public class UpsertUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

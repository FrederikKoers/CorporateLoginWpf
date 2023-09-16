namespace CorporateLogin.Common.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Institute Institute { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool Blocked { get; set; }
        public bool Verified { get; set; }
        public short FailedPasswordAttempts { get; set; }
    }
}
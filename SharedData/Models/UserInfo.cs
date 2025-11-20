namespace CNSAWiki.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}

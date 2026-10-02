namespace Shortlist.Api
{
    public sealed class UserRecord
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }
}

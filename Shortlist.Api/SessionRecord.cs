namespace Shortlist.Api;

public sealed class SessionRecord
{
    public string Token { get; set; } = "";
    public string UserId { get; set; } = "";
    public string CreatedAt { get; set; } = "";
}
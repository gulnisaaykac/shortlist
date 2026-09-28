namespace Shortlist;

public sealed class ApplicationRecord
//sealed başka classların bu classı miras almasını engeller.    
{
    public string Id { get; set; } =""; 
    public string Company { get; set; } ="";
    public string Role { get; set; } ="";
    public string Status { get; set; } ="applied";
    public string CreatedAt { get; set; } ="";
}
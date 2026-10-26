using System.Text.Json;

namespace Shortlist.Api;

public static class SessionStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static string FilePath =>
        Path.Combine(Directory.GetCurrentDirectory(), "data", "sessions.json");

    public static List<SessionRecord> Load()
    {
        var path = FilePath;
        if (!File.Exists(path))
            return new List<SessionRecord>();

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<SessionRecord>>(json)
               ?? new List<SessionRecord>();
    }

    public static void Save(List<SessionRecord> items)
    {
        var dir = Path.GetDirectoryName(FilePath)!;
        Directory.CreateDirectory(dir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(items, JsonOptions));
    }
}
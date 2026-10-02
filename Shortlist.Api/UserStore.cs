using System.Text.Json;

namespace Shortlist.Api;

public static class UserStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static string FilePath =>
        Path.Combine(Directory.GetCurrentDirectory(), "data", "users.json");

    public static List<UserRecord> Load()
    {
        var path = FilePath;
        if (!File.Exists(path))
            return new List<UserRecord>();

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<UserRecord>>(json)
               ?? new List<UserRecord>();
    }

    public static void Save(List<UserRecord> items)
    {
        var dir = Path.GetDirectoryName(FilePath)!;
        Directory.CreateDirectory(dir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(items, JsonOptions));
    }
}
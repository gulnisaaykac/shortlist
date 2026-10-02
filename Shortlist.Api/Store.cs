using System.Text.Json;//json okuyup yazan hazır library

namespace Shortlist.Api;

public static class Store
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true //jsonu tek satır değil girintili yazmak için
    };

    public static string FilePath =>
        Path.Combine(Directory.GetCurrentDirectory(), "data", "applications.json");
    // => her okunuşta sağ tarafı hesapla demek

    public static List<ApplicationRecord> Load()
    {
        var path = FilePath;//yolu bir kez al aşağıda kullan

        if (!File.Exists(path))
            return new List<ApplicationRecord>();//hata değil boş liste fırlat

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ApplicationRecord>>(json)//metni c# listesine çevir
               ?? new List<ApplicationRecord>();//null gelirse çökme
    }

    public static void Save(List<ApplicationRecord> items)
    {
        var dir = Path.GetDirectoryName(FilePath)!;
        Directory.CreateDirectory(dir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(items, JsonOptions));
    }
}
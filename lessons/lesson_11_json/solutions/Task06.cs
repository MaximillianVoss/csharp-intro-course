using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task06
{
    public sealed class Settings { public int Volume { get; set; } = 50; }
    private static void PathValid(string path) { if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path required."); }
    public static void Save(string path, Settings settings)
    {
        PathValid(path); ArgumentNullException.ThrowIfNull(settings);
        if (settings.Volume < 0 || settings.Volume > 100) throw new ArgumentException("Volume.");
        File.WriteAllText(path, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
    }
    public static Settings Load(string path)
    {
        PathValid(path);
        string json;
        try { json = File.ReadAllText(path); } catch (FileNotFoundException) { return new Settings(); }
        var s = JsonSerializer.Deserialize<Settings>(json);
        if (s is null || s.Volume < 0 || s.Volume > 100) throw new JsonException("Volume.");
        return s;
    }
    public static void Run()
    {
        var dir = Path.Combine(Path.GetTempPath(), "csharp-json-" + Guid.NewGuid()); Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "settings.json"); Check.Equal(50, Load(path).Volume);
            Save(path, new Settings { Volume = 0 }); Check.Equal(0, Load(path).Volume);
            Check.True(File.ReadAllText(path).Contains('\n'));
            Save(path, new Settings { Volume = 100 }); Check.Equal(100, Load(path).Volume);
            File.WriteAllText(path, "{"); Check.Throws<JsonException>(() => Load(path));
            File.WriteAllText(path, "{\"Volume\":101}"); Check.Throws<JsonException>(() => Load(path));
            Check.Throws<ArgumentException>(() => Save(path, new Settings { Volume = -1 }));
            Check.Throws<ArgumentNullException>(() => Save(path, null!)); Check.Throws<ArgumentException>(() => Load(""));
            Check.Throws<DirectoryNotFoundException>(() => Load(Path.Combine(dir, "absent-directory", "file.json")));
        }
        finally { Directory.Delete(dir, true); }
    }
}

using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task11
{
    public sealed record Config(string Name, int Retries);
    public static Config Read(string json)
    {
        ArgumentNullException.ThrowIfNull(json); using var doc = JsonDocument.Parse(json); var root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("version", out var v) || !TryGetInt32Safe(v, out var version) || version < 1 || version > 2) throw new JsonException("Version.");
        var key = version == 1 ? "label" : "name";
        if (!root.TryGetProperty(key, out var name) || name.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(name.GetString())) throw new JsonException("Name.");
        int retries = 3;
        if (version == 2 && (!root.TryGetProperty("retries", out var r) || r.ValueKind != JsonValueKind.Number || !r.TryGetInt32(out retries) || retries < 0 || retries > 5)) throw new JsonException("Retries.");
        return new Config(name.GetString()!, retries);
    }
    private static bool TryGetInt32Safe(JsonElement value, out int number)
    {
        number = 0; return value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out number);
    }
    public static void Run()
    {
        Check.Equal(new Config("demo", 3), Read("{\"version\":1,\"label\":\"demo\"}"));
        Check.Equal(new Config("x", 0), Read("{\"version\":2,\"name\":\"x\",\"retries\":0}"));
        foreach (var json in new[] { "{\"version\":3}", "{\"version\":2,\"name\":\"x\"}", "{\"version\":\"1\"}", "null", "{\"version\":1,\"label\":\" \"}" }) Check.Throws<JsonException>(() => Read(json));
    }
}

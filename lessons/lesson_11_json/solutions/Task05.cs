using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task05
{
    public static string Label(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object) throw new JsonException("Object required.");
        if (!root.TryGetProperty("label", out var value)) return "нет";
        if (value.ValueKind == JsonValueKind.Null) return "пусто";
        if (value.ValueKind != JsonValueKind.String) throw new JsonException("String required.");
        return value.GetString()!;
    }
    public static void Run()
    {
        Check.Equal("нет", Label("{}")); Check.Equal("пусто", Label("{\"label\":null}"));
        Check.Equal("", Label("{\"label\":\"\"}")); Check.Equal("x", Label("{\"label\":\"x\"}"));
        Check.Throws<JsonException>(() => Label("{\"label\":4}")); Check.Throws<JsonException>(() => Label("[]"));
        Check.Throws<ArgumentNullException>(() => Label(null!));
    }
}

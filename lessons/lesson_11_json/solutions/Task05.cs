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
        return Decode(value)!;
    }
    private static string? Decode(JsonElement value)
    {
        try { return value.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode string.", error); }
    }
    public static void Run()
    {
        Check.Equal("нет", Label("{}")); Check.Equal("пусто", Label("{\"label\":null}"));
        Check.Equal("", Label("{\"label\":\"\"}")); Check.Equal("x", Label("{\"label\":\"x\"}"));
        Check.Throws<JsonException>(() => Label("{\"label\":4}")); Check.Throws<JsonException>(() => Label("[]"));
        Check.Throws<ArgumentNullException>(() => Label(null!));
        Check.Throws<JsonException>(() => Label("{\"label\":\"\\uDC00\"}"));
        Check.Equal("\uD83D\uDE00", Label("{\"label\":\"\\uD83D\\uDE00\"}"));
    }
}

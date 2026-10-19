using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task14
{
    public sealed record Profile(string Name, string? Bio);
    public static Profile Patch(Profile original, string json)
    {
        ArgumentNullException.ThrowIfNull(original); ArgumentNullException.ThrowIfNull(json);
        if (string.IsNullOrWhiteSpace(original.Name)) throw new ArgumentException("Original name.");
        using var doc = JsonDocument.Parse(json); if (doc.RootElement.ValueKind != JsonValueKind.Object) throw new JsonException("Object.");
        string name = original.Name; string? bio = original.Bio; var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in doc.RootElement.EnumerateObject())
        {
            var key = PropertyName(property);
            if (!seen.Add(key)) throw new JsonException("Duplicate.");
            if (key == "name" && property.Value.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(Decode(property.Value))) name = Decode(property.Value)!;
            else if (key == "bio" && (property.Value.ValueKind == JsonValueKind.String || property.Value.ValueKind == JsonValueKind.Null)) bio = Decode(property.Value);
            else throw new JsonException("Invalid patch.");
        }
        return new Profile(name, bio);
    }
    private static string? Decode(JsonElement value)
    {
        try { return value.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode string.", error); }
    }
    private static string PropertyName(JsonProperty value)
    {
        try { return value.Name; }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode property name.", error); }
    }
    public static void Run()
    {
        var old = new Profile("Ada", "old"); Check.Equal(old, Patch(old, "{}"));
        Check.Equal(new Profile("Ada", null), Patch(old, "{\"bio\":null}"));
        Check.Equal(new Profile("New", "old"), Patch(old, "{\"name\":\"New\"}")); Check.Equal("Ada", old.Name);
        foreach (var json in new[] { "{\"extra\":1}", "{\"bio\":1}", "{\"name\":\" \"}", "{\"bio\":null,\"bio\":\"x\"}", "[]" }) Check.Throws<JsonException>(() => Patch(old, json));
        Check.Throws<ArgumentNullException>(() => Patch(null!, "{}"));
        Check.Throws<JsonException>(() => Patch(old, "{\"name\":\"New\",\"bio\":\"\\uD800\"}")); Check.Equal(new Profile("Ada", "old"), old);
        Check.Throws<JsonException>(() => Patch(old, "{\"\\uDC00\":null}"));
        Check.Throws<ArgumentException>(() => Patch(new Profile(" ", null), "{}"));
        Check.Throws<ArgumentNullException>(() => Patch(old, null!));
    }
}

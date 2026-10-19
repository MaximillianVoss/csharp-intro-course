using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task04
{
    public static JsonElement ReadDetached(string json)
    {
        ArgumentNullException.ThrowIfNull(json); using var document = JsonDocument.Parse(json);
        try { ValidateStrings(document.RootElement); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode in JSON.", error); }
        return document.RootElement.Clone();
    }
    private static JsonElement Broken(string json) { using var document = JsonDocument.Parse(json); return document.RootElement; }
    private static void ValidateStrings(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.String) { _ = value.GetString(); }
        else if (value.ValueKind == JsonValueKind.Array)
            foreach (var item in value.EnumerateArray()) ValidateStrings(item);
        else if (value.ValueKind == JsonValueKind.Object)
            foreach (var property in value.EnumerateObject()) { _ = property.Name; ValidateStrings(property.Value); }
    }
    public static void Run()
    {
        var broken = Broken("{\"Name\":\"Ada\"}"); Check.Throws<ObjectDisposedException>(() => broken.GetProperty("Name").GetString());
        var element = ReadDetached("{\"Name\":\"Ada\",\"Nested\":{\"Count\":2}}"); Check.Equal("Ada", element.GetProperty("Name").GetString()); Check.Equal(2, element.GetProperty("Nested").GetProperty("Count").GetInt32());
        var array = ReadDetached("[1,2]"); Check.Equal(2, array.GetArrayLength()); Check.Equal(2, array[1].GetInt32()); Check.Equal(JsonValueKind.Null, ReadDetached("null").ValueKind);
        Check.Throws<JsonException>(() => ReadDetached("{")); Check.Throws<ArgumentNullException>(() => ReadDetached(null!));
        Check.Throws<JsonException>(() => ReadDetached("{\"nested\":[\"\\uD800\"]}"));
        Check.Throws<JsonException>(() => ReadDetached("{\"\\uDC00\":1}"));
        Check.Equal("\uD83D\uDE00", ReadDetached("\"\\uD83D\\uDE00\"").GetString());
    }
}

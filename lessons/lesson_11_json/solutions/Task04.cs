using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task04
{
    public static JsonElement ReadDetached(string json)
    {
        ArgumentNullException.ThrowIfNull(json); using var document = JsonDocument.Parse(json); return document.RootElement.Clone();
    }
    private static JsonElement Broken(string json) { using var document = JsonDocument.Parse(json); return document.RootElement; }
    public static void Run()
    {
        var broken = Broken("{\"Name\":\"Ada\"}"); Check.Throws<ObjectDisposedException>(() => broken.GetProperty("Name").GetString());
        var element = ReadDetached("{\"Name\":\"Ada\",\"Nested\":{\"Count\":2}}"); Check.Equal("Ada", element.GetProperty("Name").GetString()); Check.Equal(2, element.GetProperty("Nested").GetProperty("Count").GetInt32());
        var array = ReadDetached("[1,2]"); Check.Equal(2, array.GetArrayLength()); Check.Equal(2, array[1].GetInt32()); Check.Equal(JsonValueKind.Null, ReadDetached("null").ValueKind);
        Check.Throws<JsonException>(() => ReadDetached("{")); Check.Throws<ArgumentNullException>(() => ReadDetached(null!));
    }
}

using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task03
{
    public sealed class Settings { public int Limit { get; set; } = 9; public int Counter = 4; }
    public static (int Limit, int Counter) Observe(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var s = JsonSerializer.Deserialize<Settings>(json) ?? throw new JsonException("Null root.");
        return (s.Limit, s.Counter);
    }
    public static void Run()
    {
        var s = JsonSerializer.Deserialize<Settings>("{\"limit\":2,\"Counter\":7}")!;
        Check.Equal((9, 4), (s.Limit, s.Counter));
        Check.Equal((9, 4), Observe("{\"limit\":2,\"Counter\":7}"));
        Check.Equal((2, 4), Observe("{\"Limit\":2,\"Counter\":7}"));
        Check.Equal((9, 4), Observe("{}"));
        Check.Throws<JsonException>(() => Observe("null"));
        Check.Throws<ArgumentNullException>(() => Observe(null!));
    }
}

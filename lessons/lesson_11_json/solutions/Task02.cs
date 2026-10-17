using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task02
{
    public sealed class Reading { public decimal Value { get; set; } public string? Unit { get; set; } }
    public static Reading Read(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var value = JsonSerializer.Deserialize<Reading>(json);
        if (value is null || value.Value < -1000 || value.Value > 1000 || (value.Unit != "C" && value.Unit != "F"))
            throw new JsonException("Invalid reading.");
        return value;
    }
    public static void Run()
    {
        Check.Equal(12.5m, Read("{\"Value\":12.5,\"Unit\":\"C\"}").Value);
        Check.Equal(-1000m, Read("{\"Value\":-1000,\"Unit\":\"F\"}").Value);
        Check.Equal(1000m, Read("{\"Value\":1000,\"Unit\":\"C\"}").Value);
        Check.Equal(0m, Read("{\"Unit\":\"C\",\"Extra\":true}").Value);
        foreach (var json in new[] { "null", "{", "{\"Unit\":\"K\"}", "{\"Value\":1001,\"Unit\":\"C\"}", "{\"Value\":\"1\",\"Unit\":\"C\"}" })
            Check.Throws<JsonException>(() => Read(json));
        Check.Throws<ArgumentNullException>(() => Read(null!));
    }
}

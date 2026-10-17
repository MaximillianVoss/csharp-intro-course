using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task13
{
    public enum Status { Pending, Done }
    private static JsonSerializerOptions Options()
    {
        var options = new JsonSerializerOptions(); options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)); return options;
    }
    public static Status Read(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.String) throw new JsonException("Status must be a string.");
        string? name;
        try { name = document.RootElement.GetString(); }
        catch (InvalidOperationException error) { throw new JsonException("Invalid Unicode in status name.", error); }
        if (string.Equals(name, nameof(Status.Pending), StringComparison.OrdinalIgnoreCase)) return Status.Pending;
        if (string.Equals(name, nameof(Status.Done), StringComparison.OrdinalIgnoreCase)) return Status.Done;
        throw new JsonException("Unknown status name.");
    }
    public static string Write(Status status)
    {
        if (!Enum.IsDefined(status)) throw new ArgumentOutOfRangeException(nameof(status));
        return JsonSerializer.Serialize(status, Options());
    }
    public static void Run()
    {
        Check.Equal(Status.Done, Read("\"done\"")); Check.Equal(Status.Pending, Read("\"pEnDiNg\""));
        Check.Equal(Status.Done, Read(" \r\n\"Done\" \t"));
        Check.Equal("\"Pending\"", Write(Status.Pending)); Check.Equal("\"Done\"", Write(Status.Done));
        Check.Equal(Status.Pending, Read(Write(Status.Pending))); Check.Equal(Status.Done, Read(Write(Status.Done)));
        foreach (var json in new[] { "1", "\"1\"", "\"Lost\"", "null", "{}", "[]", "true", "\"Pending, Done\"", "\"Done, Done\"", "\" done \"", "\"Pending \"", "\"\\tDone\"", "\"\"", "\"\\uD800\"", "{" })
            Check.Throws<JsonException>(() => Read(json));
        Check.Throws<ArgumentOutOfRangeException>(() => Write((Status)99));
        Check.Throws<ArgumentNullException>(() => Read(null!));
    }
}

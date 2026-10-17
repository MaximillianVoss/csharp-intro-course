using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task09
{
    public static string BuildMessage(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsHighSurrogate(text[i]))
            {
                if (i + 1 >= text.Length || !char.IsLowSurrogate(text[i + 1])) throw new ArgumentException("Unpaired high surrogate.", nameof(text));
                i++;
            }
            else if (char.IsLowSurrogate(text[i])) throw new ArgumentException("Unpaired low surrogate.", nameof(text));
        }
        using var buffer = new MemoryStream();
        using (var writer = new Utf8JsonWriter(buffer)) { writer.WriteStartObject(); writer.WriteString("text", text); writer.WriteEndObject(); writer.Flush(); }
        return System.Text.Encoding.UTF8.GetString(buffer.ToArray());
    }
    public static void Run()
    {
        foreach (var value in new[] { "", "обычный", "a\"b", "\\\n\t\r\0", "}{\"admin\":true}", "до\uD83D\uDE00после" })
        {
            using var doc = JsonDocument.Parse(BuildMessage(value));
            Check.Equal(value, doc.RootElement.GetProperty("text").GetString());
            int count = 0; foreach (var property in doc.RootElement.EnumerateObject()) count++; Check.Equal(1, count);
        }
        var broken = "{\"text\":\"" + "a\"b" + "\"}";
        Check.Throws<JsonException>(() => { using var doc = JsonDocument.Parse(broken); });
        foreach (var invalid in new[] { "\uD800", "\uDC00", "a\uD800b", "\uDC00\uD800", "\uD800\uD800" })
            Check.Throws<ArgumentException>(() => BuildMessage(invalid));
        Check.Throws<ArgumentNullException>(() => BuildMessage(null!));
    }
}

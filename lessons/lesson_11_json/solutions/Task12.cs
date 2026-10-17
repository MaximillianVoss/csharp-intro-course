using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task12
{
    public sealed class Item { public int Id { get; set; } }
    public sealed class Report { public List<int> Ids { get; } = new(); public List<int> ErrorLines { get; } = new(); }
    public static Report Import(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader); var report = new Report(); int lineNo = 0; string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNo++; if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                var item = JsonSerializer.Deserialize<Item>(line);
                if (item is null || item.Id <= 0) throw new JsonException("Id.");
                report.Ids.Add(item.Id);
            }
            catch (JsonException) { report.ErrorLines.Add(lineNo); }
        }
        return report;
    }
    public static void Run()
    {
        using var reader = new StringReader("{\"Id\":2}\n\n{bad}\n{\"Id\":3}\n{\"Id\":2}\nnull\n{}\n{\"Id\":-1}");
        var report = Import(reader); Check.SequenceEqual(new[] { 2, 3, 2 }, report.Ids); Check.SequenceEqual(new[] { 3, 6, 7, 8 }, report.ErrorLines);
        Check.Equal("", reader.ReadToEnd());
        using var empty = new StringReader(""); Check.Equal(0, Import(empty).Ids.Count);
        Check.Throws<ArgumentNullException>(() => Import(null!));
    }
}

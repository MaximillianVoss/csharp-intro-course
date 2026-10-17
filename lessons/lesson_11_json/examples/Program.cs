using System.Text.Json;
using Course;

var input = new Card { Name = "Книга \"C#\"", Count = 2 };
string json = JsonSerializer.Serialize(input, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(json);
var restored = JsonSerializer.Deserialize<Card>(json) ?? throw new JsonException("Null card.");
Check.Equal(input.Name, restored.Name);
Check.Equal(2, restored.Count);
using var doc = JsonDocument.Parse("{\"label\":null}");
Check.True(doc.RootElement.TryGetProperty("label", out var label));
Check.Equal(JsonValueKind.Null, label.ValueKind);
Console.WriteLine("Имя и количество восстановлены; null отличается от отсутствующего свойства.");

public sealed class Card
{
    public string Name { get; set; } = "";
    public int Count { get; set; }
}

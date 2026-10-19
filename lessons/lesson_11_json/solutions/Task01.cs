using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task01
{
    public sealed class Card { public string Name { get; set; } = ""; public int Count { get; set; } }
    public static string Serialize(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);
        if (string.IsNullOrWhiteSpace(card.Name) || card.Count < 0 || !WellFormed(card.Name)) throw new ArgumentException("Invalid card.");
        return JsonSerializer.Serialize(card);
    }
    private static bool WellFormed(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsHighSurrogate(text[i])) { if (i + 1 == text.Length || !char.IsLowSurrogate(text[i + 1])) return false; i++; }
            else if (char.IsLowSurrogate(text[i])) return false;
        }
        return true;
    }
    public static void Run()
    {
        using var doc = JsonDocument.Parse(Serialize(new Card { Name = "чай", Count = 2 }));
        Check.Equal("чай", doc.RootElement.GetProperty("Name").GetString());
        Check.Equal(2, doc.RootElement.GetProperty("Count").GetInt32());
        var text = "a\"b\n";
        Check.Equal(text, JsonSerializer.Deserialize<Card>(Serialize(new Card { Name = text }))!.Name);
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = " " }));
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = "a", Count = -1 }));
        Check.Throws<ArgumentNullException>(() => Serialize(null!));
        var unicode = " до\uD83D\uDE00после ";
        Check.Equal(unicode, JsonSerializer.Deserialize<Card>(Serialize(new Card { Name = unicode }))!.Name);
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = "\uD800" }));
        Check.Throws<ArgumentException>(() => Serialize(new Card { Name = "\uDC00" }));
    }
}

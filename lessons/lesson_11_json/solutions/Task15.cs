using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task15
{
    public sealed class Item { public int Id { get; set; } public string? Name { get; set; } }
    public sealed class Catalog
    {
        private Item?[] items = Array.Empty<Item?>();
        public int Count => items.Length;
        public void Import(string json)
        {
            ArgumentNullException.ThrowIfNull(json);
            var next = JsonSerializer.Deserialize<Item?[]>(json) ?? throw new JsonException("Array required.");
            var ids = new HashSet<int>();
            foreach (var item in next)
                if (item is null || item.Id <= 0 || string.IsNullOrWhiteSpace(item.Name) || !ids.Add(item.Id)) throw new JsonException("Invalid item.");
            items = next;
        }
        public string Export() => JsonSerializer.Serialize(items);
    }
    public static void Run()
    {
        var catalog = new Catalog(); catalog.Import("[{\"Id\":2,\"Name\":\"B\"},{\"Id\":1,\"Name\":\"A\"}]");
        var before = catalog.Export(); Check.Equal(2, catalog.Count);
        Check.Equal(2, JsonSerializer.Deserialize<Item[]>(before)![0].Id);
        Check.Throws<JsonException>(() => catalog.Import("[{\"Id\":3,\"Name\":\"C\"},{\"Id\":3,\"Name\":\"D\"}]")); Check.Equal(before, catalog.Export());
        foreach (var bad in new[] { "[null]", "null", "[{\"Id\":1,\"Name\":\" \"}]" }) { Check.Throws<JsonException>(() => catalog.Import(bad)); Check.Equal(before, catalog.Export()); }
        catalog.Import("[]"); Check.Equal(0, catalog.Count);
        Check.Throws<ArgumentNullException>(() => catalog.Import(null!));
    }
}

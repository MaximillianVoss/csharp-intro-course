using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task07
{
    public sealed class Order { public Line?[]? Lines { get; set; } }
    public sealed class Line { public int Quantity { get; set; } public decimal Price { get; set; } }
    public static decimal Total(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var order = JsonSerializer.Deserialize<Order>(json);
        if (order?.Lines is null) throw new JsonException("Lines required.");
        decimal total = 0;
        foreach (var line in order.Lines)
        {
            if (line is null || line.Quantity < 1 || line.Quantity > 1000 || line.Price < 0 || line.Price > 1000000) throw new JsonException("Invalid line.");
            total = checked(total + line.Quantity * line.Price);
        }
        return total;
    }
    public static void Run()
    {
        Check.Equal(2.50m, Total("{\"Lines\":[{\"Quantity\":2,\"Price\":1.25},{\"Quantity\":1,\"Price\":0}]}"));
        Check.Equal(0m, Total("{\"Lines\":[]}"));
        Check.Equal(1000000000m, Total("{\"Lines\":[{\"Quantity\":1000,\"Price\":1000000}]}"));
        foreach (var json in new[] { "{}", "{\"Lines\":null}", "{\"Lines\":[null]}", "{\"Lines\":[{\"Quantity\":0}]}", "{\"Lines\":[{\"Quantity\":1,\"Price\":-1}]}" }) Check.Throws<JsonException>(() => Total(json));
        Check.Throws<ArgumentNullException>(() => Total(null!));
    }
}

using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task08
{
    public static (double Lat, double Lon) ReadPoint(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        double Number(string name, double bound)
        {
            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty(name, out var v) || v.ValueKind != JsonValueKind.Number || !v.TryGetDouble(out var n) || !double.IsFinite(n) || n < -bound || n > bound)
                throw new JsonException("Invalid coordinate.");
            return n;
        }
        return (Number("lat", 90), Number("lon", 180));
    }
    public static void Run()
    {
        var p = ReadPoint("{\"lat\":55.75,\"lon\":37.62}"); Check.Near(55.75, p.Lat); Check.Near(37.62, p.Lon);
        Check.Equal((-90d, 180d), ReadPoint("{\"lat\":-90,\"lon\":180}"));
        foreach (var json in new[] { "{}", "[]", "{\"lat\":\"0\",\"lon\":0}", "{\"lat\":91,\"lon\":0}", "{\"lat\":0,\"lon\":1e999}" }) Check.Throws<JsonException>(() => ReadPoint(json));
        Check.Equal((90d, -180d), ReadPoint("{\"lat\":90,\"lon\":-180}"));
        Check.Throws<ArgumentNullException>(() => ReadPoint(null!));
    }
}

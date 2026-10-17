using Course;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Course.Lesson11;

public static class Task10
{
    public static int[] RoundTrip(int[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return JsonSerializer.Deserialize<int[]>(JsonSerializer.Serialize(values))!;
    }
    private static int[] Broken(int[] values) { var result = RoundTrip(values); Array.Sort(result); return result; }
    public static void Run()
    {
        Check.SequenceEqual(Array.Empty<int>(), RoundTrip(Array.Empty<int>()));
        var input = new[] { 3, -1, 3, 0 }; var output = RoundTrip(input);
        Check.SequenceEqual(input, output); var sorted = Broken(input); bool differs = false; for (int i = 0; i < input.Length; i++) if (input[i] != sorted[i]) differs = true; Check.True(differs);
        output[0] = 99; Check.Equal(3, input[0]);
        Check.Throws<ArgumentNullException>(() => RoundTrip(null!));
    }
}

namespace Course.Lesson07;

public static class Task02
{
    public static Dictionary<string, int> CountCodes(string[] codes)
    {
        Dictionary<string, int> counts = new(StringComparer.OrdinalIgnoreCase);
        foreach (string code in codes)
        {
            counts.TryGetValue(code, out int count);
            counts[code] = count + 1;
        }
        return counts;
    }
    public static void Run()
    {
        Dictionary<string, int> result = CountCodes(new[] { "ab", "AB", "x" });
        Check.Equal(2, result["aB"]); Check.Equal(1, result["X"]); Check.Equal(2, result.Count);
        Check.Equal(0, CountCodes(Array.Empty<string>()).Count);
        Check.Equal(2, CountCodes(new[] { "ab", "a-b" }).Count);
    }
}

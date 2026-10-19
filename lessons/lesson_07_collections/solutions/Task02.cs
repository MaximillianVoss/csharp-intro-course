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
        System.Globalization.CultureInfo previous = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
            Dictionary<string, int> technical = CountCodes(new[] { "I", "i" });
            Check.Equal(1, technical.Count); Check.Equal(2, technical["i"]);
        }
        finally { System.Globalization.CultureInfo.CurrentCulture = previous; }
    }
}

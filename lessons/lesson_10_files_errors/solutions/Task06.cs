namespace Course.Lesson10;

public static class Task06
{
    public sealed record ParseReport(int[] Values, int[] InvalidLines);
    public static ParseReport ParseNumbers(TextReader reader)
    {
        List<int> values = new(); List<int> invalid = new(); int lineNumber = 0; string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++; if (string.IsNullOrWhiteSpace(line)) continue;
            if (int.TryParse(line, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value)) values.Add(value);
            else invalid.Add(lineNumber);
        }
        return new ParseReport(values.ToArray(), invalid.ToArray());
    }
    public static void Run()
    {
        using StringReader input = new("10\n\nwrong\n -2 \n999999999999"); ParseReport report = ParseNumbers(input);
        Check.SequenceEqual(new[] { 10, -2 }, report.Values); Check.SequenceEqual(new[] { 3, 5 }, report.InvalidLines);
        using StringReader zero = new("bad\n0\n \n7"); ParseReport after = ParseNumbers(zero);
        Check.SequenceEqual(new[] { 0, 7 }, after.Values); Check.SequenceEqual(new[] { 1 }, after.InvalidLines);
        using StringReader empty = new(""); Check.Equal(0, ParseNumbers(empty).Values.Length);
    }
}

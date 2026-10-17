namespace Course.Lesson10;

public static class Task15
{
    public sealed record ImportSummary(long Sum, int Accepted, string[] Errors);
    public static ImportSummary SummarizeFiles(string directory)
    {
        string[] files = Directory.GetFiles(directory); Array.Sort(files, StringComparer.Ordinal);
        long sum = 0; int accepted = 0; List<string> errors = new();
        foreach (string path in files)
        {
            if (!string.Equals(Path.GetExtension(path), ".txt", StringComparison.Ordinal)) continue;
            using StreamReader reader = new(path, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            int lineNumber = 0; string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                lineNumber++; if (string.IsNullOrWhiteSpace(line)) continue;
                if (int.TryParse(line, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value)) { sum += value; accepted++; }
                else errors.Add(Path.GetFileName(path) + ":" + lineNumber.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }
        return new ImportSummary(sum, accepted, errors.ToArray());
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            ImportSummary empty = SummarizeFiles(directory); Check.Equal(0L, empty.Sum); Check.Equal(0, empty.Accepted); Check.Equal(0, empty.Errors.Length);
            File.WriteAllText(Path.Combine(directory, "b.txt"), "wrong\n5"); File.WriteAllText(Path.Combine(directory, "a.txt"), "2\nbad\n3");
            File.WriteAllText(Path.Combine(directory, "ignore.dat"), "99"); File.WriteAllText(Path.Combine(directory, "caps.TXT"), "99");
            ImportSummary result = SummarizeFiles(directory); Check.Equal(10L, result.Sum); Check.Equal(3, result.Accepted);
            Check.SequenceEqual(new[] { "a.txt:2", "b.txt:1" }, result.Errors);
            File.WriteAllText(Path.Combine(directory, "large.txt"), "2147483647\n\n2147483647\nwrong");
            ImportSummary large = SummarizeFiles(directory); Check.Equal(4294967304L, large.Sum); Check.Equal(5, large.Accepted);
            Check.SequenceEqual(new[] { "a.txt:2", "b.txt:1", "large.txt:4" }, large.Errors);
        }
        finally { Directory.Delete(directory, true); }
    }
}

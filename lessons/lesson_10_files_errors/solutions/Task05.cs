namespace Course.Lesson10;

public static class Task05
{
    public static int CountNonBlank(TextReader reader)
    {
        int count = 0; string? line;
        while ((line = reader.ReadLine()) is not null) if (!string.IsNullOrWhiteSpace(line)) count++;
        return count;
    }
    public static void Run()
    {
        using StringReader normal = new("A\n\n   \r\nB\n"); Check.Equal(2, CountNonBlank(normal));
        Check.True(normal.ReadLine() is null);
        using StringReader empty = new(""); Check.Equal(0, CountNonBlank(empty));
        using StringReader spaces = new(" \t\n\r\n"); Check.Equal(0, CountNonBlank(spaces));
        using StringReader last = new("last"); Check.Equal(1, CountNonBlank(last));
    }
}

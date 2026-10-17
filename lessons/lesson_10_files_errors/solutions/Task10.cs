namespace Course.Lesson10;

public static class Task10
{
    public static string? FirstLongLine(TextReader reader, int limit)
    {
        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit));
        string? line;
        while ((line = reader.ReadLine()) is not null) if (line.Length > limit) return line;
        return null;
    }
    public static void Run()
    {
        using StringReader source = new("aa\nlong\ntail"); Check.Equal("long", FirstLongLine(source, 2)); Check.Equal("tail", source.ReadLine());
        using StringReader exact = new("ab\nabc"); Check.Equal("abc", FirstLongLine(exact, 2));
        using StringReader none = new("a\nb"); Check.True(FirstLongLine(none, 2) is null);
        using StringReader empty = new(""); Check.True(FirstLongLine(empty, 0) is null);
        using StringReader invalid = new("stay"); Check.Throws<ArgumentOutOfRangeException>(() => FirstLongLine(invalid, -1)); Check.Equal("stay", invalid.ReadLine());
    }
}

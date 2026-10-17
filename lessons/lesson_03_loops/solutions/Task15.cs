using System;
using Course;
using System.IO;

namespace Course.Lesson03;

public static class Task15
{
    public static string TryCode(TextReader reader, int limit)
    {
        for (int attempt = 1; attempt <= limit; attempt++)
        {
            string? line = reader.ReadLine();
            if (line is null) return "eof";
            if (line == "0420") return $"ok:{attempt}";
        }
        return "locked";
    }

    public static void Run()
    {
        using var first = new StringReader("0420\n");
        Check.Equal("ok:1", TryCode(first, 3));
        using var second = new StringReader("bad\n0420\nnext\n");
        Check.Equal("ok:2", TryCode(second, 3));
        Check.Equal("next", second.ReadLine());
        using var locked = new StringReader("1\n2\n3\n0420\n");
        Check.Equal("locked", TryCode(locked, 3));
        Check.Equal("0420", locked.ReadLine());
        using var eof = new StringReader("bad\n");
        Check.Equal("eof", TryCode(eof, 3));
        using var emptyLine = new StringReader("\n0420\n");
        Check.Equal("locked", TryCode(emptyLine, 1));
        using var numeric = new StringReader("420\n");
        Check.Equal("locked", TryCode(numeric, 1));
    }
}

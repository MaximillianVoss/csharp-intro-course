using System;
using Course;

namespace Course.Lesson02;

public static class Task01
{
    public static string ReadCount(string? text)
    {
        if (text is null) return "eof";
        if (!int.TryParse(text, out int value)) return "invalid";
        if (value < 0 || value > 100) return "range";
        return $"ok:{value}";
    }

    public static void Run()
    {
        Check.Equal("eof", ReadCount(null));
        Check.Equal("invalid", ReadCount(""));
        Check.Equal("ok:7", ReadCount(" 007 "));
        Check.Equal("ok:0", ReadCount("0"));
        Check.Equal("ok:100", ReadCount("100"));
        Check.Equal("range", ReadCount("-1"));
        Check.Equal("range", ReadCount("101"));
        Check.Equal("invalid", ReadCount("999999999999999999"));
        Check.Equal("invalid", ReadCount("1.5"));
    }
}

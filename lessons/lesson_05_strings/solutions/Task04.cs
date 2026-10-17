using System;
using Course;

namespace Course.Lesson05;

public static class Task04
{
    public static bool IsAsciiDigits(string? text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        foreach (char ch in text)
            if (ch < '0' || ch > '9') return false;
        return true;
    }

    public static void Run()
    {
        Check.True(IsAsciiDigits("007"));
        Check.True(IsAsciiDigits(new string('0', 1000)));
        Check.Equal(false, IsAsciiDigits(null));
        Check.Equal(false, IsAsciiDigits(""));
        Check.Equal(false, IsAsciiDigits("+12"));
        Check.Equal(false, IsAsciiDigits("12 "));
        Check.Equal(false, IsAsciiDigits("١٢"));
        Check.Equal(false, IsAsciiDigits("1a2"));
    }
}

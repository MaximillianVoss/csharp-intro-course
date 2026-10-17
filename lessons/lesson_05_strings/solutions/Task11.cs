using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task11
{
    public static string FirstScalar(string text)
    {
        foreach (var rune in text.EnumerateRunes()) return rune.ToString();
        return "";
    }
    public static string Wrong(string text)
    {
        return text.Length == 0 ? "" : text.Substring(0, 1);
    }

    public static void Run()
    {
        Check.Equal("", FirstScalar(""));
        Check.Equal("a", FirstScalar("abc"));
        Check.Equal("😀", FirstScalar("😀x"));
        Check.Equal("e", FirstScalar("e\u0301"));
        string broken = Wrong("😀x");
        Check.Equal(1, broken.Length);
        Check.True(char.IsHighSurrogate(broken[0]));
        Check.True(broken != FirstScalar("😀x"));
    }
}

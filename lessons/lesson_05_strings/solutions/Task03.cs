using System;
using Course;

namespace Course.Lesson05;

public static class Task03
{
    public static string Trace(string text)
    {
        string original = text;
        string copy = original;
        copy = copy.Replace('a', 'o');
        return original + ":" + copy;
    }
    public static string Ignored(string text)
    {
        text.Replace('a', 'o');
        return text;
    }

    public static void Run()
    {
        Check.Equal("banana:bonono", Trace("banana"));
        Check.Equal(":", Trace(""));
        Check.Equal("xyz:xyz", Trace("xyz"));
        Check.Equal("banana", Ignored("banana"));
    }
}

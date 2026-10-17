using System;
using Course;
using System.Globalization;

namespace Course.Lesson05;

public static class Task12
{
    public static string TakeElements(string text, int limit)
    {
        var information = new StringInfo(text);
        int count = Math.Min(limit, information.LengthInTextElements);
        if (count == 0) return "";
        return information.SubstringByTextElements(0, count);
    }

    public static void Run()
    {
        Check.Equal("", TakeElements("abc", 0));
        Check.Equal("", TakeElements("", 10));
        Check.Equal("abc", TakeElements("abc", 10));
        Check.Equal("e\u0301", TakeElements("e\u0301x", 1));
        Check.Equal("😀", TakeElements("😀x", 1));
        Check.Equal("👨‍👩‍👧‍👦", TakeElements("👨‍👩‍👧‍👦x", 1));
    }
}

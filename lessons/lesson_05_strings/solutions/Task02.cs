using System;
using Course;

namespace Course.Lesson05;

public static class Task02
{
    public static string Middle(string text)
    {
        if (text.Length == 0) return "";
        int length = text.Length % 2 == 0 ? 2 : 1;
        return text.Substring((text.Length - length) / 2, length);
    }

    public static void Run()
    {
        Check.Equal("", Middle(""));
        Check.Equal("A", Middle("A"));
        Check.Equal("Ab", Middle("Ab"));
        Check.Equal("bc", Middle("abcd"));
        Check.Equal("c", Middle("abcde"));
    }
}

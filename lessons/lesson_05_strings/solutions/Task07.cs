using System;
using Course;

namespace Course.Lesson05;

public static class Task07
{
    public static string Broken(string text)
    {
        int position = text.IndexOf(':');
        return text.Substring(position);
    }
    public static string AfterColon(string text)
    {
        int position = text.IndexOf(':');
        if (position < 0) return "";
        return text.Substring(position + 1);
    }

    public static void Run()
    {
        Check.Equal(":value", Broken("name:value"));
        Check.Throws<ArgumentOutOfRangeException>(() => Broken("name"));
        Check.Equal("value:tail", AfterColon("name:value:tail"));
        Check.Equal("", AfterColon("name:"));
        Check.Equal("value", AfterColon(":value"));
        Check.Equal("", AfterColon("name"));
        Check.Equal("", AfterColon(""));
    }
}

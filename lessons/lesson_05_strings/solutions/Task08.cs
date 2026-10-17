using System;
using Course;
using System.Text;

namespace Course.Lesson05;

public static class Task08
{
    public static string CollapseWhitespace(string text)
    {
        var builder = new StringBuilder();
        bool pending = false;
        foreach (char ch in text)
        {
            if (char.IsWhiteSpace(ch))
            {
                if (builder.Length > 0) pending = true;
                continue;
            }
            if (pending) builder.Append(' ');
            builder.Append(ch);
            pending = false;
        }
        return builder.ToString();
    }

    public static void Run()
    {
        Check.Equal("a b", CollapseWhitespace("  a\t\nb  "));
        Check.Equal("", CollapseWhitespace(" \t\n"));
        Check.Equal("", CollapseWhitespace(""));
        Check.Equal("a b", CollapseWhitespace("a\u00A0b"));
        Check.Equal("😀 text", CollapseWhitespace(" 😀  text "));
        Check.Equal("a b", CollapseWhitespace("a b"));
    }
}

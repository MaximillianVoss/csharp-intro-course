using System;
using Course;

namespace Course.Lesson05;

public static class Task15
{
    public static bool IsKey(string text)
    {
        static bool Letter(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        }
        if (text.Length < 1 || text.Length > 20 || !Letter(text[0])) return false;
        for (int index = 1; index < text.Length; index++)
        {
            char ch = text[index];
            if (!Letter(ch) && !(ch >= '0' && ch <= '9') && ch != '_') return false;
        }
        return true;
    }
    public static bool TryAssignment(string? line, out string key, out string value)
    {
        key = "";
        value = "";
        if (line is null) return false;
        int separator = line.IndexOf('=');
        if (separator < 0) return false;
        string candidate = line.Substring(0, separator).Trim();
        if (!IsKey(candidate)) return false;
        string content = line.Substring(separator + 1).Trim();
        key = candidate;
        value = content;
        return true;
    }

    public static void Run()
    {
        Check.True(TryAssignment(" mode = a=b ", out string key, out string value));
        Check.Equal("mode", key);
        Check.Equal("a=b", value);
        Check.True(TryAssignment("A_2=", out key, out value));
        Check.Equal("A_2", key);
        Check.Equal("", value);
        Check.Equal(false, TryAssignment("2mode=x", out key, out value));
        Check.Equal("", key);
        Check.Equal("", value);
        Check.Equal(false, IsKey("Мode"));
        Check.True(IsKey(new string('a', 20)));
        Check.Equal(false, IsKey(new string('a', 21)));
        Check.Equal(false, TryAssignment("mode", out key, out value));
        Check.Equal(false, TryAssignment(null, out key, out value));
    }
}

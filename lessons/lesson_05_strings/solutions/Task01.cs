using System;
using Course;

namespace Course.Lesson05;

public static class Task01
{
    public static bool TryChar(string? text, int index, out char value)
    {
        value = '\0';
        if (text is null || index < 0 || index >= text.Length) return false;
        value = text[index];
        return true;
    }

    public static void Run()
    {
        Check.True(TryChar("кот", 1, out char value));
        Check.Equal('о', value);
        Check.Equal(false, TryChar("кот", 3, out value));
        Check.Equal('\0', value);
        Check.Equal(false, TryChar("кот", -1, out value));
        Check.Equal(false, TryChar("", 0, out value));
        Check.Equal(false, TryChar(null, 0, out value));
        Check.Equal('\0', value);
        Check.True(TryChar("😀", 0, out value));
        Check.True(char.IsHighSurrogate(value));
    }
}

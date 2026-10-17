using System;
using Course;

namespace Course.Lesson04;

public static class Task11
{
    public static bool TryPositive(string? text, out int value)
    {
        value = 0;
        if (!int.TryParse(text, out int parsed) || parsed <= 0) return false;
        value = parsed;
        return true;
    }
    public static bool Wrong(string? text, out int value)
    {
        return int.TryParse(text, out value) && value > 0;
    }

    public static void Run()
    {
        Check.True(TryPositive("7", out int value));
        Check.Equal(7, value);
        Check.Equal(false, TryPositive("0", out value));
        Check.Equal(0, value);
        Check.Equal(false, TryPositive("-5", out value));
        Check.Equal(0, value);
        Check.Equal(false, TryPositive(null, out value));
        Check.Equal(0, value);
        Check.Equal(false, TryPositive("999999999999999999", out value));
        Check.Equal(0, value);
        Check.Equal(false, Wrong("-5", out value));
        Check.Equal(-5, value);
    }
}

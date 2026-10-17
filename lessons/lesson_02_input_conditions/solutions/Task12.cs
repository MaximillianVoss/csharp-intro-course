using System;
using Course;

namespace Course.Lesson02;

public static class Task12
{
    public static string Region(int x, int y)
    {
        if (x == 0 && y == 0) return "origin";
        if (y == 0) return "x-axis";
        if (x == 0) return "y-axis";
        if (x > 0) return y > 0 ? "I" : "IV";
        return y > 0 ? "II" : "III";
    }

    public static void Run()
    {
        Check.Equal("origin", Region(0, 0));
        Check.Equal("x-axis", Region(-3, 0));
        Check.Equal("y-axis", Region(0, -7));
        Check.Equal("I", Region(2, 3));
        Check.Equal("II", Region(-2, 3));
        Check.Equal("III", Region(-2, -3));
        Check.Equal("IV", Region(2, -3));
        Check.Equal("II", Region(int.MinValue, int.MaxValue));
    }
}

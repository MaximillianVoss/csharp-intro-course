using System;
using Course;

namespace Course.Lesson04;

public static class Task10
{
    public static string Trace(int n)
    {
        if (n == 0) return "0";
        return $"{n}[{Trace(n - 1)}]{n}";
    }

    public static void Run()
    {
        Check.Equal("0", Trace(0));
        Check.Equal("1[0]1", Trace(1));
        Check.Equal("2[1[0]1]2", Trace(2));
        Check.Equal("3[2[1[0]1]2]3", Trace(3));
    }
}

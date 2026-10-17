using System;
using Course;

namespace Course.Lesson01;

public static class Task06
{
    public static string Trace(int start)
    {
        int x = start;
        int y = x + 2;
        x = y * 3;
        y = x - y;
        return $"{x}:{y}";
    }

    public static void Run()
    {
        Check.Equal("18:12", Trace(4));
        Check.Equal("6:4", Trace(0));
        Check.Equal("0:0", Trace(-2));
    }
}

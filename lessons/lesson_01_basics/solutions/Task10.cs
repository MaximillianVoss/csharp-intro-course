using System;
using Course;

namespace Course.Lesson01;

public static class Task10
{
    public static string Parts(int a, int b)
    {
        return $"{a / b}:{a % b}";
    }

    public static void Run()
    {
        Check.Equal("-2:-1", Parts(-7, 3));
        Check.Equal("-2:1", Parts(7, -3));
        Check.Equal("2:-1", Parts(-7, -3));
        Check.Equal("0:0", Parts(0, 3));
        Check.Equal(-7, (-7 / 3) * 3 + (-7 % 3));
    }
}

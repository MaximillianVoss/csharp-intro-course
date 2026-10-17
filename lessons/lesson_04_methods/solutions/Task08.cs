using System;
using Course;

namespace Course.Lesson04;

public static class Task08
{
    public static long Manhattan(int x1, int y1, int x2, int y2)
    {
        static long Absolute(long value) { return value < 0 ? -value : value; }
        long dx = (long)x1 - x2;
        long dy = (long)y1 - y2;
        return Absolute(dx) + Absolute(dy);
    }

    public static void Run()
    {
        Check.Equal(7L, Manhattan(1, 2, 4, -2));
        Check.Equal(0L, Manhattan(5, 5, 5, 5));
        Check.Equal(7L, Manhattan(4, -2, 1, 2));
        Check.Equal(8_589_934_590L, Manhattan(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));
    }
}

using System;
using Course;

namespace Course.Lesson01;

public static class Task13
{
    public static bool Within(double measured, double expected, double tolerance)
    {
        return Math.Abs(measured - expected) <= tolerance;
    }

    public static void Run()
    {
        Check.True(Within(9.5, 10, 0.5));
        Check.True(Within(10.5, 10, 0.5));
        Check.Equal(false, Within(9.49, 10, 0.5));
        Check.Equal(false, Within(-100, 10, 0.5));
        Check.True(Within(3, 3, 0));
        Check.True(Within(0.1 + 0.2, 0.3, 1e-12));
    }
}

using System;
using Course;

namespace Course.Lesson04;

public static class Task01
{
    public static int Clamp(int value, int minimum, int maximum)
    {
        if (value < minimum) return minimum;
        if (value > maximum) return maximum;
        return value;
    }

    public static void Run()
    {
        Check.Equal(0, Clamp(-2, 0, 10));
        Check.Equal(4, Clamp(4, 0, 10));
        Check.Equal(10, Clamp(12, 0, 10));
        Check.Equal(0, Clamp(0, 0, 10));
        Check.Equal(10, Clamp(10, 0, 10));
        Check.Equal(7, Clamp(100, 7, 7));
        Check.Equal(int.MinValue, Clamp(int.MinValue, int.MinValue, int.MaxValue));
    }
}

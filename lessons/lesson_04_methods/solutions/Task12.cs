using System;
using Course;

namespace Course.Lesson04;

public static class Task12
{
    public static long Power(long basis, int exponent)
    {
        long result = 1;
        long factor = basis;
        int remaining = exponent;
        while (remaining > 0)
        {
            if (remaining % 2 != 0) result = checked(result * factor);
            remaining /= 2;
            if (remaining > 0) factor = checked(factor * factor);
        }
        return result;
    }

    public static void Run()
    {
        Check.Equal(1L, Power(0, 0));
        Check.Equal(1L, Power(7, 0));
        Check.Equal(0L, Power(0, 10));
        Check.Equal(-32L, Power(-2, 5));
        Check.Equal(16L, Power(-2, 4));
        Check.Equal(1_000_000_000_000_000_000L, Power(10, 18));
    }
}

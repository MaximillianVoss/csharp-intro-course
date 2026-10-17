using System;
using Course;

namespace Course.Lesson03;

public static class Task03
{
    public static long Factorial(int n)
    {
        long result = 1;
        for (int factor = 2; factor <= n; factor++) result = checked(result * factor);
        return result;
    }

    public static void Run()
    {
        Check.Equal(1L, Factorial(0));
        Check.Equal(1L, Factorial(1));
        Check.Equal(120L, Factorial(5));
        Check.Equal(2_432_902_008_176_640_000L, Factorial(20));
    }
}

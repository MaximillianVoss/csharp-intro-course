using System;
using Course;

namespace Course.Lesson02;

public static class Task13
{
    public static int CompareFractions(int a, int b, int c, int d)
    {
        long left = (long)a * d;
        long right = (long)c * b;
        if (left < right) return -1;
        if (left > right) return 1;
        return 0;
    }

    public static void Run()
    {
        Check.Equal(0, CompareFractions(1, 3, 2, 6));
        Check.Equal(-1, CompareFractions(1, 2, 2, 3));
        Check.Equal(1, CompareFractions(2, 3, 1, 2));
        Check.Equal(-1, CompareFractions(-3, 4, -1, 2));
        Check.Equal(1, CompareFractions(999_999, 1_000_000, 999_998, 999_999));
    }
}

using System;
using Course;

namespace Course.Lesson03;

public static class Task14
{
    public static int FloorSqrt(int n)
    {
        long left = 0;
        long right = (long)n + 1;
        while (right - left > 1)
        {
            long middle = left + (right - left) / 2;
            if (middle * middle <= n) left = middle;
            else right = middle;
        }
        return (int)left;
    }

    public static void Run()
    {
        Check.Equal(0, FloorSqrt(0));
        Check.Equal(1, FloorSqrt(1));
        Check.Equal(3, FloorSqrt(15));
        Check.Equal(4, FloorSqrt(16));
        Check.Equal(4, FloorSqrt(17));
        Check.Equal(46_340, FloorSqrt(2_147_395_600));
        Check.Equal(46_340, FloorSqrt(int.MaxValue));
    }
}

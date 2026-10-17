using System;
using Course;

namespace Course.Lesson01;

public static class Task09
{
    public static long Original(long x)
    {
        return x * x + 2 * x + 1;
    }
    public static long Evaluate(long x)
    {
        long shifted = x + 1;
        return shifted * shifted;
    }

    public static void Run()
    {
        Check.Equal(0L, Evaluate(-1));
        Check.Equal(16L, Evaluate(3));
        Check.Equal(4L, Evaluate(-3));
        Check.Equal(Original(-1_000_000), Evaluate(-1_000_000));
        Check.Equal(Original(1_000_000), Evaluate(1_000_000));
    }
}

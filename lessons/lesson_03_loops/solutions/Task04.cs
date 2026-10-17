using System;
using Course;

namespace Course.Lesson03;

public static class Task04
{
    public static long Capacity(int requested)
    {
        long power = 1;
        while (power < requested) power *= 2;
        return power;
    }

    public static void Run()
    {
        Check.Equal(1L, Capacity(0));
        Check.Equal(1L, Capacity(1));
        Check.Equal(8L, Capacity(8));
        Check.Equal(16L, Capacity(9));
        Check.Equal(1_073_741_824L, Capacity(1_000_000_000));
    }
}

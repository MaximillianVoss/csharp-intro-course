using System;
using Course;

namespace Course.Lesson03;

public static class Task01
{
    public static long SumTo(int n)
    {
        long sum = 0;
        for (int i = 1; i <= n; i++) sum += i;
        return sum;
    }

    public static void Run()
    {
        Check.Equal(0L, SumTo(0));
        Check.Equal(1L, SumTo(1));
        Check.Equal(10L, SumTo(4));
        Check.Equal(500_000_500_000L, SumTo(1_000_000));
    }
}

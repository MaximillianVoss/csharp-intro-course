using System;
using Course;

namespace Course.Lesson01;

public static class Task03
{
    public static double Truncated(int total, int count)
    {
        double mean = total / count;
        return mean;
    }
    public static double Mean(int total, int count)
    {
        return (double)total / count;
    }

    public static void Run()
    {
        Check.Equal(3.0, Truncated(7, 2));
        Check.Near(3.5, Mean(7, 2));
        Check.Near(-3.5, Mean(-7, 2));
        Check.Near(4, Mean(8, 2));
        Check.Near(0, Mean(0, 100));
    }
}

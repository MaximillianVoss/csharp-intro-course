using System;
using Course;

namespace Course.Lesson01;

public static class Task08
{
    public static double Increased(int start)
    {
        double value = start;
        value = value * 1.5;
        return value;
    }

    public static void Run()
    {
        Check.Near(4.5, Increased(3));
        Check.Near(-4.5, Increased(-3));
        Check.Near(0, Increased(0));
        Check.Near(1_500_000, Increased(1_000_000));
    }
}

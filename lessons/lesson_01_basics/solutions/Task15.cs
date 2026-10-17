using System;
using Course;

namespace Course.Lesson01;

public static class Task15
{
    public static int Cans(double width, double height, double coverage)
    {
        double area = width * height;
        double required = Math.Ceiling(area / coverage);
        return checked((int)required);
    }

    public static void Run()
    {
        Check.Equal(4, Cans(4, 2.5, 3));
        Check.Equal(2, Cans(3, 2, 3));
        Check.Equal(1, Cans(0.1, 0.1, 100));
        Check.Equal(0, Cans(0, 10, 3));
        Check.Equal(1_000_000_000, Cans(10_000, 10_000, 0.1));
    }
}

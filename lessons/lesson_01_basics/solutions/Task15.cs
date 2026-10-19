using System;
using Course;

namespace Course.Lesson01;

public static class Task15
{
    public static int Cans(decimal width, decimal height, decimal coverage)
    {
        decimal area = width * height;
        decimal required = Math.Ceiling(area / coverage);
        return checked((int)required);
    }

    public static void Run()
    {
        Check.Equal(4, Cans(4m, 2.5m, 3m));
        Check.Equal(2, Cans(3m, 2m, 3m));
        Check.Equal(1, Cans(0.001m, 0.001m, 10_000m));
        Check.Equal(0, Cans(0m, 10m, 3m));
        Check.Equal(1_000_000_000, Cans(10_000m, 10_000m, 0.1m));
        Check.Equal(3, Cans(0.1m, 3m, 0.1m));
        Check.Equal(3, Cans(0.099m, 3m, 0.1m));
        Check.Equal(4, Cans(0.101m, 3m, 0.1m));
    }
}

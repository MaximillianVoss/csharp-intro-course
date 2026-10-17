using System;
using Course;

namespace Course.Lesson03;

public static class Task10
{
    public static int Years(decimal initial, decimal target, int maxYears)
    {
        decimal balance = initial;
        if (balance >= target) return 0;
        for (int year = 1; year <= maxYears; year++)
        {
            balance = Math.Round(balance * 1.10m, 2, MidpointRounding.AwayFromZero);
            if (balance >= target) return year;
        }
        return -1;
    }

    public static void Run()
    {
        Check.Equal(2, Years(100m, 121m, 5));
        Check.Equal(0, Years(100m, 99m, 0));
        Check.Equal(-1, Years(100m, 101m, 0));
        Check.Equal(-1, Years(0m, 1m, 30));
        Check.Equal(-1, Years(0.01m, 0.02m, 30));
        Check.Equal(-1, Years(100m, 200m, 2));
    }
}

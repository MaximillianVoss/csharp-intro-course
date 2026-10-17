using System;
using Course;

namespace Course.Lesson02;

public static class Task10
{
    public static int Charge(int units)
    {
        if (units <= 10) return units * 2;
        if (units <= 20) return 20 + (units - 10) * 3;
        return 50 + (units - 20) * 5;
    }

    public static void Run()
    {
        Check.Equal(0, Charge(0));
        Check.Equal(20, Charge(10));
        Check.Equal(23, Charge(11));
        Check.Equal(26, Charge(12));
        Check.Equal(50, Charge(20));
        Check.Equal(55, Charge(21));
        Check.Equal(4_999_950, Charge(1_000_000));
    }
}

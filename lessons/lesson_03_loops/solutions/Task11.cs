using System;
using Course;

namespace Course.Lesson03;

public static class Task11
{
    public static int Quotient(int dividend, int divisor)
    {
        int count = 0;
        while (dividend >= divisor)
        {
            dividend -= divisor;
            count++;
        }
        return count;
    }
    public static int Wrong(int dividend, int divisor)
    {
        int count = 0;
        while (dividend > divisor)
        {
            dividend -= divisor;
            count++;
        }
        return count;
    }

    public static void Run()
    {
        Check.Equal(0, Quotient(0, 3));
        Check.Equal(0, Quotient(2, 3));
        Check.Equal(1, Quotient(3, 3));
        Check.Equal(4, Quotient(12, 3));
        Check.Equal(4, Quotient(14, 3));
        Check.Equal(10_000, Quotient(10_000, 1));
        Check.Equal(3, Wrong(12, 3));
        Check.True(Wrong(12, 3) != Quotient(12, 3));
    }
}

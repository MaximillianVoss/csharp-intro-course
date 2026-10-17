using System;
using Course;

namespace Course.Lesson04;

public static class Task09
{
    public static int DigitSum(int value)
    {
        static int Sum(long nonNegative)
        {
            if (nonNegative == 0) return 0;
            return (int)(nonNegative % 10) + Sum(nonNegative / 10);
        }
        long rest = value;
        if (rest < 0) rest = -rest;
        return Sum(rest);
    }

    public static void Run()
    {
        Check.Equal(0, DigitSum(0));
        Check.Equal(6, DigitSum(-1203));
        Check.Equal(1, DigitSum(1000));
        Check.Equal(47, DigitSum(int.MinValue));
        Check.Equal(46, DigitSum(int.MaxValue));
    }
}

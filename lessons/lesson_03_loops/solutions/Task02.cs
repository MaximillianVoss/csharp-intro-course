using System;
using Course;

namespace Course.Lesson03;

public static class Task02
{
    public static int DigitCount(int value)
    {
        long rest = value;
        if (rest < 0) rest = -rest;
        int count = 0;
        do
        {
            count++;
            rest /= 10;
        } while (rest != 0);
        return count;
    }

    public static void Run()
    {
        Check.Equal(1, DigitCount(0));
        Check.Equal(1, DigitCount(9));
        Check.Equal(2, DigitCount(10));
        Check.Equal(4, DigitCount(-1200));
        Check.Equal(10, DigitCount(int.MinValue));
        Check.Equal(10, DigitCount(int.MaxValue));
    }
}

using System;
using Course;

namespace Course.Lesson03;

public static class Task09
{
    public static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int remainder = a % b;
            a = b;
            b = remainder;
        }
        return a;
    }

    public static void Run()
    {
        Check.Equal(6, Gcd(48, 18));
        Check.Equal(1, Gcd(17, 13));
        Check.Equal(7, Gcd(0, 7));
        Check.Equal(7, Gcd(7, 0));
        Check.Equal(0, Gcd(0, 0));
        Check.Equal(int.MaxValue, Gcd(int.MaxValue, int.MaxValue));
    }
}

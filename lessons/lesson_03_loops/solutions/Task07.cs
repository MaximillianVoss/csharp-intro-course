using System;
using Course;

namespace Course.Lesson03;

public static class Task07
{
    public static bool Broken(int n)
    {
        if (n < 2) return false;
        for (int d = 2; d * d < n; d++)
            if (n % d == 0) return false;
        return true;
    }
    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int d = 2; d <= n / d; d++)
            if (n % d == 0) return false;
        return true;
    }

    public static void Run()
    {
        Check.True(Broken(49));
        Check.Equal(false, IsPrime(0));
        Check.Equal(false, IsPrime(1));
        Check.True(IsPrime(2));
        Check.Equal(false, IsPrime(4));
        Check.Equal(false, IsPrime(49));
        Check.True(IsPrime(97));
        Check.True(IsPrime(int.MaxValue));
    }
}

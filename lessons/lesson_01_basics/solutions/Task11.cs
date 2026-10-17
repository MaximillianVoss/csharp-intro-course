using System;
using Course;

namespace Course.Lesson01;

public static class Task11
{
    public static decimal RoundMoney(decimal amount)
    {
        return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }
    public static decimal Wrong(decimal amount)
    {
        return Math.Round(amount, 2);
    }

    public static void Run()
    {
        Check.Equal(1.23m, RoundMoney(1.234m));
        Check.Equal(2.35m, RoundMoney(2.345m));
        Check.Equal(-2.35m, RoundMoney(-2.345m));
        Check.Equal(12.30m, RoundMoney(12.30m));
        Check.Equal(0m, RoundMoney(0m));
        Check.Equal(2.34m, Wrong(2.345m));
        Check.True(Wrong(2.345m) != RoundMoney(2.345m));
    }
}

using System;
using Course;

namespace Course.Lesson04;

public static class Task04
{
    public static decimal Total(decimal unitPrice, int quantity = 1, decimal fee = 0m)
    {
        return unitPrice * quantity + fee;
    }

    public static void Run()
    {
        Check.Equal(10m, Total(10m));
        Check.Equal(20m, Total(10m, 2));
        Check.Equal(13m, Total(10m, fee: 3m));
        Check.Equal(23m, Total(fee: 3m, quantity: 2, unitPrice: 10m));
        Check.Equal(3m, Total(10m, 0, 3m));
    }
}

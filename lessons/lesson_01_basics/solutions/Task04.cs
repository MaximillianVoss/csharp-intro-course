using System;
using Course;

namespace Course.Lesson01;

public static class Task04
{
    public static decimal Discounted(decimal unitPrice, int quantity, decimal percent)
    {
        decimal subtotal = unitPrice * quantity;
        return subtotal * (1m - percent / 100m);
    }

    public static void Run()
    {
        Check.Equal(53.73m, Discounted(19.90m, 3, 10m));
        Check.Equal(59.70m, Discounted(19.90m, 3, 0m));
        Check.Equal(0m, Discounted(19.90m, 3, 100m));
        Check.Equal(0m, Discounted(999m, 0, 5m));
        Check.Equal(0.009m, Discounted(0.01m, 1, 10m));
    }
}

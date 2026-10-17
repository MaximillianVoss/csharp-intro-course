using System;
using Course;
using System.Globalization;

namespace Course.Lesson04;

public static class Task07
{
    public static decimal Subtotal(decimal price, int quantity) { return price * quantity; }
    public static decimal DiscountRate(int quantity) { return quantity >= 10 ? 0.10m : 0m; }
    public static string Receipt(decimal price, int quantity)
    {
        decimal subtotal = Subtotal(price, quantity);
        decimal discount = Math.Round(subtotal * DiscountRate(quantity), 2, MidpointRounding.AwayFromZero);
        decimal total = subtotal - discount;
        return "subtotal=" + subtotal.ToString("0.00", CultureInfo.InvariantCulture) +
               ";discount=" + discount.ToString("0.00", CultureInfo.InvariantCulture) +
               ";total=" + total.ToString("0.00", CultureInfo.InvariantCulture);
    }

    public static void Run()
    {
        Check.Equal(30m, Subtotal(10m, 3));
        Check.Equal(0m, DiscountRate(9));
        Check.Equal(0.10m, DiscountRate(10));
        Check.Equal("subtotal=90.00;discount=0.00;total=90.00", Receipt(10m, 9));
        Check.Equal("subtotal=100.00;discount=10.00;total=90.00", Receipt(10m, 10));
        Check.Equal("subtotal=0.00;discount=0.00;total=0.00", Receipt(10m, 0));
        Check.Equal("subtotal=0.15;discount=0.02;total=0.13", Receipt(0.01m, 15));
    }
}

using System;
using Course;

static int Clamp(int value, int minimum, int maximum)
{
    if (value < minimum) return minimum;
    if (value > maximum) return maximum;
    return value;
}

static bool TryDouble(int value, out int doubled)
{
    doubled = 0;
    long candidate = (long)value * 2;
    if (candidate < int.MinValue || candidate > int.MaxValue) return false;
    doubled = (int)candidate;
    return true;
}

static void Swap(ref int first, ref int second)
{
    int temporary = first;
    first = second;
    second = temporary;
}

static int SumDigits(long value)
{
    if (value == 0) return 0;
    return (int)(value % 10) + SumDigits(value / 10);
}

Check.Equal(10, Clamp(15, 0, 10));
Check.True(TryDouble(21, out int result));
Check.Equal(42, result);
Check.Equal(false, TryDouble(int.MaxValue, out result));
Check.Equal(0, result);
int x = 2;
int y = 9;
Swap(ref x, ref y);
Check.Equal(9, x);
Check.Equal(2, y);
Check.Equal(6, SumDigits(1203));
Console.WriteLine($"Обмен: {x}:{y}; сумма цифр: {SumDigits(1203)}");
Console.WriteLine("Пример 04 выполнен; return, out, ref и рекурсия проверены.");

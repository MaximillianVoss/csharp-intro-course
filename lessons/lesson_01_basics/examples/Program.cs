using System;
using System.Globalization;
using Course;

// TaskNN/Run and Check in the exercise files are supplied scaffolding.
// This example uses top-level statements: read each expression in order.
int total = 7;
int count = 2;
double lost = total / count;
double kept = (double)total / count;
Check.Equal(3.0, lost);
Check.Near(3.5, kept);
Console.WriteLine($"Целое деление: {lost}; вещественное: {kept.ToString(CultureInfo.InvariantCulture)}");

int x = 3;
int y = x + 4;
x = y * 2;
y = x - y;
Check.Equal(14, x);
Check.Equal(7, y);
Console.WriteLine($"Трасса: {x}:{y}");

decimal subtotal = 19.90m * 3;
decimal discounted = subtotal * (1m - 10m / 100m);
Check.Equal(53.73m, discounted);
Check.Equal(2.35m, Math.Round(2.345m, 2, MidpointRounding.AwayFromZero));
Check.Equal(2.34m, Math.Round(2.345m, 2));
Console.WriteLine("Сумма: " + discounted.ToString("0.00", CultureInfo.InvariantCulture));

int width = 50_000;
int height = 50_000;
long area = (long)width * height;
Check.Equal(2_500_000_000L, area);
int maximum = int.MaxValue;
Check.Equal(int.MinValue, unchecked(maximum + 1));
Check.Throws<OverflowException>(() => checked(maximum + 1).ToString());
Check.True(Math.Abs((0.1 + 0.2) - 0.3) <= 1e-12);
Console.WriteLine("Пример 01 выполнен; арифметика и переполнение проверены.");

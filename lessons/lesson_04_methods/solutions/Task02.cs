using System;
using Course;

namespace Course.Lesson04;

public static class Task02
{
    public static bool TryDivide(int dividend, int divisor, out int quotient)
    {
        quotient = 0;
        if (divisor == 0 || (dividend == int.MinValue && divisor == -1)) return false;
        quotient = dividend / divisor;
        return true;
    }

    public static void Run()
    {
        Check.True(TryDivide(7, 2, out int q));
        Check.Equal(3, q);
        Check.True(TryDivide(-7, 2, out q));
        Check.Equal(-3, q);
        Check.True(TryDivide(0, 1, out q));
        Check.Equal(0, q);
        Check.Equal(false, TryDivide(7, 0, out q));
        Check.Equal(0, q);
        Check.Equal(false, TryDivide(int.MinValue, -1, out q));
        Check.Equal(0, q);
    }
}

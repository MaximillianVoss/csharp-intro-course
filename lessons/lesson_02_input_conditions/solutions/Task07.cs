using System;
using Course;

namespace Course.Lesson02;

public static class Task07
{
    public static bool Broken(int value, int divisor)
    {
        return value % divisor == 0 && divisor != 0;
    }
    public static bool Divides(int value, int divisor)
    {
        return divisor != 0 && value % divisor == 0;
    }

    public static void Run()
    {
        Check.Throws<DivideByZeroException>(() => Broken(12, 0));
        Check.True(Divides(12, 3));
        Check.Equal(false, Divides(12, 5));
        Check.Equal(false, Divides(12, 0));
        Check.True(Divides(0, 7));
        Check.True(Divides(12, -3));
    }
}

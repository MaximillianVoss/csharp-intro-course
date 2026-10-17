using System;
using Course;

namespace Course.Lesson01;

public static class Task12
{
    public static int CheckedSum(int a, int b)
    {
        return checked(a + b);
    }
    public static int WrappedSum(int a, int b)
    {
        return unchecked(a + b);
    }

    public static void Run()
    {
        Check.Equal(42, CheckedSum(20, 22));
        Check.Equal(42, WrappedSum(20, 22));
        Check.Throws<OverflowException>(() => CheckedSum(int.MaxValue, 1));
        Check.Equal(int.MinValue, WrappedSum(int.MaxValue, 1));
        Check.Throws<OverflowException>(() => CheckedSum(int.MinValue, -1));
        Check.Equal(int.MaxValue, WrappedSum(int.MinValue, -1));
    }
}

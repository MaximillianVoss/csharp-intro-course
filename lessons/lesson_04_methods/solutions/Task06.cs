using System;
using Course;

namespace Course.Lesson04;

public static class Task06
{
    public static bool IsEven(int value)
    {
        return value % 2 == 0;
    }

    public static void Run()
    {
        Check.True(IsEven(-4));
        Check.Equal(false, IsEven(-3));
        Check.True(IsEven(0));
        Check.Equal(false, IsEven(7));
        Check.True(IsEven(int.MinValue));
    }
}

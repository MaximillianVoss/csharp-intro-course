using System;
using Course;

namespace Course.Lesson04;

public static class Task03
{
    public static void Exchange(int a, int b)
    {
        int temporary = a;
        a = b;
        b = temporary;
    }
    public static void Swap(ref int a, ref int b)
    {
        int temporary = a;
        a = b;
        b = temporary;
    }

    public static void Run()
    {
        int x = 2;
        int y = 9;
        Exchange(x, y);
        Check.Equal(2, x);
        Check.Equal(9, y);
        Swap(ref x, ref y);
        Check.Equal(9, x);
        Check.Equal(2, y);
        Swap(ref x, ref x);
        Check.Equal(9, x);
        x = int.MinValue;
        y = int.MaxValue;
        Swap(ref x, ref y);
        Check.Equal(int.MaxValue, x);
        Check.Equal(int.MinValue, y);
    }
}

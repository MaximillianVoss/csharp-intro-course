using System;
using Course;

namespace Course.Lesson03;

public static class Task12
{
    public static int PairCount(int n, int target)
    {
        int count = 0;
        for (int a = 1; a <= n; a++)
            for (int b = a + 1; b <= n; b++)
                if (a + b == target) count++;
        return count;
    }

    public static void Run()
    {
        Check.Equal(0, PairCount(0, 0));
        Check.Equal(0, PairCount(1, 2));
        Check.Equal(2, PairCount(5, 6));
        Check.Equal(1, PairCount(5, 9));
        Check.Equal(0, PairCount(5, 20));
        Check.Equal(50, PairCount(100, 101));
    }
}

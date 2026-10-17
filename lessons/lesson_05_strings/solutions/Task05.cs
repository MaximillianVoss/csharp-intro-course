using System;
using Course;

namespace Course.Lesson05;

public static class Task05
{
    public static int FirstIndex(string text, char target)
    {
        for (int index = 0; index < text.Length; index++)
            if (text[index] == target) return index;
        return -1;
    }

    public static void Run()
    {
        Check.Equal(1, FirstIndex("banana", 'a'));
        Check.Equal(-1, FirstIndex("banana", 'A'));
        Check.Equal(0, FirstIndex("abc", 'a'));
        Check.Equal(2, FirstIndex("abc", 'c'));
        Check.Equal(-1, FirstIndex("", 'a'));
    }
}

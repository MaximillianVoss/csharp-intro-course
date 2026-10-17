using System;
using Course;

namespace Course.Lesson05;

public static class Task09
{
    public static int CountPieces(string text, string needle)
    {
        if (needle.Length == 0) return 0;
        int start = 0;
        int count = 0;
        while (start <= text.Length - needle.Length)
        {
            int position = text.IndexOf(needle, start, StringComparison.Ordinal);
            if (position < 0) break;
            count++;
            start = position + needle.Length;
        }
        return count;
    }

    public static void Run()
    {
        Check.Equal(2, CountPieces("aaaaa", "aa"));
        Check.Equal(1, CountPieces("AaA", "a"));
        Check.Equal(0, CountPieces("a", "aa"));
        Check.Equal(0, CountPieces("abc", ""));
        Check.Equal(0, CountPieces("", "a"));
        Check.Equal(1, CountPieces("abc", "abc"));
    }
}

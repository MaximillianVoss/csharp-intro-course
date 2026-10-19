using System;
using Course;

namespace Course.Lesson05;

public static class Task06
{
    public static bool SameCommand(string? text, string expected)
    {
        return string.Equals(text, expected, StringComparison.OrdinalIgnoreCase);
    }

    public static void Run()
    {
        Check.True(SameCommand("StArT", "start"));
        Check.Equal(false, SameCommand(null, "start"));
        Check.Equal(false, SameCommand(" start ", "start"));
        Check.Equal(false, SameCommand("starter", "start"));
        Check.True(SameCommand("stop", "stop"));

        // Review 2026-10-19: ordinal Latin I contrasts
        Check.True(SameCommand("I", "i"));
        Check.Equal(false, SameCommand("ı", "i"));
        Check.Equal(false, SameCommand("İ", "i"));
    }
}

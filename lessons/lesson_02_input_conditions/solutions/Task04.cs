using System;
using Course;

namespace Course.Lesson02;

public static class Task04
{
    public static bool MayOpen(bool active, bool owner, bool hasPass, bool blocked)
    {
        return active && (owner || hasPass) && !blocked;
    }

    public static void Run()
    {
        Check.True(MayOpen(true, true, false, false));
        Check.True(MayOpen(true, false, true, false));
        Check.Equal(false, MayOpen(true, false, false, false));
        Check.Equal(false, MayOpen(false, false, true, false));
        Check.Equal(false, MayOpen(true, true, true, true));
    }
}

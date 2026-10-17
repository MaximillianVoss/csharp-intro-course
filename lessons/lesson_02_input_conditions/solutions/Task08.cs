using System;
using Course;

namespace Course.Lesson02;

public static class Task08
{
    public static string Status(bool paused, bool addressValid, bool paid)
    {
        if (paused) return "paused";
        if (!addressValid) return "address";
        if (!paid) return "payment";
        return "send";
    }

    public static void Run()
    {
        Check.Equal("paused", Status(true, false, false));
        Check.Equal("paused", Status(true, false, true));
        Check.Equal("paused", Status(true, true, false));
        Check.Equal("paused", Status(true, true, true));
        Check.Equal("address", Status(false, false, false));
        Check.Equal("address", Status(false, false, true));
        Check.Equal("payment", Status(false, true, false));
        Check.Equal("send", Status(false, true, true));
    }
}

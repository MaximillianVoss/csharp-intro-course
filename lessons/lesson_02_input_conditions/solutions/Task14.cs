using System;
using Course;

namespace Course.Lesson02;

public static class Task14
{
    public static string Command(bool running, string? command)
    {
        if (command is null) return "eof";
        switch (command)
        {
            case "start": return running ? "already-running" : "started";
            case "stop": return running ? "stopped" : "already-stopped";
            case "status": return running ? "running" : "stopped";
            default: return "invalid";
        }
    }

    public static void Run()
    {
        Check.Equal("started", Command(false, "start"));
        Check.Equal("already-running", Command(true, "start"));
        Check.Equal("stopped", Command(true, "stop"));
        Check.Equal("already-stopped", Command(false, "stop"));
        Check.Equal("running", Command(true, "status"));
        Check.Equal("stopped", Command(false, "status"));
        Check.Equal("eof", Command(false, null));
        Check.Equal("invalid", Command(false, ""));
        Check.Equal("invalid", Command(false, "Start"));
        Check.Equal("invalid", Command(false, " start "));
    }
}

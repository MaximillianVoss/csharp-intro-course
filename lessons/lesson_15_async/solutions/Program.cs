// Generated dispatcher; TaskNN.cs files are authored separately.
using Course.Lesson15;
return await Runner.RunAsync(args, new (string Id, Func<Task> Body)[]
{
    ("15-01", Task01.Run),
    ("15-02", Task02.Run),
    ("15-03", Task03.Run),
    ("15-04", Task04.Run),
    ("15-05", Task05.Run),
    ("15-06", Task06.Run),
    ("15-07", Task07.Run),
    ("15-08", Task08.Run),
    ("15-09", Task09.Run),
    ("15-10", Task10.Run),
    ("15-11", Task11.Run),
    ("15-12", Task12.Run),
    ("15-13", Task13.Run),
    ("15-14", Task14.Run),
    ("15-15", Task15.Run)
}, template: false);

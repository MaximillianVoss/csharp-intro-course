namespace Course.Lesson08;

public static class Task12
{
    public sealed class TimeSlot
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        private TimeSlot(DateTime start, DateTime end) { Start = start; End = end; }
        public static TimeSlot? Create(DateTime start, DateTime end) => start < end ? new TimeSlot(start, end) : null;
        public bool Overlaps(TimeSlot other) => Start < other.End && other.Start < End;
    }
    public static void Run()
    {
        DateTime day = new(2030, 1, 1);
        TimeSlot a = TimeSlot.Create(day.AddHours(10), day.AddHours(11))!;
        TimeSlot touching = TimeSlot.Create(day.AddHours(11), day.AddHours(12))!;
        TimeSlot cross = TimeSlot.Create(day.AddHours(10.5), day.AddHours(11.5))!;
        Check.True(!a.Overlaps(touching)); Check.True(a.Overlaps(cross) && cross.Overlaps(a));
        Check.True(a.Overlaps(TimeSlot.Create(day.AddHours(10.2), day.AddHours(10.4))!));
        Check.True(TimeSlot.Create(day, day) is null); Check.True(TimeSlot.Create(day.AddHours(1), day) is null);
    }
}

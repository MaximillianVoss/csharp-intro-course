namespace Course.Lesson08;

public static class Task09
{
    public sealed class BrokenMeter
    {
        private static int value;
        public int Value => value;
        public void Tick() { value++; }
    }
    public sealed class Meter
    {
        private int value;
        public int Value => value;
        public void Tick() { value++; }
    }
    public static void Run()
    {
        BrokenMeter a = new(); BrokenMeter b = new(); int baseline = a.Value;
        a.Tick(); b.Tick(); Check.Equal(baseline + 2, a.Value); Check.Equal(a.Value, b.Value);
        Meter first = new(); Meter second = new(); first.Tick(); second.Tick();
        Check.Equal(1, first.Value); Check.Equal(1, second.Value);
        first.Tick(); Check.Equal(2, first.Value); Check.Equal(1, second.Value);
    }
}

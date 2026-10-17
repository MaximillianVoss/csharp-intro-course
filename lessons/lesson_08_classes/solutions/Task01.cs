namespace Course.Lesson08;

public static class Task01
{
    public sealed class BoundedCounter
    {
        public int Maximum { get; }
        public int Value { get; private set; }
        public BoundedCounter(int maximum) { Maximum = maximum; }
        public bool TryIncrement()
        {
            if (Value == Maximum) return false;
            Value++; return true;
        }
    }
    public static void Run()
    {
        BoundedCounter counter = new(2);
        Check.True(counter.TryIncrement()); Check.True(counter.TryIncrement());
        Check.True(!counter.TryIncrement()); Check.Equal(2, counter.Value);
        BoundedCounter zero = new(0); Check.True(!zero.TryIncrement()); Check.Equal(0, zero.Value);
        BoundedCounter large = new(int.MaxValue); Check.Equal(int.MaxValue, large.Maximum); Check.True(large.TryIncrement());
    }
}

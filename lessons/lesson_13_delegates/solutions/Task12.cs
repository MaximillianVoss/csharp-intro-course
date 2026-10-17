using Course;

namespace Course.Lesson13;

public static class Task12
{
    public sealed class Counter
    {
        private readonly int threshold;
        public Counter(int threshold) { if (threshold <= 0) throw new ArgumentOutOfRangeException(nameof(threshold)); this.threshold = threshold; }
        public int Value { get; private set; }
        public event Action<int>? Reached;
        public void Add(int delta)
        {
            int old = Value, next = checked(old + delta); if (next < 0) throw new ArgumentOutOfRangeException(nameof(delta));
            Value = next; if (old < threshold && next >= threshold) Reached?.Invoke(next);
        }
    }
    public static void Run()
    {
        var counter = new Counter(5); var seen = new List<int>(); Action<int> listener = v => { Check.Equal(v, counter.Value); seen.Add(v); }; counter.Reached += listener;
        counter.Add(5); counter.Add(1); counter.Add(-4); counter.Add(3); Check.SequenceEqual(new[] { 5, 5 }, seen);
        counter.Reached -= listener; counter.Add(-3); counter.Add(3); Check.Equal(2, seen.Count);
        Check.Throws<ArgumentOutOfRangeException>(() => counter.Add(-6)); Check.Equal(5, counter.Value);
        Check.Throws<OverflowException>(() => counter.Add(int.MaxValue)); Check.Equal(5, counter.Value);
        var broken = new Counter(1); broken.Reached += _ => throw new FormatException(); Check.Throws<FormatException>(() => broken.Add(1)); Check.Equal(1, broken.Value);
        Check.Throws<ArgumentOutOfRangeException>(() => new Counter(0));
    }
}

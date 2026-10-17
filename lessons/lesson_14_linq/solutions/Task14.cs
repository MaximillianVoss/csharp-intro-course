using Course;

namespace Course.Lesson14;

public static class Task14
{
    public static (int Count, long Sum) Analyze(IEnumerable<int> source)
    {
        ArgumentNullException.ThrowIfNull(source); var snapshot = source.ToArray(); return (snapshot.Length, snapshot.Sum(x => (long)x));
    }
    private sealed class OnceSequence : IEnumerable<int>
    {
        private readonly int[] values; private bool used;
        public OnceSequence(int[] values) { this.values = values; }
        public IEnumerator<int> GetEnumerator()
        {
            if (used) throw new InvalidOperationException("Second enumeration."); used = true; return ((IEnumerable<int>)values).GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
    private static IEnumerable<int> Once(int[] values) => new OnceSequence(values);
    private static (int, long) Broken(IEnumerable<int> source) => (source.Count(), source.Sum(x => (long)x));
    public static void Run()
    {
        Check.Equal((2, 4294967294L), Analyze(Once(new[] { int.MaxValue, int.MaxValue })));
        Check.Throws<InvalidOperationException>(() => Broken(Once(new[] { 1, 2 })));
        Check.Equal((0, 0L), Analyze(Once(Array.Empty<int>()))); Check.Throws<ArgumentNullException>(() => Analyze(null!));
    }
}

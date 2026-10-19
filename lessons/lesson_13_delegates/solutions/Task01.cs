using Course;

namespace Course.Lesson13;

public static class Task01
{
    public static int Apply(int value, Func<int, int> operation) { ArgumentNullException.ThrowIfNull(operation); return operation(value); }
    public static void Run()
    {
        Check.Equal(7, Apply(4, x => x + 3)); Check.Equal(16, Apply(4, x => x * x));
        int calls = 0; Check.Equal(5, Apply(4, x => { calls++; return x + 1; })); Check.Equal(1, calls);
        Check.Throws<FormatException>(() => Apply(0, _ => throw new FormatException("bad")));
        Check.Throws<ArgumentNullException>(() => Apply(0, null!));
        var failure = new FormatException("same instance"); int faultCalls = 0;
        var observed = Check.Throws<FormatException>(() => Apply(1, _ => { faultCalls++; throw failure; })); Check.True(ReferenceEquals(failure, observed)); Check.Equal(1, faultCalls);
    }
}

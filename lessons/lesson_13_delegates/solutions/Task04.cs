using Course;

namespace Course.Lesson13;

public static class Task04
{
    public static Func<int>[] Build(int count)
    {
        if (count < 0 || count > 1000) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new Func<int>[count]; for (int i = 0; i < count; i++) { int copy = i; result[i] = () => copy; } return result;
    }
    private static Func<int>[] Broken(int count)
    {
        var result = new Func<int>[count]; for (int i = 0; i < count; i++) result[i] = () => i; return result;
    }
    public static void Run()
    {
        var broken = Broken(3); Check.SequenceEqual(new[] { 3, 3, 3 }, new[] { broken[0](), broken[1](), broken[2]() });
        var fixedValues = Build(3); Check.SequenceEqual(new[] { 0, 1, 2 }, new[] { fixedValues[0](), fixedValues[1](), fixedValues[2]() });
        Check.Equal(0, Build(0).Length); Check.Equal(0, Build(1)[0]());
        Check.Throws<ArgumentOutOfRangeException>(() => Build(-1)); Check.Throws<ArgumentOutOfRangeException>(() => Build(1001));
    }
}

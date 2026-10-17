using Course;

namespace Course.Lesson13;

public static class Task03
{
    public static int[] Observe()
    {
        int factor = 2; Func<int, int> multiply = x => x * factor; int a = multiply(3); factor = 5; int b = multiply(3); return new[] { a, b };
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 6, 15 }, Observe()); Check.SequenceEqual(new[] { 6, 15 }, Observe());
        int factor = 2; int snapshot = factor; Func<int, int> fixedMultiply = x => x * snapshot; factor = 5;
        Check.Equal(6, fixedMultiply(3)); Check.Equal(5, factor);
    }
}

using Course;

namespace Course.Lesson12;

public static class Task03
{
    public sealed class Box<T> { public static int Count; }
    public static (int IntCount, int StringCount) Observe()
    {
        Box<int>.Count = 0; Box<string>.Count = 0;
        Box<int>.Count++; Box<string>.Count++; Box<int>.Count++;
        return (Box<int>.Count, Box<string>.Count);
    }
    public static void Run()
    {
        Check.Equal((2, 1), Observe()); Check.Equal((2, 1), Observe());
        _ = new Box<int>(); Check.Equal(2, Box<int>.Count); Check.Equal(1, Box<string>.Count);
    }
}

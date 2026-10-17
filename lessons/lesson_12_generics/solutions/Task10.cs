using Course;

namespace Course.Lesson12;

public static class Task10
{
    public sealed class Box { public int Value { get; set; } }
    public static T[] Copy<T>(T[] source)
    {
        ArgumentNullException.ThrowIfNull(source); var copy = new T[source.Length]; Array.Copy(source, copy, source.Length); return copy;
    }
    private static T[] Broken<T>(T[] source) => source;
    public static void Run()
    {
        var input = new[] { 1, 2 }; var copy = Copy(input); Check.SequenceEqual(input, copy); copy[0] = 8; Check.Equal(1, input[0]);
        Check.True(ReferenceEquals(input, Broken(input))); Check.True(!ReferenceEquals(input, Copy(input)));
        Check.Equal(0, Copy(Array.Empty<int>()).Length);
        var boxes = new[] { new Box { Value = 1 } }; var other = Copy(boxes); other[0].Value = 2; Check.Equal(2, boxes[0].Value);
        other[0] = new Box { Value = 9 }; Check.Equal(2, boxes[0].Value);
        Check.Throws<ArgumentNullException>(() => Copy<int>(null!));
    }
}

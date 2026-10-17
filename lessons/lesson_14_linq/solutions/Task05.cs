using Course;

namespace Course.Lesson14;

public static class Task05
{
    public static int? FirstEven(IEnumerable<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers); return numbers.Where(x => x % 2 == 0).Select(x => (int?)x).FirstOrDefault();
    }
    private static IEnumerable<int> DangerousTail() { yield return 0; throw new InvalidOperationException("Must not enumerate tail."); }
    public static void Run()
    {
        Check.Equal<int?>(0, FirstEven(new[] { 1, 0, 2 })); Check.Equal<int?>(null, FirstEven(new[] { 1, 3 })); Check.Equal<int?>(null, FirstEven(Array.Empty<int>()));
        Check.Equal<int?>(-2, FirstEven(new[] { -3, -2, 4 })); Check.Equal<int?>(0, FirstEven(DangerousTail())); Check.Throws<ArgumentNullException>(() => FirstEven(null!));
    }
}

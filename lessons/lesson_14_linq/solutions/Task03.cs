using Course;

namespace Course.Lesson14;

public static class Task03
{
    public static (int[] Query, int[] Snapshot) Observe()
    {
        var numbers = new List<int> { 1, 2, 3 }; var query = numbers.Where(x => x > 1); var snapshot = query.ToArray(); numbers.Add(4);
        return (query.ToArray(), snapshot);
    }
    public static void Run()
    {
        var pair = Observe(); Check.SequenceEqual(new[] { 2, 3, 4 }, pair.Query); Check.SequenceEqual(new[] { 2, 3 }, pair.Snapshot);
        var again = Observe(); Check.SequenceEqual(pair.Query, again.Query); Check.SequenceEqual(pair.Snapshot, again.Snapshot);
    }
}

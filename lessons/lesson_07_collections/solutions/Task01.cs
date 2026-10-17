namespace Course.Lesson07;

public static class Task01
{
    public static List<int> InsertCopy(List<int> source, int index, int value)
    {
        List<int> result = new(source);
        result.Insert(index, value);
        return result;
    }
    public static void Run()
    {
        List<int> source = new() { 10, 30 };
        List<int> result = InsertCopy(source, 1, 20);
        Check.SequenceEqual(new[] { 10, 20, 30 }, result);
        Check.SequenceEqual(new[] { 7 }, InsertCopy(new List<int>(), 0, 7));
        Check.SequenceEqual(new[] { 10, 30, 40 }, InsertCopy(source, 2, 40));
        result[0] = 99; Check.SequenceEqual(new[] { 10, 30 }, source);
    }
}

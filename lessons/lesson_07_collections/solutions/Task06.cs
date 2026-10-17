namespace Course.Lesson07;

public static class Task06
{
    public static void RemoveNegatives(List<int> values)
    {
        for (int i = values.Count - 1; i >= 0; i--) if (values[i] < 0) values.RemoveAt(i);
    }
    public static void Run()
    {
        List<int> wrong = new() { -1, -2, 3 };
        for (int i = 0; i < wrong.Count; i++) if (wrong[i] < 0) wrong.RemoveAt(i);
        Check.SequenceEqual(new[] { -2, 3 }, wrong);
        List<int> values = new() { -1, -2, 3, 0, -4 }; RemoveNegatives(values);
        Check.SequenceEqual(new[] { 3, 0 }, values);
        List<int> all = new() { -1, -2 }; RemoveNegatives(all); Check.Equal(0, all.Count);
        List<int> empty = new(); RemoveNegatives(empty); Check.Equal(0, empty.Count);
    }
}

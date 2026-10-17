namespace Course.Lesson07;

public static class Task09
{
    public static List<int> CompletionOrder(int[] durations, int quantum)
    {
        int[] remaining = (int[])durations.Clone(); Queue<int> queue = new(); List<int> result = new();
        for (int i = 0; i < remaining.Length; i++) queue.Enqueue(i);
        while (queue.Count > 0)
        {
            int id = queue.Dequeue(); remaining[id] -= Math.Min(quantum, remaining[id]);
            if (remaining[id] == 0) result.Add(id); else queue.Enqueue(id);
        }
        return result;
    }
    public static void Run()
    {
        int[] durations = { 5, 2, 3 };
        Check.SequenceEqual(new[] { 1, 2, 0 }, CompletionOrder(durations, 2));
        Check.SequenceEqual(new[] { 0, 1, 2 }, CompletionOrder(durations, 10));
        Check.SequenceEqual(new[] { 0 }, CompletionOrder(new[] { 9 }, 1));
        Check.SequenceEqual(Array.Empty<int>(), CompletionOrder(Array.Empty<int>(), 1));
        Check.SequenceEqual(new[] { 5, 2, 3 }, durations);
    }
}

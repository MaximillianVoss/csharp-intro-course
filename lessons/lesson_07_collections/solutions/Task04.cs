namespace Course.Lesson07;

public static class Task04
{
    public static int[] ReplayQueue()
    {
        Queue<int> queue = new();
        queue.Enqueue(10); queue.Enqueue(20);
        int first = queue.Dequeue(); queue.Enqueue(30);
        int next = queue.Peek(); int count = queue.Count;
        int second = queue.Dequeue();
        Check.Equal(30, queue.Peek()); Check.Equal(1, queue.Count);
        return new[] { first, next, count, second };
    }
    public static void Run() => Check.SequenceEqual(new[] { 10, 20, 2, 20 }, ReplayQueue());
}

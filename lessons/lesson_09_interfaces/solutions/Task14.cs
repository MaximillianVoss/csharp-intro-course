namespace Course.Lesson09;

public static class Task14
{
    public interface IIntQueue { void Add(int value); bool TryTake(out int value); }
    public sealed class FifoQueue : IIntQueue
    {
        private readonly Queue<int> data = new(); public void Add(int value) { data.Enqueue(value); }
        public bool TryTake(out int value) => data.TryDequeue(out value);
    }
    public sealed class LifoQueue : IIntQueue
    {
        private readonly Stack<int> data = new(); public void Add(int value) { data.Push(value); }
        public bool TryTake(out int value) => data.TryPop(out value);
    }
    public static bool SatisfiesContract(IIntQueue queue)
    {
        if (queue.TryTake(out int empty) || empty != 0) return false;
        queue.Add(1); queue.Add(2);
        if (!queue.TryTake(out int first) || first != 1) return false;
        queue.Add(3);
        if (!queue.TryTake(out int second) || second != 2) return false;
        if (!queue.TryTake(out int third) || third != 3) return false;
        return !queue.TryTake(out int last) && last == 0;
    }
    public static void Run()
    {
        Check.True(SatisfiesContract(new FifoQueue())); Check.True(!SatisfiesContract(new LifoQueue()));
        IIntQueue queue = new FifoQueue(); Check.True(!queue.TryTake(out int absent)); Check.Equal(0, absent);
        queue.Add(0); Check.True(queue.TryTake(out int present)); Check.Equal(0, present);
    }
}

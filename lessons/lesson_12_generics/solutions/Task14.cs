using Course;

namespace Course.Lesson12;

public static class Task14
{
    public sealed class RingBuffer<T>
    {
        private readonly T[] data; private int next;
        public RingBuffer(int capacity) { if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity)); data = new T[capacity]; }
        public int Count { get; private set; }
        public void Add(T value) { data[next] = value; next = (next + 1) % data.Length; if (Count < data.Length) Count++; }
        public T[] Snapshot()
        {
            var result = new T[Count]; int start = Count == data.Length ? next : 0;
            for (int i = 0; i < Count; i++) result[i] = data[(start + i) % data.Length]; return result;
        }
    }
    public static void Run()
    {
        var ring = new RingBuffer<int>(3); Check.Equal(0, ring.Count); ring.Add(1); ring.Add(2); Check.SequenceEqual(new[] { 1, 2 }, ring.Snapshot());
        ring.Add(3); ring.Add(4); Check.SequenceEqual(new[] { 2, 3, 4 }, ring.Snapshot());
        ring.Add(5); ring.Add(6); Check.SequenceEqual(new[] { 4, 5, 6 }, ring.Snapshot()); Check.Equal(3, ring.Count);
        var snap = ring.Snapshot(); snap[0] = 0; Check.Equal(4, ring.Snapshot()[0]);
        var single = new RingBuffer<string?>(1); single.Add("x"); single.Add(null); Check.Equal<string?>(null, single.Snapshot()[0]);
        Check.Throws<ArgumentOutOfRangeException>(() => new RingBuffer<int>(0));
    }
}

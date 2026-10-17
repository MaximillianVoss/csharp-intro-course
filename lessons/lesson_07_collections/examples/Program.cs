using Course;
List<string> ordered = new() { "alpha", "beta" }; ordered.Insert(1, "middle");
Check.SequenceEqual(new[] { "alpha", "middle", "beta" }, ordered);
HashSet<string> seen = new(StringComparer.OrdinalIgnoreCase);
Check.True(seen.Add("abc")); Check.True(!seen.Add("ABC"));
Dictionary<string, int> counts = new(StringComparer.Ordinal);
foreach (string key in new[] { "a", "b", "a" })
{
    counts.TryGetValue(key, out int count); counts[key] = count + 1;
}
Check.Equal(2, counts["a"]);
Queue<int> queue = new(); queue.Enqueue(1); queue.Enqueue(2); Check.Equal(1, queue.Dequeue());
Stack<int> stack = new(); stack.Push(1); stack.Push(2); Check.Equal(2, stack.Pop());
Console.WriteLine("List хранит порядок; HashSet — членство; Dictionary — значение; Queue — FIFO; Stack — LIFO.");

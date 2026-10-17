using Course;

var history = new History<string>(2);
history.Add("создано"); history.Add("проверено"); history.Add("сохранено");
Check.SequenceEqual(new[] { "проверено", "сохранено" }, history.Snapshot());
Console.WriteLine(string.Join(" → ", history.Snapshot()));
int a = 1, b = 2; Swap(ref a, ref b); Check.Equal(2, a); Check.Equal(1, b);
Console.WriteLine($"После Swap: {a}, {b}");

static void Swap<T>(ref T a, ref T b) { T value = a; a = b; b = value; }

public sealed class History<T>
{
    private readonly Queue<T> values = new();
    private readonly int capacity;
    public History(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        this.capacity = capacity;
    }
    public void Add(T value) { values.Enqueue(value); if (values.Count > capacity) values.Dequeue(); }
    public T[] Snapshot() => values.ToArray();
}

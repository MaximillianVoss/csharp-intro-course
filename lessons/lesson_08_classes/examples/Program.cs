using Course;
Counter first = new(2); Counter alias = first;
Check.True(alias.TryIncrement()); Check.Equal(1, first.Value);
Counter independent = new(2); Check.Equal(0, independent.Value);
Product original = new("A", 10m); Product changed = original with { Price = 12m };
Check.Equal(10m, original.Price); Check.Equal(12m, changed.Price);
Check.Equal(new Product("A", 10m), original);
Console.WriteLine($"Общий объект: {first.Value}; независимый: {independent.Value}; запись: {changed}.");
sealed class Counter
{
    public int Maximum { get; }
    public int Value { get; private set; }
    public Counter(int maximum) { Maximum = maximum; }
    public bool TryIncrement() { if (Value == Maximum) return false; Value++; return true; }
}
sealed record Product(string Code, decimal Price);

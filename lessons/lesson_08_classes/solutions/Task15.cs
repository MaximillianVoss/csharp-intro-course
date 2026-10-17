namespace Course.Lesson08;

public static class Task15
{
    public sealed class Bin
    {
        public int Capacity { get; }
        public int Quantity { get; private set; }
        public Bin(int capacity, int quantity) { Capacity = capacity; Quantity = quantity; }
        public static bool TryTransfer(Bin source, Bin target, int amount)
        {
            if (amount < 0 || amount > source.Quantity) return false;
            if (ReferenceEquals(source, target) || amount == 0) return true;
            if (amount > target.Capacity - target.Quantity) return false;
            source.Quantity -= amount; target.Quantity += amount; return true;
        }
    }
    public static void Run()
    {
        Bin source = new(10, 7); Bin target = new(5, 2);
        Check.True(Bin.TryTransfer(source, target, 3)); Check.Equal(4, source.Quantity); Check.Equal(5, target.Quantity);
        Check.Equal(9, source.Quantity + target.Quantity);
        Check.True(!Bin.TryTransfer(source, target, 1)); Check.Equal(4, source.Quantity); Check.Equal(5, target.Quantity);
        Check.True(!Bin.TryTransfer(source, target, 5)); Check.True(!Bin.TryTransfer(source, target, -1));
        Check.True(Bin.TryTransfer(source, target, 0)); Check.True(Bin.TryTransfer(source, source, 4)); Check.Equal(4, source.Quantity);
        Check.True(!Bin.TryTransfer(source, source, 5));
    }
}

namespace Course.Lesson08;

public static class Task05
{
    public sealed class Tank
    {
        public int Capacity { get; }
        public int Volume { get; private set; }
        public Tank(int capacity) { Capacity = capacity; }
        public bool TryFill(int amount)
        {
            if (amount < 0 || amount > Capacity - Volume) return false;
            Volume += amount; return true;
        }
    }
    private static bool WrongCanFill(int capacity, int current, int amount) => amount >= 0 && amount < capacity - current;
    public static void Run()
    {
        Tank tank = new(10); Check.True(tank.TryFill(10)); Check.Equal(10, tank.Volume);
        Check.True(!tank.TryFill(1)); Check.Equal(10, tank.Volume);
        Check.True(tank.TryFill(0)); Check.True(!tank.TryFill(-1));
        Tank empty = new(10); Check.True(!empty.TryFill(int.MaxValue)); Check.Equal(0, empty.Volume);
        bool wrongExactFill = WrongCanFill(empty.Capacity, empty.Volume, 10);
        Check.True(!wrongExactFill); Check.True(empty.TryFill(10));
        Tank partial = new(10); Check.True(partial.TryFill(3)); Check.True(partial.TryFill(7)); Check.Equal(10, partial.Volume);
    }
}

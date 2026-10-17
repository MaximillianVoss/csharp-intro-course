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
    public static void Run()
    {
        Tank tank = new(10); Check.True(tank.TryFill(10)); Check.Equal(10, tank.Volume);
        Check.True(!tank.TryFill(1)); Check.Equal(10, tank.Volume);
        Check.True(tank.TryFill(0)); Check.True(!tank.TryFill(-1));
        Tank empty = new(10); Check.True(!empty.TryFill(int.MaxValue)); Check.Equal(0, empty.Volume);
        bool wrongExactFill = 10 >= 0 && 10 < empty.Capacity - empty.Volume;
        Check.True(!wrongExactFill); Check.True(empty.TryFill(10));
    }
}

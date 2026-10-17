namespace Course.Lesson09;

public static class Task05
{
    public interface IClock { DateTime Now { get; } }
    public sealed class FixedClock : IClock
    {
        public DateTime Now { get; }
        public FixedClock(DateTime now) { Now = now; }
    }
    public sealed class Greeting
    {
        private readonly IClock clock;
        public Greeting(IClock clock) { this.clock = clock; }
        public string Get() => clock.Now.Hour < 12 ? "утро" : "день";
    }
    public static void Run()
    {
        Check.Equal("утро", new Greeting(new FixedClock(new DateTime(2030, 1, 1, 0, 0, 0))).Get());
        Check.Equal("утро", new Greeting(new FixedClock(new DateTime(2030, 1, 1, 11, 59, 0))).Get());
        FixedClock noon = new(new DateTime(2030, 1, 1, 12, 0, 0));
        Check.Equal("день", new Greeting(noon).Get());
        Check.Equal("день", new Greeting(new FixedClock(new DateTime(2030, 1, 1, 23, 59, 0))).Get());
        string wrong = noon.Now.Hour <= 12 ? "утро" : "день";
        Check.True(wrong != new Greeting(noon).Get());
    }
}

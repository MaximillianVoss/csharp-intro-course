using Course;
IClock fixedClock = new FixedClock(new DateTime(2030, 1, 1, 12, 0, 0));
Greeting greeting = new(fixedClock);
Check.Equal("день", greeting.Get());
IText source = new Literal("  C#  "); IText decorated = new Trimmed(source);
Check.Equal("C#", decorated.Read()); Check.Equal("  C#  ", source.Read());
Console.WriteLine($"Внедрённое время: {greeting.Get()}; композиция: '{decorated.Read()}'.");
interface IClock { DateTime Now { get; } }
sealed class FixedClock : IClock
{
    public DateTime Now { get; } public FixedClock(DateTime now) { Now = now; }
}
sealed class Greeting
{
    private readonly IClock clock; public Greeting(IClock clock) { this.clock = clock; }
    public string Get() => clock.Now.Hour < 12 ? "утро" : "день";
}
interface IText { string Read(); }
sealed class Literal : IText
{
    private readonly string value; public Literal(string value) { this.value = value; }
    public string Read() => value;
}
sealed class Trimmed : IText
{
    private readonly IText inner; public Trimmed(IText inner) { this.inner = inner; }
    public string Read() => inner.Read().Trim();
}

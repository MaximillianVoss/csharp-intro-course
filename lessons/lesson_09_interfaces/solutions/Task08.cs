namespace Course.Lesson09;

public static class Task08
{
    public interface IReadName { string Read(); }
    public interface IWriteName { void Write(string value); }
    public sealed class MemoryName : IReadName, IWriteName
    {
        private string value; public MemoryName(string initial) { value = initial; }
        public string Read() => value; public void Write(string value) { this.value = value; }
    }
    public sealed class ReadOnlyName : IReadName
    {
        private readonly string value; public ReadOnlyName(string value) { this.value = value; }
        public string Read() => value;
    }
    public static string BuildHeading(IReadName source) => "Имя: " + source.Read();
    public static void Run()
    {
        MemoryName storage = new("A"); Check.Equal("Имя: A", BuildHeading(storage));
        IWriteName writer = storage; writer.Write("B"); Check.Equal("Имя: B", BuildHeading(storage));
        Check.Equal("Имя: C", BuildHeading(new ReadOnlyName("C"))); Check.Equal("Имя: ", BuildHeading(new ReadOnlyName("")));
    }
}

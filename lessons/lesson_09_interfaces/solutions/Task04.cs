namespace Course.Lesson09;

public static class Task04
{
    public interface ICommand { string Execute(); }
    public sealed class EchoCommand : ICommand
    {
        private readonly string text; public EchoCommand(string text) { this.text = text; }
        public string Execute() => text;
    }
    public sealed class SumCommand : ICommand
    {
        private readonly int left, right;
        public SumCommand(int left, int right) { this.left = left; this.right = right; }
        public string Execute() => (left + right).ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
    private sealed class CountingCommand : ICommand
    {
        public int Calls { get; private set; }
        public string Execute() { Calls++; return "counted"; }
    }
    public static string[] ExecuteAll(ICommand[] commands)
    {
        string[] result = new string[commands.Length];
        for (int i = 0; i < commands.Length; i++) result[i] = commands[i].Execute();
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { "готово", "5" }, ExecuteAll(new ICommand[] { new EchoCommand("готово"), new SumCommand(2, 3) }));
        Check.SequenceEqual(new[] { "-3" }, ExecuteAll(new ICommand[] { new SumCommand(-5, 2) }));
        Check.Equal(0, ExecuteAll(Array.Empty<ICommand>()).Length);
        CountingCommand command = new(); ExecuteAll(new ICommand[] { command }); Check.Equal(1, command.Calls);
        System.Globalization.CultureInfo previous = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo unusual = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.InvariantCulture.Clone();
            unusual.NumberFormat.NegativeSign = "minus"; System.Globalization.CultureInfo.CurrentCulture = unusual;
            Check.Equal("-3", new SumCommand(-5, 2).Execute());
        }
        finally { System.Globalization.CultureInfo.CurrentCulture = previous; }
    }
}

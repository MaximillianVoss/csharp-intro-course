namespace Course.Lesson09;

public static class Task15
{
    public interface IRecipients { string[] Get(); }
    public interface IMessageTemplate { string Build(string recipient); }
    public interface IChannel { bool TrySend(string recipient, string message); }
    public readonly record struct DeliverySummary(int Attempted, int Succeeded);
    public sealed class NotificationService
    {
        private readonly IRecipients recipients; private readonly IMessageTemplate template; private readonly IChannel channel;
        public NotificationService(IRecipients recipients, IMessageTemplate template, IChannel channel)
        { this.recipients = recipients; this.template = template; this.channel = channel; }
        public DeliverySummary SendAll()
        {
            string[] targets = recipients.Get(); int succeeded = 0;
            foreach (string target in targets) if (channel.TrySend(target, template.Build(target))) succeeded++;
            return new DeliverySummary(targets.Length, succeeded);
        }
    }
    private sealed class FixedRecipients : IRecipients
    {
        private readonly string[] values; public int Calls { get; private set; }
        public FixedRecipients(string[] values) { this.values = values; }
        public string[] Get() { Calls++; return (string[])values.Clone(); }
    }
    private sealed class GreetingTemplate : IMessageTemplate { public string Build(string recipient) => "Привет, " + recipient; }
    private sealed class RecordingChannel : IChannel
    {
        public List<string> Attempts { get; } = new();
        public bool TrySend(string recipient, string message) { Attempts.Add(recipient + "|" + message); return recipient != "b"; }
    }
    public static void Run()
    {
        FixedRecipients recipients = new(new[] { "a", "b", "a" }); RecordingChannel channel = new();
        DeliverySummary result = new NotificationService(recipients, new GreetingTemplate(), channel).SendAll();
        Check.Equal(new DeliverySummary(3, 2), result); Check.Equal(1, recipients.Calls);
        Check.SequenceEqual(new[] { "a|Привет, a", "b|Привет, b", "a|Привет, a" }, channel.Attempts);
        Check.Equal(new DeliverySummary(0, 0), new NotificationService(new FixedRecipients(Array.Empty<string>()), new GreetingTemplate(), new RecordingChannel()).SendAll());
    }
}

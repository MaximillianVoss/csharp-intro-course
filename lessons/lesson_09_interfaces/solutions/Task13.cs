namespace Course.Lesson09;

public static class Task13
{
    public interface ICompactReport { string Render(); }
    public interface IDetailedReport { string Render(); }
    public sealed class Report : ICompactReport, IDetailedReport
    {
        private readonly string title; private readonly int count;
        public Report(string title, int count) { this.title = title; this.count = count; }
        string ICompactReport.Render() => title;
        string IDetailedReport.Render() => title + ": " + count.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string[] BothViews(Report report)
    {
        ICompactReport compact = report; IDetailedReport detailed = report;
        return new[] { compact.Render(), detailed.Render() };
    }
    public static void Run()
    {
        Report report = new("Задачи", 3); Check.SequenceEqual(new[] { "Задачи", "Задачи: 3" }, BothViews(report));
        Check.SequenceEqual(new[] { "", ": 0" }, BothViews(new Report("", 0)));
        ICompactReport compact = report; IDetailedReport detailed = report; Check.True(ReferenceEquals(compact, detailed));
    }
}

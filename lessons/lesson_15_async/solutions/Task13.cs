using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task13
{
    public static async IAsyncEnumerable<string> NonBlankLinesAsync(TextReader reader, [EnumeratorCancellation] CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(reader);
        while (true) { token.ThrowIfCancellationRequested(); var line = await reader.ReadLineAsync(token); if (line is null) yield break; if (!string.IsNullOrWhiteSpace(line)) yield return line; }
    }
    public static async Task Run()
    {
        using var reader = new StringReader("\n a \n \nb\n"); var result = new List<string>(); await foreach (var line in NonBlankLinesAsync(reader)) result.Add(line); Check.SequenceEqual(new[] { " a ", "b" }, result); Check.Equal("", reader.ReadToEnd());
        using var cts = new CancellationTokenSource(); using var remaining = new StringReader("first\nsecond");
        await using (var iterator = NonBlankLinesAsync(remaining, cts.Token).GetAsyncEnumerator()) { Check.True(await iterator.MoveNextAsync()); Check.Equal("first", iterator.Current); cts.Cancel(); await Check.ThrowsAsync<OperationCanceledException>(async () => { await iterator.MoveNextAsync(); }); }
        Check.Equal("second", remaining.ReadLine());
        await Check.ThrowsAsync<ArgumentNullException>(async () => { await foreach (var line in NonBlankLinesAsync(null!)) { _ = line; } });
    }
}

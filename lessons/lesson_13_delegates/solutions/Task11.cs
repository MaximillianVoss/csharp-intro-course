using Course;

namespace Course.Lesson13;

public static class Task11
{
    public static List<Exception> InvokeAll(Action[] handlers)
    {
        if (handlers is null) throw new ArgumentException("Null array."); foreach (var handler in handlers) if (handler is null) throw new ArgumentException("Null handler.");
        var errors = new List<Exception>(); foreach (var handler in handlers) { try { handler(); } catch (Exception error) { errors.Add(error); } } return errors;
    }
    public static void Run()
    {
        var trace = new List<string>(); Action handlers = () => trace.Add("A"); handlers += () => throw new FormatException("bad"); handlers += () => trace.Add("B");
        Check.Throws<FormatException>(() => handlers()); Check.SequenceEqual(new[] { "A" }, trace); trace.Clear();
        var errors = InvokeAll(new Action[] { () => trace.Add("A"), () => throw new FormatException(), () => trace.Add("B"), () => throw new IOException() });
        Check.SequenceEqual(new[] { "A", "B" }, trace); Check.Equal(2, errors.Count); Check.True(errors[0] is FormatException); Check.True(errors[1] is IOException);
        int calls = 0; Check.Throws<ArgumentException>(() => InvokeAll(new Action[] { () => calls++, null! })); Check.Equal(0, calls);
        Check.Equal(0, InvokeAll(Array.Empty<Action>()).Count); Check.Throws<ArgumentException>(() => InvokeAll(null!));
    }
}

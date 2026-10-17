using Course;

namespace Course.Lesson12;

public static class Task06
{
    public sealed class ArrayStack<T>
    {
        private readonly List<T> values = new();
        public int Count => values.Count;
        public void Push(T value) => values.Add(value);
        public bool TryPop(out T value)
        {
            if (values.Count == 0) { value = default!; return false; }
            int i = values.Count - 1; value = values[i]; values.RemoveAt(i); return true;
        }
    }
    public static void Run()
    {
        var stack = new ArrayStack<int>(); stack.Push(0); stack.Push(7); Check.Equal(2, stack.Count);
        Check.True(stack.TryPop(out var a)); Check.Equal(7, a); Check.True(stack.TryPop(out var b)); Check.Equal(0, b);
        Check.True(!stack.TryPop(out var c)); Check.Equal(0, c); Check.Equal(0, stack.Count);
        var strings = new ArrayStack<string?>(); strings.Push(null); Check.True(strings.TryPop(out var s)); Check.Equal<string?>(null, s);
    }
}

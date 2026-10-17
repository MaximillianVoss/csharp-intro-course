using Course;

namespace Course.Lesson12;

public static class Task09
{
    public sealed class Result<T>
    {
        private readonly T value;
        private Result(bool success, T value, string? error) { IsSuccess = success; this.value = value; Error = error; }
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T Value => IsSuccess ? value : throw new InvalidOperationException("No value.");
        public static Result<T> Ok(T value) => new(true, value, null);
        public static Result<T> Fail(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("Error required."); return new(false, default!, error);
        }
    }
    public static void Run()
    {
        var zero = Result<int>.Ok(0); Check.True(zero.IsSuccess); Check.Equal(0, zero.Value); Check.Equal<string?>(null, zero.Error);
        var nullable = Result<string?>.Ok(null); Check.True(nullable.IsSuccess); Check.Equal<string?>(null, nullable.Value);
        var bad = Result<int>.Fail("bad"); Check.True(!bad.IsSuccess); Check.Equal("bad", bad.Error); Check.Throws<InvalidOperationException>(() => { _ = bad.Value; });
        Check.Throws<ArgumentException>(() => Result<int>.Fail(" "));
    }
}

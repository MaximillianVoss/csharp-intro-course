using System.Collections.Generic;

namespace Course;

public static class Check
{
    public static int Assertions { get; private set; }
    public static void Equal<T>(T expected, T actual)
    {
        Assertions++;
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException($"Expected <{expected}>, got <{actual}>.");
    }
    public static void True(bool condition, string? message = null)
    {
        Assertions++;
        if (!condition) throw new InvalidOperationException(message ?? "Expected true.");
    }
    public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        Assertions++;
        if (!expected.SequenceEqual(actual)) throw new InvalidOperationException("Sequences differ.");
    }
    public static void Near(double expected, double actual, double tolerance = 1e-9)
    {
        Assertions++;
        if (!double.IsFinite(expected) || !double.IsFinite(actual) || !double.IsFinite(tolerance) || tolerance < 0 || Math.Abs(expected - actual) > tolerance)
            throw new InvalidOperationException($"Expected {expected} within {tolerance}, got {actual}.");
    }
    public static TException Throws<TException>(Action action) where TException : Exception
    {
        Assertions++;
        try { action(); }
        catch (TException exception) { return exception; }
        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }
    public static async Task<TException> ThrowsAsync<TException>(Func<Task> action) where TException : Exception
    {
        Assertions++;
        try { await action(); }
        catch (TException exception) { return exception; }
        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }
}

namespace Course.Lesson06;

public static class Task05
{
    public static void Reverse(int[] values)
    {
        for (int left = 0, right = values.Length - 1; left < right; left++, right--)
        {
            int temporary = values[left];
            values[left] = values[right];
            values[right] = temporary;
        }
    }
    private static void WrongNoOp(int[] values) { _ = values; }
    public static void Run()
    {
        int[] empty = Array.Empty<int>(); Reverse(empty);
        Check.SequenceEqual(Array.Empty<int>(), empty);
        int[] one = { 6 }; Reverse(one); Check.SequenceEqual(new[] { 6 }, one);
        int[] even = { 1, 2, 3, 4 }; Reverse(even);
        Check.SequenceEqual(new[] { 4, 3, 2, 1 }, even);
        int[] odd = { 1, 1, 3 }; Reverse(odd); Check.SequenceEqual(new[] { 3, 1, 1 }, odd);
        int[] wrongNoOp = { 1, 2, 3, 4 }; WrongNoOp(wrongNoOp);
        Check.True(wrongNoOp[0] != even[0], "Асимметричный тест различает no-op.");
    }
}

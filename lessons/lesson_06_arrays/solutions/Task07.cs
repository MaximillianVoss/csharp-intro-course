namespace Course.Lesson06;

public static class Task07
{
    public static int[,] EncodeRuns(int[] values)
    {
        int runs = 0;
        for (int i = 0; i < values.Length; i++) if (i == 0 || values[i] != values[i - 1]) runs++;
        int[,] result = new int[runs, 2];
        int row = -1;
        for (int i = 0; i < values.Length; i++)
        {
            if (i == 0 || values[i] != values[i - 1]) { row++; result[row, 0] = values[i]; }
            result[row, 1]++;
        }
        return result;
    }
    public static void Run()
    {
        int[,] encoded = EncodeRuns(new[] { 2, 2, 5, 2, 2, 2 });
        Check.Equal(3, encoded.GetLength(0));
        Check.SequenceEqual(new[] { 2, 2, 5, 1, 2, 3 }, Flatten(encoded));
        Check.Equal(4, EncodeRuns(new[] { 7, 7, 7, 7 })[0, 1]);
        int[,] empty = EncodeRuns(Array.Empty<int>());
        Check.Equal(0, empty.GetLength(0)); Check.Equal(2, empty.GetLength(1));
        Check.Equal(3, EncodeRuns(new[] { 1, 2, 1 }).GetLength(0));
    }

    private static int[] Flatten(int[,] matrix)
    {
        int[] result = new int[matrix.Length]; int index = 0;
        foreach (int value in matrix) result[index++] = value;
        return result;
    }
}

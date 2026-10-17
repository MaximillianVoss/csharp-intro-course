namespace Course.Lesson06;

public static class Task13
{
    public static bool[,] StrictPeaks(int[,] matrix)
    {
        int rows = matrix.GetLength(0), columns = matrix.GetLength(1);
        bool[,] result = new bool[rows, columns];
        for (int r = 0; r < rows; r++) for (int c = 0; c < columns; c++)
        {
            int value = matrix[r, c]; bool peak = true;
            if (r > 0 && matrix[r - 1, c] >= value) peak = false;
            if (r + 1 < rows && matrix[r + 1, c] >= value) peak = false;
            if (c > 0 && matrix[r, c - 1] >= value) peak = false;
            if (c + 1 < columns && matrix[r, c + 1] >= value) peak = false;
            result[r, c] = peak;
        }
        return result;
    }
    public static void Run()
    {
        bool[,] result = StrictPeaks(new[,] { { 4, 1 }, { 2, 3 } });
        Check.True(result[0, 0] && result[1, 1] && !result[0, 1] && !result[1, 0]);
        bool[,] plateau = StrictPeaks(new[,] { { 5, 5 } }); Check.True(!plateau[0, 0] && !plateau[0, 1]);
        Check.True(StrictPeaks(new[,] { { int.MinValue } })[0, 0]);
        Check.Equal(0, StrictPeaks(new int[0, 2]).Length);
        Check.True(!StrictPeaks(new[,] { { 2 }, { 3 } })[0, 0]);
    }
}

namespace Course.Lesson06;

public static class Task11
{
    public static int MaxSumRow(int[,] matrix)
    {
        int rows = matrix.GetLength(0), columns = matrix.GetLength(1);
        if (rows == 0 || columns == 0) return -1;
        long bestSum = long.MinValue; int best = -1;
        for (int row = 0; row < rows; row++)
        {
            long total = 0;
            for (int column = 0; column < columns; column++) total += matrix[row, column];
            if (total > bestSum) { bestSum = total; best = row; }
        }
        return best;
    }
    public static void Run()
    {
        Check.Equal(1, MaxSumRow(new[,] { { 1, 2, 3 }, { 8, -1, 0 } }));
        Check.Equal(1, MaxSumRow(new[,] { { -9, -1 }, { -2, -3 } }));
        Check.Equal(0, MaxSumRow(new[,] { { 1, 2 }, { 2, 1 } }));
        Check.Equal(-1, MaxSumRow(new int[2, 0])); Check.Equal(-1, MaxSumRow(new int[0, 2]));
        Check.Equal(0, MaxSumRow(new[,] { { int.MaxValue, int.MaxValue }, { 1, 2 } }));
    }
}

namespace Course.Lesson06;

public static class Task12
{
    public static int[,] Transpose(int[,] matrix)
    {
        int[,] result = new int[matrix.GetLength(1), matrix.GetLength(0)];
        for (int row = 0; row < matrix.GetLength(0); row++)
            for (int column = 0; column < matrix.GetLength(1); column++)
                result[column, row] = matrix[row, column];
        return result;
    }
    public static void Run()
    {
        int[,] source = { { 1, 2, 3 }, { 4, 5, 6 } };
        int[,] result = Transpose(source);
        Check.Equal(3, result.GetLength(0)); Check.Equal(2, result.GetLength(1));
        Check.Equal(5, result[1, 1]); Check.Equal(3, result[2, 0]);
        int[,] empty = Transpose(new int[0, 3]);
        Check.Equal(3, empty.GetLength(0)); Check.Equal(0, empty.GetLength(1));
        int[,] twice = Transpose(result); Check.Equal(6, twice[1, 2]);
        result[0, 0] = 99; Check.Equal(1, source[0, 0]);
    }
}

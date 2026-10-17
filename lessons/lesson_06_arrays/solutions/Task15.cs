namespace Course.Lesson06;

public static class Task15
{
    public static int[] FindSeatBlock(bool[,] occupied, int groupSize)
    {
        for (int row = 0; row < occupied.GetLength(0); row++)
        {
            int run = 0;
            for (int column = 0; column < occupied.GetLength(1); column++)
            {
                run = occupied[row, column] ? 0 : run + 1;
                if (run == groupSize) return new[] { row, column - groupSize + 1 };
            }
        }
        return Array.Empty<int>();
    }
    public static void Run()
    {
        bool[,] map = { { false, true, false, false }, { false, false, false, true } };
        Check.SequenceEqual(new[] { 0, 2 }, FindSeatBlock(map, 2));
        Check.SequenceEqual(Array.Empty<int>(), FindSeatBlock(map, 4));
        Check.SequenceEqual(Array.Empty<int>(), FindSeatBlock(new[,] { { true, false }, { false, true } }, 2));
        Check.SequenceEqual(Array.Empty<int>(), FindSeatBlock(new bool[0, 3], 1));
        Check.SequenceEqual(new[] { 0, 0 }, FindSeatBlock(map, 1));
        Check.True(!map[0, 2]);
    }
}

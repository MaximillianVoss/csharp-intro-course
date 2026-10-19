namespace Course.Lesson07;

public static class Task15
{
    public static int ShortestPath(bool[,] blocked, int startRow, int startColumn, int endRow, int endColumn)
    {
        if (blocked[startRow, startColumn] || blocked[endRow, endColumn]) return -1;
        int rows = blocked.GetLength(0), columns = blocked.GetLength(1);
        int[,] distance = new int[rows, columns];
        for (int r = 0; r < rows; r++) for (int c = 0; c < columns; c++) distance[r, c] = -1;
        Queue<int> queue = new(); queue.Enqueue(startRow * columns + startColumn); distance[startRow, startColumn] = 0;
        int[] dr = { -1, 1, 0, 0 }; int[] dc = { 0, 0, -1, 1 };
        while (queue.Count > 0)
        {
            int id = queue.Dequeue(); int r = id / columns, c = id % columns;
            if (r == endRow && c == endColumn) return distance[r, c];
            for (int direction = 0; direction < 4; direction++)
            {
                int nr = r + dr[direction], nc = c + dc[direction];
                if (nr < 0 || nr >= rows || nc < 0 || nc >= columns || blocked[nr, nc] || distance[nr, nc] >= 0) continue;
                distance[nr, nc] = distance[r, c] + 1; queue.Enqueue(nr * columns + nc);
            }
        }
        return -1;
    }
    public static void Run()
    {
        bool[,] map = { { false, true, false }, { false, false, false } };
        Check.Equal(4, ShortestPath(map, 0, 0, 0, 2));
        Check.Equal(-1, ShortestPath(new[,] { { false, true, false } }, 0, 0, 0, 2));
        Check.Equal(0, ShortestPath(new bool[1, 1], 0, 0, 0, 0));
        Check.Equal(-1, ShortestPath(new[,] { { true, false } }, 0, 0, 0, 1));
        Check.True(map[0, 1] && !map[1, 1]);
        Check.Equal(-1, ShortestPath(new[,] { { false, true } }, 0, 0, 0, 1));
        Check.Equal(4, ShortestPath(new bool[2, 4], 0, 0, 1, 3));
    }
}

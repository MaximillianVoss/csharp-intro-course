using Course;

namespace Course.Lesson12;

public static class Task15
{
    public sealed class Grid<T>
    {
        private readonly T[] data;
        public Grid(int rows, int columns, T initial)
        {
            if (rows < 1 || rows > 1000) throw new ArgumentOutOfRangeException(nameof(rows));
            if (columns < 1 || columns > 1000) throw new ArgumentOutOfRangeException(nameof(columns));
            Rows = rows; Columns = columns; data = new T[rows * columns]; Array.Fill(data, initial);
        }
        public int Rows { get; } public int Columns { get; }
        private int Index(int row, int column)
        {
            if (row < 0 || row >= Rows) throw new ArgumentOutOfRangeException(nameof(row));
            if (column < 0 || column >= Columns) throw new ArgumentOutOfRangeException(nameof(column)); return row * Columns + column;
        }
        public T this[int row, int column] { get => data[Index(row, column)]; set => data[Index(row, column)] = value; }
        public T[] CopyRow(int row) { int start = Index(row, 0); var result = new T[Columns]; Array.Copy(data, start, result, 0, Columns); return result; }
    }
    public static void Run()
    {
        var grid = new Grid<int>(2, 3, 0); grid[1, 2] = 7; Check.SequenceEqual(new[] { 0, 0, 7 }, grid.CopyRow(1)); Check.Equal(0, grid[0, 2]);
        var row = grid.CopyRow(1); row[2] = 99; Check.Equal(7, grid[1, 2]); Check.Equal(2, grid.Rows); Check.Equal(3, grid.Columns);
        Check.Throws<ArgumentOutOfRangeException>(() => { _ = grid[0, 3]; }); Check.Throws<ArgumentOutOfRangeException>(() => { grid[-1, 0] = 1; });
        Check.Throws<ArgumentOutOfRangeException>(() => grid.CopyRow(2)); Check.Throws<ArgumentOutOfRangeException>(() => new Grid<int>(0, 2, 0)); Check.Throws<ArgumentOutOfRangeException>(() => new Grid<int>(2, 1001, 0));
        var item = new object(); var refs = new Grid<object>(1, 2, item); Check.True(ReferenceEquals(refs[0, 0], refs[0, 1]));
    }
}

using Course;
int[] source = { 8, 3, 3, 1 };
int[] alias = source;
int[] copy = (int[])source.Clone();
alias[0] = 9;
Check.Equal(9, source[0]); Check.Equal(8, copy[0]);
Array.Sort(copy);
Check.SequenceEqual(new[] { 1, 3, 3, 8 }, copy);
int low = 0, high = copy.Length;
while (low < high)
{
    int middle = low + (high - low) / 2;
    if (copy[middle] < 3) low = middle + 1; else high = middle;
}
Check.Equal(1, low);
int[,] table = { { 2, 4, 6 }, { 1, 3, 5 } };
Check.Equal(2, table.GetLength(0)); Check.Equal(3, table.GetLength(1));
Console.WriteLine($"Первая тройка: {low}; матрица {table.GetLength(0)}x{table.GetLength(1)}.");

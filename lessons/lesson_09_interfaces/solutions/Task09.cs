namespace Course.Lesson09;

public static class Task09
{
    public interface IIntRepository { void Save(int[] values); int[] Load(); }
    public sealed class MemoryRepository : IIntRepository
    {
        private int[] data = Array.Empty<int>();
        public void Save(int[] values) { data = (int[])values.Clone(); }
        public int[] Load() => (int[])data.Clone();
    }
    public static void Run()
    {
        IIntRepository repository = new MemoryRepository(); Check.Equal(0, repository.Load().Length);
        int[] input = { 1, 2 }; repository.Save(input); input[0] = 9;
        Check.SequenceEqual(new[] { 1, 2 }, repository.Load());
        int[] loaded = repository.Load(); loaded[1] = 8; Check.SequenceEqual(new[] { 1, 2 }, repository.Load());
        repository.Save(new[] { 7 }); Check.SequenceEqual(new[] { 7 }, repository.Load());
    }
}

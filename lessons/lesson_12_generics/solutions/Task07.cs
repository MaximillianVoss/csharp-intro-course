using Course;

namespace Course.Lesson12;

public static class Task07
{
    public interface ISource<out T> { bool HasNext { get; } T Read(); }
    public interface ISink<in T> { void Write(T value); }
    public class Animal { public Animal(string name) { Name = name; } public string Name { get; } }
    public sealed class Dog : Animal { public Dog(string name) : base(name) { } }
    private sealed class ArraySource<T> : ISource<T>
    {
        private readonly T[] values; private int position; public int ReadCount { get; private set; }
        public ArraySource(T[] values) { this.values = values; } public bool HasNext => position < values.Length;
        public T Read() { if (!HasNext) throw new InvalidOperationException("Read after EOF."); ReadCount++; return values[position++]; }
    }
    private sealed class ListSink<T> : ISink<T>
    {
        public List<T> Values { get; } = new(); public int WriteCount { get; private set; }
        public void Write(T value) { WriteCount++; Values.Add(value); }
    }
    public static int CopyAtMost<T>(ISource<T> source, ISink<T> sink, int limit)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(sink); if (limit < 0 || limit > 1000) throw new ArgumentOutOfRangeException(nameof(limit));
        int copied = 0; while (copied < limit && source.HasNext) { T value = source.Read(); sink.Write(value); copied++; } return copied;
    }
    public static void Run()
    {
        var first = new Dog("A"); var second = new Dog("B"); var dogs = new ArraySource<Dog>(new[] { first, second }); ISource<Animal> animals = dogs;
        var animalSink = new ListSink<Animal>(); Check.Equal(0, CopyAtMost(animals, animalSink, 0)); Check.Equal(0, dogs.ReadCount); Check.Equal(0, animalSink.WriteCount);
        Check.Equal(1, CopyAtMost(animals, animalSink, 1)); Check.Equal(1, dogs.ReadCount); Check.True(ReferenceEquals(first, animalSink.Values[0]));
        ISink<Dog> acceptsDogs = animalSink; Check.Equal(1, CopyAtMost(dogs, acceptsDogs, 5)); Check.Equal(2, dogs.ReadCount); Check.Equal(2, animalSink.WriteCount); Check.True(ReferenceEquals(second, animalSink.Values[1]));
        Check.Equal(0, CopyAtMost(dogs, acceptsDogs, 1)); Check.Equal(2, dogs.ReadCount);
        Check.Throws<ArgumentOutOfRangeException>(() => CopyAtMost(dogs, acceptsDogs, -1)); Check.Throws<ArgumentOutOfRangeException>(() => CopyAtMost(dogs, acceptsDogs, 1001));
        Check.Throws<ArgumentNullException>(() => CopyAtMost<Dog>(null!, acceptsDogs, 1)); Check.Throws<ArgumentNullException>(() => CopyAtMost<Dog>(dogs, null!, 1));
    }
}

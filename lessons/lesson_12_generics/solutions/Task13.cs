using Course;

namespace Course.Lesson12;

public static class Task13
{
    public class Animal { public Animal(string name) { Name = name; } public string Name { get; } }
    public sealed class Cat : Animal { public Cat(string name) : base(name) { } }
    public static string[] Names(IReadOnlyList<Animal> animals)
    {
        ArgumentNullException.ThrowIfNull(animals); var result = new string[animals.Count];
        for (int i = 0; i < animals.Count; i++) { if (animals[i] is null) throw new ArgumentException("Null animal."); result[i] = animals[i].Name; } return result;
    }
    public static void Run()
    {
        List<Cat> cats = new() { new Cat("Мур"), new Cat("Барс") }; IReadOnlyList<Animal> view = cats;
        Check.SequenceEqual(new[] { "Мур", "Барс" }, Names(cats)); Check.Equal(2, view.Count);
        Check.Equal(0, Names(new List<Cat>()).Length); Check.Throws<ArgumentException>(() => Names(new Animal[] { null! }));
        Check.Throws<ArgumentNullException>(() => Names(null!));
    }
}

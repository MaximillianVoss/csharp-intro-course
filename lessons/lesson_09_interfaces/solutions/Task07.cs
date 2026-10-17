namespace Course.Lesson09;

public static class Task07
{
    public class Animal { public virtual string Speak() => "unknown"; }
    public sealed class BrokenDog : Animal { public string Speak(string suffix) => "woof" + suffix; }
    public sealed class Dog : Animal { public override string Speak() => "woof"; }
    public static void Run()
    {
        Animal broken = new BrokenDog(); Check.Equal("unknown", broken.Speak());
        Check.Equal("woof!", new BrokenDog().Speak("!"));
        Animal fixedDog = new Dog(); Check.Equal("woof", fixedDog.Speak());
        Animal[] animals = { new Animal(), new Dog() }; Check.Equal("unknown", animals[0].Speak()); Check.Equal("woof", animals[1].Speak());
    }
}

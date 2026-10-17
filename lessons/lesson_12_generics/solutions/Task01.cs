using Course;

namespace Course.Lesson12;

public static class Task01
{
    public static void Swap<T>(ref T first, ref T second) { T temp = first; first = second; second = temp; }
    public static void Run()
    {
        int a = 2, b = 7; Swap(ref a, ref b); Check.Equal(7, a); Check.Equal(2, b);
        string? x = null, y = "text"; Swap<string?>(ref x, ref y); Check.Equal("text", x); Check.Equal<string?>(null, y);
        Swap(ref a, ref a); Check.Equal(7, a);
    }
}

using Course;

namespace Course.Lesson12;

public static class Task08
{
    public sealed class Widget
    {
        public static int Created { get; set; } public int Value { get; set; }
        public Widget() { Created++; }
    }
    public sealed class Note { public string Text { get; set; } = "empty"; }
    public static T[] CreateMany<T>(int count) where T : class, new()
    {
        if (count < 0 || count > 1000) throw new ArgumentOutOfRangeException(nameof(count));
        var result = new T[count]; for (int i = 0; i < result.Length; i++) result[i] = new T(); return result;
    }
    public static void Run()
    {
        Widget.Created = 0; var widgets = CreateMany<Widget>(3); Check.Equal(3, Widget.Created); Check.Equal(3, widgets.Length);
        Check.True(widgets[0] is not null && widgets[1] is not null && widgets[2] is not null);
        Check.True(!ReferenceEquals(widgets[0], widgets[1]) && !ReferenceEquals(widgets[0], widgets[2]) && !ReferenceEquals(widgets[1], widgets[2]));
        widgets[0].Value = 9; Check.Equal(0, widgets[1].Value); Check.Equal(0, widgets[2].Value);
        var notes = CreateMany<Note>(2); notes[0].Text = "changed"; Check.Equal("empty", notes[1].Text);
        Check.Equal(0, CreateMany<Widget>(0).Length); Check.Equal(3, Widget.Created);
        Check.Throws<ArgumentOutOfRangeException>(() => CreateMany<Widget>(-1)); Check.Throws<ArgumentOutOfRangeException>(() => CreateMany<Widget>(1001)); Check.Equal(3, Widget.Created);
    }
}

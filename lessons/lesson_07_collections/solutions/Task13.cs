namespace Course.Lesson07;

public static class Task13
{
    public static int ReplayEditor(string[] commands)
    {
        int current = 0; Stack<int> undo = new(); Stack<int> redo = new();
        foreach (string command in commands)
        {
            if (command.StartsWith("set ", StringComparison.Ordinal))
            {
                undo.Push(current); current = int.Parse(command.Substring(4), System.Globalization.CultureInfo.InvariantCulture); redo.Clear();
            }
            else if (command == "undo" && undo.Count > 0) { redo.Push(current); current = undo.Pop(); }
            else if (command == "redo" && redo.Count > 0) { undo.Push(current); current = redo.Pop(); }
        }
        return current;
    }
    public static void Run()
    {
        Check.Equal(8, ReplayEditor(new[] { "set 4", "set 8", "undo", "redo" }));
        Check.Equal(7, ReplayEditor(new[] { "set 4", "undo", "set 7", "redo" }));
        Check.Equal(0, ReplayEditor(new[] { "undo", "redo" }));
        Check.Equal(0, ReplayEditor(new[] { "set 1", "set 2", "undo", "undo" }));
        Check.Equal(4, ReplayEditor(new[] { "set 4", "set 4", "undo" }));
        Check.Equal(1, ReplayEditor(new[] { "set 1", "set 2", "undo", "set 1", "redo" }));
        System.Globalization.CultureInfo previous = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo unusual = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.InvariantCulture.Clone();
            unusual.NumberFormat.NegativeSign = "minus"; System.Globalization.CultureInfo.CurrentCulture = unusual;
            Check.Equal(int.MinValue, ReplayEditor(new[] { "set -2147483648" }));
        }
        finally { System.Globalization.CultureInfo.CurrentCulture = previous; }
    }
}

namespace Course.Lesson07;

public static class Task05
{
    public static bool IsBalanced(string text)
    {
        Stack<char> opens = new();
        foreach (char symbol in text)
        {
            if (symbol == '(' || symbol == '[' || symbol == '{') opens.Push(symbol);
            else if (symbol == ')' || symbol == ']' || symbol == '}')
            {
                if (opens.Count == 0) return false;
                char open = opens.Pop();
                if ((symbol == ')' && open != '(') || (symbol == ']' && open != '[') || (symbol == '}' && open != '{')) return false;
            }
        }
        return opens.Count == 0;
    }
    public static void Run()
    {
        Check.True(IsBalanced("a([b]{c})")); Check.True(!IsBalanced("([)]"));
        Check.True(!IsBalanced("(")); Check.True(!IsBalanced(")")); Check.True(IsBalanced(""));
    }
}

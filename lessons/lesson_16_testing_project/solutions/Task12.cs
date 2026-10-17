namespace Course.Lesson16;

public static class Task12
{
    public static void Run()
    {
        var queue=new Queue<int>(); var model=new List<int>(); var observed=new List<int>();
        int?[] operations=[4,7,null,9,null,null];
        foreach(int? operation in operations)
        {
            if(operation is int value){queue.Enqueue(value);model.Add(value);}
            else{int expected=model[0];model.RemoveAt(0);int actual=queue.Dequeue();Check.Equal(expected,actual);observed.Add(actual);}
            Check.Equal(model.Count,queue.Count);
        }
        Check.SequenceEqual(new[]{4,7,9},observed);
        Check.Throws<InvalidOperationException>(()=>queue.Dequeue());
        var mutant=new Stack<int>();mutant.Push(4);mutant.Push(7);Check.True(mutant.Pop()!=observed[0]);
    }
}

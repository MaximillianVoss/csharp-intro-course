namespace Course.Lesson16;

public static class Task10
{
    public sealed record Options(int Limit,bool Reverse);
    public static Options Parse(string[] args)
    {
        int limit=10; bool reverse=false,seenLimit=false;
        for(int i=0;i<args.Length;i++)
        {
            if(args[i]=="--reverse"&&!reverse){reverse=true;continue;}
            if(args[i]=="--limit"&&!seenLimit&&i+1<args.Length)
            {
                string value=args[++i]; seenLimit=true;
                if(value.Length==0||value.Any(c=>c is < '0' or > '9')||!int.TryParse(value,out limit)||limit is < 1 or > 100)
                    throw new ArgumentException("Неверный limit.");
                continue;
            }
            throw new ArgumentException("Неизвестная или повторная опция.");
        }
        return new(limit,reverse);
    }
    public static void Run()
    {
        Check.Equal(new Options(10,false),Parse([]));
        Check.Equal(new Options(3,true),Parse(["--reverse","--limit","3"]));
        Check.Equal(new Options(1,true),Parse(["--limit","1","--reverse"]));
        Check.Equal(100,Parse(["--limit","100"]).Limit);
        string[][] wrong=[["--reverse","--reverse"],["--limit"],["--limit","0"],["--limit","101"],["--limit","+2"],["other"],["--limit","10","--limit","2"]];
        foreach(var row in wrong)Check.Throws<ArgumentException>(()=>Parse(row));
    }
}

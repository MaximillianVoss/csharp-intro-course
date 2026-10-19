namespace Course.Lesson16;

public static class Task04
{
    public static double Percent(int done,int total)
    {
        if (total <= 0 || done < 0 || done > total) throw new ArgumentOutOfRangeException(nameof(total));
        return 100.0 * done / total;
    }
    public static void Run()
    {
        Check.Near(100.0/3,Percent(1,3)); Check.Near(100,Percent(int.MaxValue,int.MaxValue));
        Check.Near(0,Percent(0,1)); Check.Near(25,Percent(1,4));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(1,0));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(3,2));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(-1,2));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(0,-1));
        Func<int,int,double> truncated=(done,total)=>100*done/total;
        Func<int,int,double> overflowed=(done,total)=>unchecked(100*done)/total;
        Check.True(Math.Abs(truncated(1,3)-Percent(1,3))>0.1);
        Check.True(overflowed(int.MaxValue,int.MaxValue)!=Percent(int.MaxValue,int.MaxValue));
    }
}

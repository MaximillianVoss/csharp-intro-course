namespace Course.Lesson16;

public static class Task08
{
    public static bool IsValidSort(int[] original,int[] result)
    {
        if(original.Length!=result.Length)return false;
        for(int i=1;i<result.Length;i++)if(result[i]<result[i-1])return false;
        var counts=new Dictionary<int,int>();
        foreach(int n in original)counts[n]=counts.GetValueOrDefault(n)+1;
        foreach(int n in result) { if(!counts.TryGetValue(n,out int count)||count==0)return false; counts[n]=count-1; }
        return true;
    }
    public static void Run()
    {
        Check.True(IsValidSort([],[])); Check.True(IsValidSort([2,1,2],[1,2,2]));
        Check.True(!IsValidSort([2,1,2],[1,2])); Check.True(!IsValidSort([1,4],[2,3]));
        Check.True(!IsValidSort([2,1],[2,1]));
        Check.True(IsValidSort([int.MaxValue,int.MinValue],[int.MinValue,int.MaxValue]));
        Check.True(!IsValidSort([1,1,2],[1,2,2]));
        int[] original=[3,1,3,2],result=[1,2,3,3];
        int[] originalCopy=(int[])original.Clone(),resultCopy=(int[])result.Clone();
        Check.True(IsValidSort(original,result));
        Check.SequenceEqual(originalCopy,original); Check.SequenceEqual(resultCopy,result);
    }
}

namespace Course.Lesson16;

public static class Task14
{
    public static void Apply(Dictionary<string,int> stock,(string Product,int Quantity)[] batch)
    {
        if(batch.Length>100)throw new ArgumentException("Слишком большой пакет.");
        var seen=new HashSet<string>(StringComparer.Ordinal);
        foreach(var row in batch)
            if(row.Product is null||!stock.ContainsKey(row.Product)||row.Quantity is < 0 or > 100||!seen.Add(row.Product))
                throw new ArgumentException("Некорректное обновление.");
        foreach(var row in batch)stock[row.Product]=row.Quantity;
    }
    public static void Run()
    {
        var stock=new Dictionary<string,int>(StringComparer.Ordinal){{"tea",2},{"milk",1}};
        Check.Throws<ArgumentException>(()=>Apply(stock,[("tea",5),("milk",-1)]));Check.Equal(2,stock["tea"]);Check.Equal(1,stock["milk"]);
        Check.Throws<ArgumentException>(()=>Apply(stock,[("other",1)]));
        Check.Throws<ArgumentException>(()=>Apply(stock,[("tea",5),("tea",6)]));Check.Equal(2,stock["tea"]);
        Apply(stock,[("tea",5),("milk",0)]);Check.Equal(5,stock["tea"]);Check.Equal(0,stock["milk"]);Apply(stock,[]);
        var snapshot=stock.OrderBy(p=>p.Key,StringComparer.Ordinal).ToArray();
        foreach(var invalid in new (string Product,int Quantity)[][] {
            [("tea",1),("milk",101)],[("tea",1),(null!,2)],[("tea",1),("",2)] })
        {
            Check.Throws<ArgumentException>(()=>Apply(stock,invalid));
            Check.SequenceEqual(snapshot,stock.OrderBy(p=>p.Key,StringComparer.Ordinal));
        }
        var large=Enumerable.Range(0,101).ToDictionary(i=>"p"+i,_=>0,StringComparer.Ordinal);
        var tooLong=large.Keys.Select(key=>(Product:key,Quantity:1)).ToArray();
        Check.Throws<ArgumentException>(()=>Apply(large,tooLong));
        Check.True(large.Values.All(n=>n==0));
        Apply(large,tooLong.Take(100).ToArray());
        Check.Equal(100,large.Values.Count(n=>n==1));
    }
}

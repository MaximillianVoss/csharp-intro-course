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
    }
}

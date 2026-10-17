namespace Course.Lesson16;

public static class Task15
{
    public static async Task ScenarioAsync()
    {
        string root=Path.Combine(Path.GetTempPath(),"course-acceptance-"+Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);string path=Path.Combine(root,"books.json");
        try
        {
            Course.Library.CliApplication NewApp()=>new(p=>new Course.Library.JsonLibraryStore(p));
            Check.Equal(0,await NewApp().RunAsync(["--data",path,"add","Азбука","Автор","2000"],TextWriter.Null,TextWriter.Null));
            Check.Equal(0,await NewApp().RunAsync(["--data",path,"read","1"],TextWriter.Null,TextWriter.Null));
            using var output=new StringWriter();
            Check.Equal(0,await NewApp().RunAsync(["--data",path,"stats"],output,TextWriter.Null));
            Check.True(output.ToString().Contains("Всего: 1; прочитано: 1; осталось: 0.",StringComparison.Ordinal));
            await File.WriteAllTextAsync(path,"{broken");
            byte[] broken=await File.ReadAllBytesAsync(path);
            Check.Equal(1,await NewApp().RunAsync(["--data",path,"stats"],TextWriter.Null,TextWriter.Null));
            Check.SequenceEqual(broken,await File.ReadAllBytesAsync(path));
        }
        finally{Directory.Delete(root,recursive:true);}
    }
    public static void Run()=>ScenarioAsync().GetAwaiter().GetResult();
}

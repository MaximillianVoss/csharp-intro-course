using Course.Library;

namespace Course.Library.Tests;

[TestClass]
public sealed class CliTests
{
    private sealed class MemoryStore : ILibraryStore
    {
        public LibraryCatalog Catalog { get; } = new();
        public int Loads { get; private set; }
        public int Saves { get; private set; }
        public Task<LibraryCatalog> LoadAsync(CancellationToken token = default)
        { token.ThrowIfCancellationRequested(); Loads++; return Task.FromResult(Catalog); }
        public Task SaveAsync(LibraryCatalog catalog, CancellationToken token = default)
        { token.ThrowIfCancellationRequested(); Saves++; return Task.CompletedTask; }
    }

    [TestMethod]
    public async Task HelpDoesNotAccessStorage()
    {
        using var output = new StringWriter();
        int code = await new CliApplication(_ => throw new InvalidOperationException("Must not access"))
            .RunAsync(["help"], output, TextWriter.Null);
        Assert.AreEqual(0, code);
        StringAssert.Contains(output.ToString(), "add");
    }

    [TestMethod]
    [DataRow("unknown")]
    [DataRow("add")]
    [DataRow("read", "0")]
    [DataRow("remove", "-1")]
    [DataRow("list", "extra")]
    [DataRow("--data")]
    [DataRow("search", " ")]
    [DataRow("stats", "extra")]
    public async Task WrongArgumentsDoNotLoad(params string[] args)
    {
        var store = new MemoryStore();
        int code = await new CliApplication(_ => store).RunAsync(args, TextWriter.Null, TextWriter.Null);
        Assert.AreEqual(2, code);
        Assert.AreEqual(0, store.Loads);
    }

    [TestMethod]
    public async Task AddReadListSearchRemoveAndStats()
    {
        var store = new MemoryStore();
        var app = new CliApplication(_ => store);
        using var output = new StringWriter();
        Assert.AreEqual(0, await app.RunAsync(["add", "Азбука", "Автор", "2000"], output, TextWriter.Null));
        Assert.AreEqual(1, store.Saves);
        Assert.AreEqual(0, await app.RunAsync(["read", "1"], output, TextWriter.Null));
        Assert.AreEqual(0, await app.RunAsync(["list", "--unread"], output, TextWriter.Null));
        StringAssert.Contains(output.ToString(), "Книг нет.");
        Assert.AreEqual(0, await app.RunAsync(["search", "аЗб"], output, TextWriter.Null));
        StringAssert.Contains(output.ToString(), "[1] Азбука");
        Assert.AreEqual(0, await app.RunAsync(["stats"], output, TextWriter.Null));
        StringAssert.Contains(output.ToString(), "Всего: 1; прочитано: 1; осталось: 0.");
        Assert.AreEqual(0, await app.RunAsync(["remove", "1"], output, TextWriter.Null));
        Assert.AreEqual(3, store.Saves);
    }

    [TestMethod]
    public async Task UnknownIdReturnsThreeAndDoesNotSave()
    {
        var store = new MemoryStore();
        int code = await new CliApplication(_ => store).RunAsync(["read", "7"], TextWriter.Null, TextWriter.Null);
        Assert.AreEqual(3, code);
        Assert.AreEqual(0, store.Saves);
    }

    [TestMethod]
    public async Task InvalidBookReturnsTwoAndDoesNotSave()
    {
        var store = new MemoryStore();
        int code = await new CliApplication(_ => store).RunAsync(["add", "", "Автор", "2000"], TextWriter.Null, TextWriter.Null);
        Assert.AreEqual(2, code);
        Assert.AreEqual(0, store.Saves);
    }

    [TestMethod]
    public async Task ExplicitDataPathPassedToFactory()
    {
        string? observed = null;
        int code = await new CliApplication(path => { observed = path; return new MemoryStore(); })
            .RunAsync(["--data", "folder/books.json", "list"], TextWriter.Null, TextWriter.Null);
        Assert.AreEqual(0, code);
        Assert.AreEqual("folder/books.json", observed);
    }

    [TestMethod]
    public async Task CorruptFileIsErrorAndProducesNoSuccessMessage()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        int code = await new CliApplication(_ => throw new InvalidDataException("Повреждён JSON"))
            .RunAsync(["stats"], output, error);
        Assert.AreEqual(1, code);
        Assert.AreEqual("", output.ToString());
        StringAssert.Contains(error.ToString(), "Повреждён JSON");
    }

    [TestMethod]
    public async Task CancellationReportedAs130()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        int code = await new CliApplication(_ => new MemoryStore())
            .RunAsync(["list"], TextWriter.Null, TextWriter.Null, cancellation.Token);
        Assert.AreEqual(130, code);
    }
}

using System.Text;
using Course.Library;

namespace Course.Library.Tests;

[TestClass]
public sealed class StoreTests
{
    private string directory = null!;
    private string file = null!;

    [TestInitialize]
    public void CreateDirectory()
    {
        directory = Path.Combine(Path.GetTempPath(), "csharp-course-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        file = Path.Combine(directory, "каталог.json");
    }

    [TestCleanup]
    public void RemoveDirectory() => Directory.Delete(directory, recursive: true);

    [TestMethod]
    public async Task MissingFileIsNewCatalogWithoutCreatingFile()
    {
        var catalog = await new JsonLibraryStore(file).LoadAsync();
        Assert.AreEqual(0, catalog.Books.Count);
        Assert.IsFalse(File.Exists(file));
    }

    [TestMethod]
    public async Task SaveLoadUnicodeReadFlagAndNextId()
    {
        var catalog = new LibraryCatalog();
        catalog.Add("Азбука 🌲", "Автор 😀", 2020);
        catalog.Add("Удаляемая", "Автор", 2021);
        catalog.Remove(2);
        catalog.MarkRead(1);
        await new JsonLibraryStore(file).SaveAsync(catalog);
        var loaded = await new JsonLibraryStore(file).LoadAsync();
        Assert.AreEqual(catalog.Books[0], loaded.Books[0]);
        Assert.AreEqual(3, loaded.Add("Новая", "Автор", 2000).Id);
        Assert.AreEqual(1, Directory.GetFiles(directory).Length);
    }

    [TestMethod]
    public async Task NestedDirectoryCreatedOnlyOnSave()
    {
        string nested = Path.Combine(directory, "a", "b", "books.json");
        var store = new JsonLibraryStore(nested);
        await store.LoadAsync();
        Assert.IsFalse(Directory.Exists(Path.GetDirectoryName(nested)));
        await store.SaveAsync(new LibraryCatalog());
        Assert.IsTrue(File.Exists(nested));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("{broken")]
    [DataRow("null")]
    [DataRow("{}")]
    [DataRow("{\"schemaVersion\":2,\"nextId\":1,\"books\":[]}")]
    [DataRow("{\"schemaVersion\":1,\"nextId\":0,\"books\":[]}")]
    [DataRow("{\"schemaVersion\":1,\"nextId\":1,\"books\":null}")]
    [DataRow("{\"schemaVersion\":1,\"nextId\":2,\"books\":[null]}")]
    [DataRow("{\"schemaVersion\":1,\"nextId\":2,\"books\":[{\"id\":1,\"title\":\"\",\"author\":\"Автор\",\"year\":2000}]}")]
    [DataRow("{\"schemaVersion\":1,\"nextId\":2,\"books\":[{\"id\":1,\"title\":\"Книга\",\"author\":null,\"year\":2000}]}")]
    public async Task BadDocumentIsRejectedAndNeverOverwritten(string json)
    {
        byte[] before = Encoding.UTF8.GetBytes(json);
        await File.WriteAllBytesAsync(file, before);
        await Assert.ThrowsAsync<InvalidDataException>(() => new JsonLibraryStore(file).LoadAsync());
        CollectionAssert.AreEqual(before, await File.ReadAllBytesAsync(file));
    }

    [TestMethod]
    public async Task DuplicateIdsAndStaleNextIdRejected()
    {
        const string book = "{\"id\":1,\"title\":\"Книга\",\"author\":\"Автор\",\"year\":2000}";
        await File.WriteAllTextAsync(file, "{\"schemaVersion\":1,\"nextId\":2,\"books\":[" + book + "," + book + "]}");
        await Assert.ThrowsAsync<InvalidDataException>(() => new JsonLibraryStore(file).LoadAsync());
        await File.WriteAllTextAsync(file, "{\"schemaVersion\":1,\"nextId\":1,\"books\":[" + book + "]}");
        await Assert.ThrowsAsync<InvalidDataException>(() => new JsonLibraryStore(file).LoadAsync());
    }

    [TestMethod]
    public async Task LargeDocumentRejectedBeforeDeserialization()
    {
        await File.WriteAllTextAsync(file, new string(' ', JsonLibraryStore.MaxBytes + 1));
        await Assert.ThrowsAsync<InvalidDataException>(() => new JsonLibraryStore(file).LoadAsync());
    }

    [TestMethod]
    public async Task CancelledSavePreservesExistingBytesAndLeavesNoTemporaryFile()
    {
        var store = new JsonLibraryStore(file);
        await store.SaveAsync(new LibraryCatalog());
        byte[] before = await File.ReadAllBytesAsync(file);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(() => store.SaveAsync(new LibraryCatalog(), cancellation.Token));
        CollectionAssert.AreEqual(before, await File.ReadAllBytesAsync(file));
        Assert.AreEqual(1, Directory.GetFiles(directory).Length);
    }

    [TestMethod]
    public async Task CancelledLoadDoesNotMasqueradeAsMissingFile()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(() => new JsonLibraryStore(file).LoadAsync(cancellation.Token));
    }
}

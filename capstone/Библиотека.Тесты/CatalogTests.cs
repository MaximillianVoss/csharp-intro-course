using Course.Library;

namespace Course.Library.Tests;

[TestClass]
public sealed class CatalogTests
{
    [TestMethod]
    public void NewCatalogIsEmpty()
    {
        var catalog = new LibraryCatalog();
        Assert.AreEqual((0, 0, 0), catalog.Statistics());
        Assert.AreEqual(1, catalog.NextId);
    }

    [TestMethod]
    public void AddTrimsTextAndPreservesUnicode()
    {
        var book = new LibraryCatalog().Add("  Азбука 🌲  ", "  Автор  ", 2020);
        Assert.AreEqual("Азбука 🌲", book.Title);
        Assert.AreEqual("Автор", book.Author);
        Assert.IsFalse(book.IsRead);
    }

    [TestMethod]
    [DataRow("", "Автор", 2000)]
    [DataRow(" ", "Автор", 2000)]
    [DataRow("Текст\nстрока", "Автор", 2000)]
    [DataRow("Название", "", 2000)]
    [DataRow("Название", "Автор", 1449)]
    [DataRow("Название", "Автор", 2101)]
    public void InvalidAddDoesNotConsumeId(string title, string author, int year)
    {
        var catalog = new LibraryCatalog();
        Assert.Throws<ArgumentException>(() => catalog.Add(title, author, year));
        Assert.AreEqual(1, catalog.NextId);
        Assert.AreEqual(0, catalog.Books.Count);
    }

    [TestMethod]
    public void UnpairedSurrogatesAreRejectedWithoutConsumingId()
    {
        var catalog = new LibraryCatalog();
        foreach (string invalid in new[] { "A\uD800B", "A\uDC00B", "\uD800", "\uDC00" })
        {
            Assert.Throws<ArgumentException>(() => catalog.Add(invalid, "Автор", 2000));
            Assert.Throws<ArgumentException>(() => catalog.Add("Книга", invalid, 2000));
            Assert.AreEqual(1, catalog.NextId);
            Assert.AreEqual(0, catalog.Books.Count);
        }
        var book = catalog.Add("Книга \U0001F600", "Автор \U0001F332", 2000);
        Assert.AreEqual("Книга \U0001F600", book.Title);
        Assert.AreEqual("Автор \U0001F332", book.Author);
    }

    [TestMethod]
    public void ExactTextAndYearBoundaries()
    {
        var catalog = new LibraryCatalog();
        catalog.Add(new string('я', 120), new string('а', 80), 1450);
        catalog.Add("Верхняя граница", "Автор", 2100);
        Assert.Throws<ArgumentException>(() => catalog.Add(new string('я', 121), "Автор", 2000));
        Assert.Throws<ArgumentException>(() => catalog.Add("Книга", new string('а', 81), 2000));
        Assert.AreEqual(2, catalog.Books.Count);
    }

    [TestMethod]
    public void NullAndInvalidIdsRejected()
    {
        Assert.Throws<ArgumentNullException>(() => new Book(1, null!, "Автор", 2000));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book(0, "Книга", "Автор", 2000));
        Assert.Throws<ArgumentOutOfRangeException>(() => new LibraryCatalog([], 0));
    }

    [TestMethod]
    public void RemovedIdNeverReusedAndReadIsIdempotent()
    {
        var catalog = new LibraryCatalog();
        catalog.Add("Первый", "Автор", 2001);
        catalog.Add("Второй", "Автор", 2002);
        Assert.IsTrue(catalog.MarkRead(2));
        Assert.IsTrue(catalog.MarkRead(2));
        Assert.AreEqual((2, 1, 1), catalog.Statistics());
        Assert.IsTrue(catalog.Remove(2));
        Assert.AreEqual(3, catalog.Add("Третий", "Автор", 2003).Id);
        Assert.AreEqual((2, 0, 2), catalog.Statistics());
    }

    [TestMethod]
    public void UnknownIdLeavesCatalogUnchanged()
    {
        var catalog = new LibraryCatalog();
        catalog.Add("Книга", "Автор", 2000);
        Assert.IsFalse(catalog.Remove(77));
        Assert.IsFalse(catalog.MarkRead(77));
        Assert.AreEqual((1, 0, 1), catalog.Statistics());
    }

    [TestMethod]
    public void ConstructorRejectsConflictingSnapshot()
    {
        var book = new Book(1, "Книга", "Автор", 2000);
        Assert.Throws<ArgumentException>(() => new LibraryCatalog([book, book], 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => new LibraryCatalog([book], 1));
        Assert.Throws<ArgumentException>(() => new LibraryCatalog([null!], 2));
    }

    [TestMethod]
    public void ReadOnlyViewIsSnapshotAndInputCollectionIsCopied()
    {
        var original = new List<Book> { new(1, "Книга", "Автор", 2000) };
        var catalog = new LibraryCatalog(original, 2);
        original.Clear();
        var snapshot = (Book[])catalog.Books;
        snapshot[0] = new Book(8, "Другой", "Автор", 2000);
        Assert.AreEqual(1, catalog.Books[0].Id);
        catalog.MarkRead(1);
        Assert.IsFalse(original.Any());
        Assert.IsTrue(catalog.Books[0].IsRead);
    }

    [TestMethod]
    public void SearchIsCaseInsensitiveByTitleOrAuthorAndOrderedById()
    {
        var catalog = new LibraryCatalog([
            new Book(3, "Другой", "АВТОР", 2000),
            new Book(1, "Азбука", "Некто", 2000)], 4);
        CollectionAssert.AreEqual(new[] { 1 }, catalog.Search("  аЗбУк  ").Select(b => b.Id).ToArray());
        CollectionAssert.AreEqual(new[] { 3 }, catalog.Search("автор").Select(b => b.Id).ToArray());
        Assert.AreEqual(0, catalog.Search("нет совпадений").Count);
        Assert.Throws<ArgumentException>(() => catalog.Search(" "));
    }

    [TestMethod]
    public void ExhaustedIdDoesNotOverflow()
    {
        var catalog = new LibraryCatalog([], int.MaxValue);
        Assert.Throws<InvalidOperationException>(() => catalog.Add("Книга", "Автор", 2000));
        Assert.AreEqual(int.MaxValue, catalog.NextId);
        Assert.AreEqual(0, catalog.Books.Count);
    }
}

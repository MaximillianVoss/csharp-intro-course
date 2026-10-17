using Course.Library;

var catalog = new LibraryCatalog();
catalog.Add("Азбука", "Автор", 2000);
catalog.MarkRead(1);
Check.Equal((1,1,0),catalog.Statistics());
Console.WriteLine("Модель проверена: одна книга, одна прочитана.");
Console.WriteLine("CLI и файловая приёмка: решение задачи 16-15.");

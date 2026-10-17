# Урок 10. Исключения, ресурсы и текстовые файлы: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="10-01"></a>
## 10-01. Ошибки чтения различаются

Отделите отсутствие строки, синтаксис числа и бизнес-диапазон, не заменяя все ошибки нулём.

**Ход решения**

ReadLine возвращает null на EOF, а пустую строку — для реально прочитанной пустой строки. int.Parse различает нечисловой текст и переполнение int. Только после успешного разбора проверяем диапазон порта. Каждый вид отказа несёт точную причину. Время O(длина строки), память O(длина строки).

**Частая ошибка**

Возвращать 0 при любой ошибке; путать EOF с пустой строкой; перехватывать Exception и скрывать дефекты.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
namespace Course.Lesson10;

public static class Task01
{
    public static int ReadPort(TextReader reader)
    {
        string line = reader.ReadLine() ?? throw new EndOfStreamException("Ожидалась строка порта.");
        int port = int.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
        if (port < 1 || port > 65535) throw new ArgumentOutOfRangeException(nameof(port), port, "Порт вне 1..65535.");
        return port;
    }
    public static void Run()
    {
        using StringReader normal = new(" 8080\n"); Check.Equal(8080, ReadPort(normal));
        using StringReader eof = new(""); Check.Throws<EndOfStreamException>(() => ReadPort(eof));
        using StringReader bad = new("x"); Check.Throws<FormatException>(() => ReadPort(bad));
        using StringReader huge = new("999999999999"); Check.Throws<OverflowException>(() => ReadPort(huge));
        using StringReader zero = new("0"); Check.Throws<ArgumentOutOfRangeException>(() => ReadPort(zero));
        using StringReader tooLarge = new("65536"); Check.Throws<ArgumentOutOfRangeException>(() => ReadPort(tooLarge));
        using StringReader lower = new("1"); Check.Equal(1, ReadPort(lower));
        using StringReader upper = new("65535"); Check.Equal(65535, ReadPort(upper));
    }
}
```

**Исполняемые проверки**

- Допустимая строка
- EOF
- Нечисло
- Переполнение int
- Нижняя и верхняя границы
- Выход за диапазон

<a id="10-02"></a>
## 10-02. Порядок using, catch и finally

using освобождает ресурс на выходе из своей области как при успехе, так и при исключении.

**Ход решения**

При обычном ходе после Dispose выполняется after, catch пропускается, finally выполняется всегда. При ошибке выход из using вызывает Dispose до поиска внешнего обработчика; after пропускается, затем выполняются catch и finally. Тест сравнивает весь порядок, поэтому позднее или двойное освобождение обнаружится.

**Частая ошибка**

Ожидать Dispose после catch; считать finally выполняемым только при ошибке; выполнять Dispose вручную внутри using.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
namespace Course.Lesson10;

public static class Task02
{
    private sealed class Probe : IDisposable
    {
        private readonly List<string> log; public Probe(List<string> log) { this.log = log; }
        public void Dispose() { log.Add("dispose"); }
    }
    public static string[] Trace(bool fail)
    {
        List<string> log = new();
        try
        {
            log.Add("enter");
            using (Probe probe = new(log))
            {
                log.Add("body");
                if (fail) throw new InvalidOperationException("demo");
            }
            log.Add("after");
        }
        catch (InvalidOperationException) { log.Add("catch"); }
        finally { log.Add("finally"); }
        return log.ToArray();
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { "enter", "body", "dispose", "after", "finally" }, Trace(false));
        Check.SequenceEqual(new[] { "enter", "body", "dispose", "catch", "finally" }, Trace(true));
    }
}
```

**Исполняемые проверки**

- Полный журнал нормального хода
- Полный журнал исключения
- Dispose ровно один раз в обоих случаях

<a id="10-03"></a>
## 10-03. Обработчик, до которого нельзя дойти

Общий catch делает следующий недостижимым; оставьте только ожидаемые узкие обработчики в порядке от частного к общему.

**Ход решения**

IOException наследует Exception, поэтому первый исходный catch перехватывал бы всё, что предназначено второму; компилятор запрещает такой порядок. FileNotFoundException в свою очередь наследует IOException и должен стоять первым. Исправленный вариант не маскирует FormatException. Исходник с ошибкой не исполняется, выполняется исправленный counterpart; причина компиляции явно описана.

**Частая ошибка**

Поменять порядок, но оставить catch(Exception), скрывающий неожиданные ошибки; FileNotFoundException после IOException.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
namespace Course.Lesson10;

public static class Task03
{
    private sealed class FailingReader : TextReader
    {
        private readonly Exception error; public FailingReader(Exception error) { this.error = error; }
        public override string? ReadLine() => throw error;
    }
    public static string ReadFirstStatus(TextReader reader)
    {
        try { string? line = reader.ReadLine(); return line is null ? "eof" : "ok:" + line; }
        catch (FileNotFoundException) { return "missing"; }
        catch (IOException) { return "io"; }
    }
    public static void Run()
    {
        using StringReader text = new("A"); using StringReader eof = new("");
        Check.Equal("ok:A", ReadFirstStatus(text)); Check.Equal("eof", ReadFirstStatus(eof));
        using FailingReader missing = new(new FileNotFoundException()); using FailingReader io = new(new IOException());
        Check.Equal("missing", ReadFirstStatus(missing)); Check.Equal("io", ReadFirstStatus(io));
        using FailingReader unexpected = new(new FormatException()); Check.Throws<FormatException>(() => ReadFirstStatus(unexpected));
    }
}
```

**Исполняемые проверки**

- Обычная строка
- EOF
- Специальный missing
- Общий io
- FormatException проходит наружу

<a id="10-04"></a>
## 10-04. Текст проходит через UTF-8 файл

Используйте явный UTF8Encoding без BOM; операции всего текста не нормализуют переводы строк.

**Ход решения**

Кодировка — перевод между символами и байтами. UTF-8 допускает разные длины байтов, поэтому число байтов не равно Length строки. WriteAllText/ReadAllText сохраняют имеющиеся CRLF/LF, в отличие от пересборки строк WriteLine. Уникальный temp предотвращает конфликт запусков; finally ограничивает владение файлами. O(L) времени и памяти.

**Частая ошибка**

Использовать ASCII для кириллицы; собирать строки с Environment.NewLine и менять исходные переводы; фиксированное общее имя temp без GUID.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
namespace Course.Lesson10;

public static class Task04
{
    public static string RoundTrip(string text)
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "text.txt");
            System.Text.UTF8Encoding encoding = new(false, true);
            File.WriteAllText(path, text, encoding);
            return File.ReadAllText(path, encoding);
        }
        finally { Directory.Delete(directory, true); }
    }
    public static void Run()
    {
        Check.Equal("Привет\r\nC#\n🙂", RoundTrip("Привет\r\nC#\n🙂")); Check.Equal("", RoundTrip(""));
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "bytes.txt"); File.WriteAllText(path, "Я", new System.Text.UTF8Encoding(false, true));
            Check.SequenceEqual(new byte[] { 0xD0, 0xAF }, File.ReadAllBytes(path));
        }
        finally { Directory.Delete(directory, true); }
        Check.True(!Directory.Exists(directory));
    }
}
```

**Исполняемые проверки**

- Кириллица, CRLF, LF и emoji
- Пустой текст
- UTF-8 байты слова Я без BOM
- Временный каталог удалён после проверки

<a id="10-05"></a>
## 10-05. Потоковый подсчёт содержательных строк

Цикл ReadLine возвращает строки до null, а IsNullOrWhiteSpace определяет полезность каждой.

**Ход решения**

Пустая строка — допустимый результат чтения, поэтому условие line!="" остановилось бы слишком рано. Последний перевод строки не обязан создавать дополнительную логическую строку, и не влияет на число содержательных. Потоковая обработка держит только одну строку. Время O(число символов), дополнительная память O(максимальная строка).

**Частая ошибка**

Остановиться на пустой строке; считать пробельную строку содержательной; dispose reader, которым владеет вызывающий.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
namespace Course.Lesson10;

public static class Task05
{
    public static int CountNonBlank(TextReader reader)
    {
        int count = 0; string? line;
        while ((line = reader.ReadLine()) is not null) if (!string.IsNullOrWhiteSpace(line)) count++;
        return count;
    }
    public static void Run()
    {
        using StringReader normal = new("A\n\n   \r\nB\n"); Check.Equal(2, CountNonBlank(normal));
        Check.True(normal.ReadLine() is null);
        using StringReader empty = new(""); Check.Equal(0, CountNonBlank(empty));
        using StringReader spaces = new(" \t\n\r\n"); Check.Equal(0, CountNonBlank(spaces));
        using StringReader last = new("last"); Check.Equal(1, CountNonBlank(last));
    }
}
```

**Исполняемые проверки**

- Пустые строки внутри потока
- Разные переводы строк
- Пустой поток
- Только пробелы
- Reader остаётся пригоден для чтения EOF

<a id="10-06"></a>
## 10-06. Импорт продолжается после плохой строки

Ожидаемые ошибки формата удобнее обрабатывать TryParse на уровне отдельной записи.

**Ход решения**

Глобальный catch вокруг всего цикла оборвал бы импорт на первой неверной строке. TryParse не бросает исключение на плохой int и позволяет продолжить. lineNumber растёт до фильтрации пустоты, поэтому диагностический номер соответствует файлу. Ошибку канала чтения нельзя объявлять плохим числом: она распространяется наружу. O(объём текста), память O(результаты).

**Частая ошибка**

Нумеровать только непустые строки; прервать весь цикл после первой ошибки; записывать невалидное как ноль.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
namespace Course.Lesson10;

public static class Task06
{
    public sealed record ParseReport(int[] Values, int[] InvalidLines);
    public static ParseReport ParseNumbers(TextReader reader)
    {
        List<int> values = new(); List<int> invalid = new(); int lineNumber = 0; string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++; if (string.IsNullOrWhiteSpace(line)) continue;
            if (int.TryParse(line, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value)) values.Add(value);
            else invalid.Add(lineNumber);
        }
        return new ParseReport(values.ToArray(), invalid.ToArray());
    }
    public static void Run()
    {
        using StringReader input = new("10\n\nwrong\n -2 \n999999999999"); ParseReport report = ParseNumbers(input);
        Check.SequenceEqual(new[] { 10, -2 }, report.Values); Check.SequenceEqual(new[] { 3, 5 }, report.InvalidLines);
        using StringReader zero = new("bad\n0\n \n7"); ParseReport after = ParseNumbers(zero);
        Check.SequenceEqual(new[] { 0, 7 }, after.Values); Check.SequenceEqual(new[] { 1 }, after.InvalidLines);
        using StringReader empty = new(""); Check.Equal(0, ParseNumbers(empty).Values.Length);
    }
}
```

**Исполняемые проверки**

- Номера физических строк 3 и 5
- Корректный ноль сохранён
- Пустой ввод
- Пробелы пропущены без ошибки
- Последующие числа после ошибки сохранены

<a id="10-07"></a>
## 10-07. Имя файла без выхода из рабочей папки

Сначала ограничьте имя одним переносимым компонентом; Path.Combine сам по себе не проверяет намерение.

**Ход решения**

Второй аргумент Combine может быть абсолютным путём, а .. имеет смысл перехода к родителю. Белый список исключает оба случая без платформенно различающихся наборов запрещённых символов. Существование файла не проверяется. Файловые операции теста ограничены собственным temp. O(длина имени).

**Частая ошибка**

Проверять только строковое начало результирующего пути; считать Combine санитаризацией; забыть обратный слеш или двоеточие.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
namespace Course.Lesson10;

public static class Task07
{
    public static string ResolveChild(string root, string? fileName)
    {
        if (string.IsNullOrEmpty(fileName) || fileName.Length > 100 || fileName == "." || fileName == "..") throw new ArgumentException("Неверное имя.", nameof(fileName));
        foreach (char c in fileName)
            if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '.' || c == '-' || c == '_'))
                throw new ArgumentException("Допускается один компонент ASCII.", nameof(fileName));
        return Path.Combine(root, fileName);
    }
    public static void Run()
    {
        string root = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(root);
        try
        {
            Check.Equal(Path.Combine(root, "report-1.txt"), ResolveChild(root, "report-1.txt"));
            foreach (string? invalid in new string?[] { "../report.txt", "a\\b", "C:other", "..", ".", null, " ", new string('a', 101) })
                Check.Throws<ArgumentException>(() => ResolveChild(root, invalid));
        }
        finally { Directory.Delete(root, true); }
    }
}
```

**Исполняемые проверки**

- Допустимое переносимое имя
- Оба разделителя
- Родительская точка
- Двоеточие
- null и пробелы
- Слишком длинное имя

<a id="10-08"></a>
## 10-08. Освобождение должно быть идемпотентным

Проверяйте флаг перед чтением и делайте Dispose безопасным для повторного вызова.

**Ход решения**

IDisposable — договор о завершении использования ресурса; сборщик мусора не определяет момент Dispose. Идемпотентность полезна при вложенной очистке. using вызывает Dispose при любом выходе, включая исключение. Тест хранит ссылку на учебный объект после using только для проверки факта освобождения. O(1).

**Частая ошибка**

Увеличивать счётчик на каждом Dispose; разрешать Read после освобождения; полагаться на финализатор вместо using.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
namespace Course.Lesson10;

public static class Task08
{
    public sealed class TokenLease : IDisposable
    {
        private readonly string token; private bool disposed;
        public int ReleaseCount { get; private set; }
        public TokenLease(string token) { this.token = token; }
        public string Read()
        {
            if (disposed) throw new ObjectDisposedException(nameof(TokenLease));
            return token;
        }
        public void Dispose() { if (disposed) return; disposed = true; ReleaseCount++; }
    }
    public static void Run()
    {
        TokenLease lease = new("abc"); Check.Equal("abc", lease.Read()); lease.Dispose(); lease.Dispose();
        Check.Equal(1, lease.ReleaseCount); Check.Throws<ObjectDisposedException>(() => lease.Read());
        TokenLease normal = new("x"); using (normal) { Check.Equal("x", normal.Read()); } Check.Equal(1, normal.ReleaseCount);
        TokenLease failed = new("y");
        Check.Throws<InvalidOperationException>(() => { using (failed) { throw new InvalidOperationException(); } });
        Check.Equal(1, failed.ReleaseCount); Check.Throws<ObjectDisposedException>(() => failed.Read());
    }
}
```

**Исполняемые проверки**

- Чтение до освобождения
- Повторный Dispose не удваивает освобождение
- Read после Dispose бросает
- using на нормальном ходе
- using при исключении

<a id="10-09"></a>
## 10-09. Журнал действительно дописывается

Создайте пустой файл заранее; каждый новый StreamWriter должен использовать append:true и завершить область using до чтения.

**Ход решения**

Без append каждый проход перезаписывал бы предыдущие записи. Dispose сбрасывает буфер и освобождает дескриптор, поэтому чтение идёт после закрытия writer. Пустая запись — отдельная строка, она не исчезает при ReadAllLines. Учебное отдельное открытие каждой записи показывает режим, хотя для большого журнала эффективнее один writer на пакет. O(объём текста).

**Частая ошибка**

Открывать с append:false; читать до flush/dispose; отфильтровать пустые записи как ошибки.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
namespace Course.Lesson10;

public static class Task09
{
    public static string[] AppendAndRead(string[] messages)
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "journal.txt"); System.Text.UTF8Encoding encoding = new(false, true);
            File.WriteAllText(path, "", encoding);
            foreach (string message in messages)
            {
                using StreamWriter writer = new(path, append: true, encoding: encoding); writer.WriteLine(message);
            }
            return File.ReadAllLines(path, encoding);
        }
        finally { Directory.Delete(directory, true); }
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { "первый", "", "третий" }, AppendAndRead(new[] { "первый", "", "третий" }));
        Check.SequenceEqual(Array.Empty<string>(), AppendAndRead(Array.Empty<string>()));
        Check.SequenceEqual(new[] { "один" }, AppendAndRead(new[] { "один" }));
    }
}
```

**Исполняемые проверки**

- Несколько записей не перезаписываются
- Пустая строка сохранена
- Нулевой пакет
- Кириллица
- Одна запись

<a id="10-10"></a>
## 10-10. Не читать дальше найденного

Обрабатывайте одну строку за раз и возвращайте сразу после совпадения.

**Ход решения**

Потоковая версия не держит весь документ и оставляет хвост непрочитанным. Условие строгое: строка ровно limit не подходит. Проверка limit выполняется до чтения, поэтому отказ не потребляет вход. Тест следующего ReadLine обнаруживает и чтение всего хвоста, и преждевременное Dispose. O(прочитанный префикс), память O(длина одной строки).

**Частая ошибка**

Использовать >=; прочитать весь документ, прежде чем вернуть; закрыть чужой reader.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
namespace Course.Lesson10;

public static class Task10
{
    public static string? FirstLongLine(TextReader reader, int limit)
    {
        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit));
        string? line;
        while ((line = reader.ReadLine()) is not null) if (line.Length > limit) return line;
        return null;
    }
    public static void Run()
    {
        using StringReader source = new("aa\nlong\ntail"); Check.Equal("long", FirstLongLine(source, 2)); Check.Equal("tail", source.ReadLine());
        using StringReader exact = new("ab\nabc"); Check.Equal("abc", FirstLongLine(exact, 2));
        using StringReader none = new("a\nb"); Check.True(FirstLongLine(none, 2) is null);
        using StringReader empty = new(""); Check.True(FirstLongLine(empty, 0) is null);
        using StringReader invalid = new("stay"); Check.Throws<ArgumentOutOfRangeException>(() => FirstLongLine(invalid, -1)); Check.Equal("stay", invalid.ReadLine());
    }
}
```

**Исполняемые проверки**

- Первая подходящая и непрочитанный хвост
- Ровно limit не подходит
- Нет совпадений
- Пустой поток
- Отрицательный limit не потребляет строку

<a id="10-11"></a>
## 10-11. Новый текст до точки фиксации

Отложите изменение целевого файла до одной операции фиксации; staging принадлежит методу и всегда очищается.

**Ход решения**

Прямой WriteAllText(path,...) уничтожил бы прежнее содержимое до успешного завершения подготовки. Отдельный файл делает проверяемую границу: до Move исходник неизменен. Файлы на одной директории исключают заведомо разные тома, но задача не обещает стойкость к отключению питания. finally удаляет только собственный staging. O(L) по размеру текста.

**Частая ошибка**

Сначала очистить целевой файл; оставить staging после исключения; утверждать доказанную надёжность при аппаратном сбое.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
namespace Course.Lesson10;

public static class Task11
{
    public static void WriteThenCommit(string path, string text, bool failBeforeCommit)
    {
        string staging = Path.Combine(Path.GetDirectoryName(path)!, ".stage-" + Guid.NewGuid().ToString("N"));
        try
        {
            File.WriteAllText(staging, text, new System.Text.UTF8Encoding(false, true));
            if (failBeforeCommit) throw new IOException("Учебный отказ до фиксации.");
            File.Move(staging, path, overwrite: true);
        }
        finally { if (File.Exists(staging)) File.Delete(staging); }
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "data.txt"); File.WriteAllText(path, "old");
            Check.Throws<IOException>(() => WriteThenCommit(path, "new", true)); Check.Equal("old", File.ReadAllText(path));
            Check.Equal(1, Directory.GetFiles(directory).Length);
            WriteThenCommit(path, "new", false); Check.Equal("new", File.ReadAllText(path)); Check.Equal(1, Directory.GetFiles(directory).Length);
            string second = Path.Combine(directory, "second.txt"); WriteThenCommit(second, "создан", false); Check.Equal("создан", File.ReadAllText(second));
        }
        finally { Directory.Delete(directory, true); }
    }
}
```

**Исполняемые проверки**

- Имитированный отказ сохраняет old
- После отказа нет staging
- Успешная замена
- Создание нового назначения
- Весь тестовый temp очищается

<a id="10-12"></a>
## 10-12. Отсутствие файла не равно пустому файлу

Попытайтесь прочитать и перехватите только два ожидаемых вида отсутствия.

**Ход решения**

Проверка Exists перед чтением не гарантирует, что файл останется до ReadAllText, и сливает несколько причин недоступности. Узкие catch сохраняют различимость отсутствия, пустого содержимого и ошибки. Нулевой символ тестирует распространение неожиданной для контракта ошибки без изменения прав доступа или внешних файлов. O(L) для прочитанного текста.

**Частая ошибка**

Любой IOException превратить в отсутствие; вернуть "" на отсутствие и потерять различие; проводить тест отказа через нестабильные права доступа.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
namespace Course.Lesson10;

public static class Task12
{
    public static string? ReadOptionalText(string path)
    {
        try { return File.ReadAllText(path, System.Text.Encoding.UTF8); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "file.txt"); Check.True(ReadOptionalText(path) is null);
            Check.True(ReadOptionalText(Path.Combine(directory, "missing", "file.txt")) is null);
            File.WriteAllText(path, ""); Check.Equal("", ReadOptionalText(path));
            File.WriteAllText(path, "данные", System.Text.Encoding.UTF8); Check.Equal("данные", ReadOptionalText(path));
            Check.Throws<ArgumentException>(() => ReadOptionalText(path + '\0'));
        }
        finally { Directory.Delete(directory, true); }
    }
}
```

**Исполняемые проверки**

- Отсутствующий файл
- Отсутствующий вложенный каталог
- Пустой существующий файл
- Кириллица
- Нулевой символ пути не подавляется

<a id="10-13"></a>
## 10-13. Причина исключения не потеряна

Добавьте понятный уровень контекста, сохранив техническую причину через inner.

**Ход решения**

Тот, кто работает с настройками, получает доменный тип ошибки, а диагностика сохраняет оригинальную IOException и её сведения. catch не должен охватывать все Exception: ошибка формата относится к данным и остаётся FormatException. throw new Exception(error.Message) уничтожил бы связь причин. Время и память O(текст).

**Частая ошибка**

Выбросить новое исключение только с Message; обернуть также FormatException; закрыть переданный reader.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
namespace Course.Lesson10;

public static class Task13
{
    public sealed class ConfigurationReadException : Exception
    {
        public ConfigurationReadException(string message, Exception inner) : base(message, inner) { }
    }
    private sealed class FailingReader : TextReader
    {
        private readonly Exception error; public FailingReader(Exception error) { this.error = error; }
        public override string ReadToEnd() => throw error;
    }
    public static string LoadRequired(TextReader reader)
    {
        try
        {
            string text = reader.ReadToEnd(); if (string.IsNullOrWhiteSpace(text)) throw new FormatException("Пустые настройки.");
            return text;
        }
        catch (IOException error) { throw new ConfigurationReadException("Не удалось прочитать настройки.", error); }
    }
    public static void Run()
    {
        using StringReader normal = new("mode=demo"); Check.Equal("mode=demo", LoadRequired(normal));
        using StringReader empty = new(" "); Check.Throws<FormatException>(() => LoadRequired(empty));
        IOException cause = new("broken channel"); using FailingReader failing = new(cause);
        ConfigurationReadException wrapped = Check.Throws<ConfigurationReadException>(() => LoadRequired(failing));
        Check.Equal("Не удалось прочитать настройки.", wrapped.Message); Check.True(ReferenceEquals(cause, wrapped.InnerException));
        using FailingReader unexpected = new(new InvalidOperationException()); Check.Throws<InvalidOperationException>(() => LoadRequired(unexpected));
    }
}
```

**Исполняемые проверки**

- Успешный текст сохранён
- Whitespace -> FormatException
- Доменный тип и сообщение
- ReferenceEquals исходной InnerException
- Непредусмотренная ошибка чтения проходит наружу

<a id="10-14"></a>
## 10-14. Повреждённый UTF-8 не должен молча заменяться

Задайте UTF8Encoding(false,true), явно обработайте допустимый UTF-8 BOM и проверьте плохие байты.

**Ход решения**

Стандартный мягкий декодер заменяет плохие байты, из-за чего повреждение может остаться незамеченным. Строгий декодер делает ошибку наблюдаемой. StreamReader с автоматическим определением BOM способен выбрать UTF-16, что не соответствует этому контракту, поэтому читаем байты и обрабатываем только UTF-8 префикс. O(B) времени и памяти.

**Частая ошибка**

Считать успешный round-trip корректного текста проверкой повреждений; автоматически принять UTF-16; оставить U+FEFF перед текстом.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
namespace Course.Lesson10;

public static class Task14
{
    public static string ReadStrictUtf8(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        int offset = bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF ? 3 : 0;
        return new System.Text.UTF8Encoding(false, true).GetString(bytes, offset, bytes.Length - offset);
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "bytes.txt"); File.WriteAllBytes(path, new byte[] { 0xD0, 0xAF }); Check.Equal("Я", ReadStrictUtf8(path));
            File.WriteAllBytes(path, new byte[] { 0xEF, 0xBB, 0xBF, 0x41 }); Check.Equal("A", ReadStrictUtf8(path));
            File.WriteAllBytes(path, Array.Empty<byte>()); Check.Equal("", ReadStrictUtf8(path));
            byte[] corrupt = { 0xC3, 0x28 }; File.WriteAllBytes(path, corrupt);
            Check.Throws<System.Text.DecoderFallbackException>(() => ReadStrictUtf8(path));
            string wrong = System.Text.Encoding.UTF8.GetString(corrupt); Check.True(wrong.Contains('\uFFFD'));
        }
        finally { Directory.Delete(directory, true); }
    }
}
```

**Исполняемые проверки**

- Кириллица без BOM
- UTF-8 BOM исключён из текста
- Пустой файл
- Повреждение бросает точный тип
- Мягкий ошибочный вариант содержит U+FFFD

<a id="10-15"></a>
## 10-15. Сводка нескольких текстовых файлов

Соедините обнаружение и сортировку файлов, потоковый разбор каждой записи, отдельную диагностику и освобождение reader.

**Ход решения**

Порядок файловой системы не гарантирован, поэтому сортировка делает отчёт повторяемым. Расширение проверяется явно, поскольку платформенная маска может иначе учитывать регистр. Каждый reader живёт только на время одного файла. Счётчик строк сбрасывается при новом файле, глобальная сумма сохраняется. Ограничения гарантируют вместимость long. Сложность O(F log F+B), память O(F+E+максимальная строка).

**Частая ошибка**

Полагаться на порядок GetFiles; учитывать .TXT вопреки точному расширению; нумеровать только успешные строки; суммировать в int.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
namespace Course.Lesson10;

public static class Task15
{
    public sealed record ImportSummary(long Sum, int Accepted, string[] Errors);
    public static ImportSummary SummarizeFiles(string directory)
    {
        string[] files = Directory.GetFiles(directory); Array.Sort(files, StringComparer.Ordinal);
        long sum = 0; int accepted = 0; List<string> errors = new();
        foreach (string path in files)
        {
            if (!string.Equals(Path.GetExtension(path), ".txt", StringComparison.Ordinal)) continue;
            using StreamReader reader = new(path, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            int lineNumber = 0; string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                lineNumber++; if (string.IsNullOrWhiteSpace(line)) continue;
                if (int.TryParse(line, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value)) { sum += value; accepted++; }
                else errors.Add(Path.GetFileName(path) + ":" + lineNumber.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }
        return new ImportSummary(sum, accepted, errors.ToArray());
    }
    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "csharp-course-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
        try
        {
            ImportSummary empty = SummarizeFiles(directory); Check.Equal(0L, empty.Sum); Check.Equal(0, empty.Accepted); Check.Equal(0, empty.Errors.Length);
            File.WriteAllText(Path.Combine(directory, "b.txt"), "wrong\n5"); File.WriteAllText(Path.Combine(directory, "a.txt"), "2\nbad\n3");
            File.WriteAllText(Path.Combine(directory, "ignore.dat"), "99"); File.WriteAllText(Path.Combine(directory, "caps.TXT"), "99");
            ImportSummary result = SummarizeFiles(directory); Check.Equal(10L, result.Sum); Check.Equal(3, result.Accepted);
            Check.SequenceEqual(new[] { "a.txt:2", "b.txt:1" }, result.Errors);
            File.WriteAllText(Path.Combine(directory, "large.txt"), "2147483647\n\n2147483647\nwrong");
            ImportSummary large = SummarizeFiles(directory); Check.Equal(4294967304L, large.Sum); Check.Equal(5, large.Accepted);
            Check.SequenceEqual(new[] { "a.txt:2", "b.txt:1", "large.txt:4" }, large.Errors);
        }
        finally { Directory.Delete(directory, true); }
    }
}
```

**Исполняемые проверки**

- Несколько файлов и упорядоченные ошибки
- Не-txt и .TXT игнорируются
- Пустые строки не портят нумерацию
- Сумма больше int.MaxValue
- Пустой каталог
- Продолжение после ошибки

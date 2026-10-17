# Урок 15. Асинхронность, отмена и потоки данных: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="15-01"></a>
## 15-01. Ожидание результата функции

Вызвать делегат и await полученного Task.

**Ход решения**

Task<int> описывает будущее завершение, а int получается через await. Сохранение связи жизненного цикла проверяется TaskCompletionSource: до SetResult внешний Task не должен завершиться. Завершение исходного обещания управляет тестом без предположений о скорости процессора. Возвращённая ошибка остаётся ошибкой внешнего Task.

**Частая ошибка**

Вызов transform без await и возврат заранее вычисленного значения теряет жизненный цикл операции.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task01
{
    public static async Task<int> TransformAsync(int value, Func<int, Task<int>> transform)
    {
        ArgumentNullException.ThrowIfNull(transform); return await transform(value);
    }
    public static async Task Run()
    {
        Check.Equal(6, await TransformAsync(3, x => Task.FromResult(x * 2)));
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); int calls = 0;
        var pending = TransformAsync(3, x => { calls++; Check.Equal(3, x); return gate.Task; }); Check.True(!pending.IsCompleted); Check.Equal(1, calls);
        gate.SetResult(8); Check.Equal(8, await pending);
        await Check.ThrowsAsync<FormatException>(() => TransformAsync(1, _ => Task.FromException<int>(new FormatException())));
        await Check.ThrowsAsync<ArgumentNullException>(() => TransformAsync(0, null!));
    }
}
```

**Исполняемые проверки**

- Завершённый результат
- Незавершённая зависимость и один вызов
- Ошибка и null зависимости

<a id="15-02"></a>
## 15-02. Асинхронный файловый round trip

Проверить параметры и отмену, затем await записи и чтения.

**Ход решения**

Read до ожидания Write мог бы увидеть старый или неполный файл. Оба await фиксируют требуемый порядок. Явный ThrowIfCancellationRequested перед первым I/O делает политику предварительной отмены однозначной. UTF-8 сохраняет разные письменности. Временный каталог ограничивает побочные эффекты теста.

**Частая ошибка**

Синхронные ReadAllText/WriteAllText внутри async не становятся асинхронными от имени метода.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task02
{
    public static async Task<string> RoundTripAsync(string path, string text, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path."); ArgumentNullException.ThrowIfNull(text); token.ThrowIfCancellationRequested();
        await File.WriteAllTextAsync(path, text, Encoding.UTF8, token); return await File.ReadAllTextAsync(path, Encoding.UTF8, token);
    }
    public static async Task Run()
    {
        var dir = Path.Combine(Path.GetTempPath(), "csharp-async-" + Guid.NewGuid()); Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "sample.txt"); Check.Equal("строка\n二", await RoundTripAsync(path, "строка\n二")); Check.Equal("", await RoundTripAsync(path, ""));
            var absent = Path.Combine(dir, "cancelled.txt"); using var cts = new CancellationTokenSource(); cts.Cancel();
            await Check.ThrowsAsync<OperationCanceledException>(() => RoundTripAsync(absent, "x", cts.Token)); Check.True(!File.Exists(absent));
            await Check.ThrowsAsync<ArgumentException>(() => RoundTripAsync(" ", "x")); await Check.ThrowsAsync<ArgumentNullException>(() => RoundTripAsync(path, null!));
        }
        finally { Directory.Delete(dir, true); }
    }
}
```

**Исполняемые проверки**

- Unicode и перенос
- Пустой текст
- Предварительная отмена не создаёт файл
- Путь и null text отклонены

<a id="15-03"></a>
## 15-03. До и после незавершённого await

Результат start,caller,end; async выполняется до первого незавершённого await.

**Ход решения**

Вызов async-метода начинает исполнять его тело сразу. Добавление start происходит до await незавершённого gate. После возврата Task вызывающий записывает caller и разрешает продолжение через SetResult. Await work гарантирует, что end уже добавлен перед снимком. RunContinuationsAsynchronously не означает случайную перестановку start/caller: gate ещё закрыт.

**Частая ошибка**

async не означает автоматический запуск всего тела в новом потоке.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task03
{
    public static async Task<string[]> ObserveAsync()
    {
        var trace = new List<string>(); var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        async Task WorkAsync() { trace.Add("start"); await gate.Task; trace.Add("end"); }
        Task work = WorkAsync(); Check.True(!work.IsCompleted); trace.Add("caller"); gate.SetResult(); await work; return trace.ToArray();
    }
    public static async Task Run()
    {
        Check.SequenceEqual(new[] { "start", "caller", "end" }, await ObserveAsync());
        Check.SequenceEqual(new[] { "start", "caller", "end" }, await ObserveAsync());
    }
}
```

**Исполняемые проверки**

- Точный исходный порядок
- Work не завершён до gate
- Повторный запуск независим

<a id="15-04"></a>
## 15-04. Исправление блокирующего чтения результата

Сделать метод async, await input, затем checked умножение.

**Ход решения**

Исходный return возвращает int вместо Task<int>. Механическая обёртка в Task.FromResult(input.Result*2) исправила бы тип, но сохранила блокировку. Await отдаёт управление при незавершённом input и продолжает после его результата. Checked не даёт молчаливого переполнения. Gate-тест обнаруживает связь завершений без временных задержек.

**Частая ошибка**

Task.Run вокруг .Result прячет блокировку в другом потоке, а не исправляет протокол ожидания.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task04
{
    public static async Task<int> DoubleAsync(Task<int> input)
    {
        ArgumentNullException.ThrowIfNull(input); int value = await input; return checked(value * 2);
    }
    public static async Task Run()
    {
        Check.Equal(8, await DoubleAsync(Task.FromResult(4))); Check.Equal(-6, await DoubleAsync(Task.FromResult(-3)));
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); var pending = DoubleAsync(gate.Task); Check.True(!pending.IsCompleted); gate.SetResult(5); Check.Equal(10, await pending);
        await Check.ThrowsAsync<FormatException>(() => DoubleAsync(Task.FromException<int>(new FormatException())));
        await Check.ThrowsAsync<OverflowException>(() => DoubleAsync(Task.FromResult(int.MaxValue))); await Check.ThrowsAsync<ArgumentNullException>(() => DoubleAsync(null!));
    }
}
```

**Исполняемые проверки**

- Обычный и отрицательный результат
- Незавершённый input
- Ошибка input и переполнение
- null input

<a id="15-05"></a>
## 15-05. Отмена до и после загрузки

Отмена является отдельным условием, которое зависимости могут не проверять сами.

**Ход решения**

Передача token не означает автоматическую реакцию на него. Предварительная проверка защищает от запуска ненужного действия. Повторная проверка нужна, когда зависимость завершилась успешно, проигнорировав отмену. Тест вызывает Cancel прямо внутри зависимости, поэтому порядок детерминирован. Проверка null-строки выполняется после проверки отмены согласно указанной последовательности.

**Частая ошибка**

Создание нового CancellationToken вместо передачи исходного разрывает управление отменой.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task05
{
    public static async Task<int> LengthAsync(Func<CancellationToken, Task<string>> load, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(load); token.ThrowIfCancellationRequested(); var text = await load(token); token.ThrowIfCancellationRequested();
        return text?.Length ?? throw new InvalidOperationException("Null text.");
    }
    public static async Task Run()
    {
        using var cts = new CancellationTokenSource(); Check.Equal(3, await LengthAsync(t => { Check.Equal(cts.Token, t); return Task.FromResult("abc"); }, cts.Token));
        cts.Cancel(); int calls = 0; await Check.ThrowsAsync<OperationCanceledException>(() => LengthAsync(_ => { calls++; return Task.FromResult("x"); }, cts.Token)); Check.Equal(0, calls);
        using var during = new CancellationTokenSource(); await Check.ThrowsAsync<OperationCanceledException>(() => LengthAsync(_ => { during.Cancel(); return Task.FromResult("abc"); }, during.Token));
        await Check.ThrowsAsync<InvalidOperationException>(() => LengthAsync(_ => Task.FromResult<string>(null!))); await Check.ThrowsAsync<ArgumentNullException>(() => LengthAsync(null!));
    }
}
```

**Исполняемые проверки**

- Обычная строка и тот же token
- Предварительная отмена без вызова
- Отмена внутри успешной загрузки
- null и ошибочный результат

<a id="15-06"></a>
## 15-06. Все результаты в исходном порядке

Асинхронный wrapper превращает синхронные ошибки фабрик в faulted Task; WhenAll собирает результаты.

**Ход решения**

Создаём массив задач циклом, не ожидая каждую внутри цикла. InvokeAsync вызывает фабрику внутри async-тела: её throw сохраняется в Task, поэтому цикл может запустить остальных. WhenAll ожидает все завершения и возвращает значения в том же индексном порядке. Проверка двух gate показывает и совместный запуск, и отсутствие зависимости от порядка завершения.

**Частая ошибка**

await factory внутри цикла сериализует независимые операции.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task06
{
    private static async Task<int> InvokeAsync(Func<Task<int>> factory)
    {
        var task = factory() ?? throw new InvalidOperationException("Null task."); return await task;
    }
    public static async Task<int[]> RunAllAsync(Func<Task<int>>[] factories)
    {
        if (factories is null || factories.Any(f => f is null)) throw new ArgumentException("Factories.");
        var tasks = new Task<int>[factories.Length]; for (int i = 0; i < tasks.Length; i++) tasks[i] = InvokeAsync(factories[i]); return await Task.WhenAll(tasks);
    }
    public static async Task Run()
    {
        var a = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); var b = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); int starts = 0;
        var pending = RunAllAsync(new Func<Task<int>>[] { () => { starts++; return a.Task; }, () => { starts++; return b.Task; } }); Check.Equal(2, starts); b.SetResult(20); Check.True(!pending.IsCompleted); a.SetResult(10); Check.SequenceEqual(new[] { 10, 20 }, await pending);
        int later = 0; await Check.ThrowsAsync<FormatException>(() => RunAllAsync(new Func<Task<int>>[] { () => throw new FormatException(), () => { later++; return Task.FromResult(2); } })); Check.Equal(1, later);
        await Check.ThrowsAsync<InvalidOperationException>(() => RunAllAsync(new Func<Task<int>>[] { () => null! })); Check.Equal(0, (await RunAllAsync(Array.Empty<Func<Task<int>>>())).Length);
        await Check.ThrowsAsync<ArgumentException>(() => RunAllAsync(new Func<Task<int>>[] { () => { later++; return Task.FromResult(0); }, null! })); Check.Equal(1, later);
    }
}
```

**Исполняемые проверки**

- Все фабрики запущены до завершения первой
- Обратное завершение сохраняет порядок
- Синхронная ошибка не подавляет последующие запуски
- null конфигурация/Task и пустота

<a id="15-07"></a>
## 15-07. Первая завершившаяся операция

Явно обработать завершённые задачи по приоритету, иначе await WhenAny, затем await победителя.

**Ход решения**

WhenAny возвращает Task-победитель, а не его строковое значение. Поэтому требуется второй await; именно он наблюдает исключение победителя. Явные IsCompleted делают оговорённую ничью очевидной. После возврата другая операция может продолжаться: жизненный цикл проигравшей принадлежит вызывающему.

**Частая ошибка**

Один await WhenAny не сообщает об исключении выбранной внутренней задачи.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task07
{
    public static async Task<string> FirstAsync(Task<string> first, Task<string> second)
    {
        ArgumentNullException.ThrowIfNull(first); ArgumentNullException.ThrowIfNull(second);
        if (first.IsCompleted) return await first; if (second.IsCompleted) return await second; return await await Task.WhenAny(first, second);
    }
    public static async Task Run()
    {
        var loser = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously); Check.Equal("B", await FirstAsync(loser.Task, Task.FromResult("B"))); Check.True(!loser.Task.IsCompleted); loser.SetResult("later"); Check.Equal("later", await loser.Task);
        Check.Equal("A", await FirstAsync(Task.FromResult("A"), Task.FromResult("B")));
        var a = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously); var b = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously); var race = FirstAsync(a.Task, b.Task); a.SetResult("first"); Check.Equal("first", await race); b.SetResult("second"); await b.Task;
        await Check.ThrowsAsync<FormatException>(() => FirstAsync(Task.FromException<string>(new FormatException()), Task.FromResult("ok")));
        await Check.ThrowsAsync<OperationCanceledException>(() => FirstAsync(Task.FromCanceled<string>(new CancellationToken(true)), Task.FromResult("ok")));
    }
}
```

**Исполняемые проверки**

- Победа второго и первого
- Обе готовы: приоритет first
- Ошибка/отмена победителя
- Проигравший остаётся управляемым

<a id="15-08"></a>
## 15-08. Граница ожидаемой ошибки

Поместить вызов и await read внутрь try/catch FormatException.

**Ход решения**

Исключение может возникнуть до возврата Task или при наблюдении faulted Task после ожидания. Оба варианта лежат внутри try. Флаг успеха отделяет настоящий ноль от ошибки формата. Отмена не является неверным числом, поэтому не маскируется как false. Gate в тесте позволяет поместить ошибку строго после возврата внешнего Task.

**Частая ошибка**

try вокруг одного read() без await не перехватывает будущее исключение асинхронной операции.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task08
{
    public static async Task<(bool Success, int Value)> TryReadAsync(Func<Task<int>> read)
    {
        ArgumentNullException.ThrowIfNull(read); try { return (true, await read()); } catch (FormatException) { return (false, 0); }
    }
    public static async Task Run()
    {
        Check.Equal((true, 0), await TryReadAsync(() => Task.FromResult(0))); Check.Equal((false, 0), await TryReadAsync(() => throw new FormatException()));
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); var pending = TryReadAsync(() => gate.Task); Check.True(!pending.IsCompleted); gate.SetException(new FormatException()); Check.Equal((false, 0), await pending);
        await Check.ThrowsAsync<IOException>(() => TryReadAsync(() => Task.FromException<int>(new IOException())));
        await Check.ThrowsAsync<OperationCanceledException>(() => TryReadAsync(() => Task.FromCanceled<int>(new CancellationToken(true)))); await Check.ThrowsAsync<ArgumentNullException>(() => TryReadAsync(null!));
    }
}
```

**Исполняемые проверки**

- Успешный ноль
- Синхронная и поздняя FormatException
- IOException и отмена распространяются
- null read

<a id="15-09"></a>
## 15-09. Подсчёт байтов без владения потоком

В цикле прибавлять возвращённый ReadAsync размер, выйти при 0.

**Ход решения**

Поток вправе вернуть меньше запрошенного и до EOF. Размер буфера не является числом прочитанных данных. Чтение продолжается до явного нуля. Использование только ReadAsync поддерживает непозиционируемый источник. Владение отделено от использования: input создаёт и освобождает вызывающий. Память O(1), время O(B).

**Частая ошибка**

Length-Position не работает у всех потоков и вообще не выполняет требуемого чтения.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task09
{
    public static async Task<long> CountBytesAsync(Stream input, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(input); token.ThrowIfCancellationRequested(); var buffer = new byte[4]; long count = 0; int read;
        while ((read = await input.ReadAsync(buffer.AsMemory(), token)) != 0) count = checked(count + read); return count;
    }
    private sealed class NoSeek : MemoryStream
    {
        public NoSeek(byte[] bytes) : base(bytes) { }
        public override bool CanSeek => false;
        public override long Length => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin loc) => throw new NotSupportedException();
    }
    public static async Task Run()
    {
        using var stream = new MemoryStream(new byte[7]); stream.Position = 2; Check.Equal(5L, await CountBytesAsync(stream)); Check.True(stream.CanRead); Check.Equal(0L, await CountBytesAsync(stream));
        using var noSeek = new NoSeek(new byte[9]); Check.Equal(9L, await CountBytesAsync(noSeek));
        using var cancelled = new MemoryStream(new byte[2]); await Check.ThrowsAsync<OperationCanceledException>(() => CountBytesAsync(cancelled, new CancellationToken(true))); Check.Equal(0L, cancelled.Position);
        await Check.ThrowsAsync<ArgumentNullException>(() => CountBytesAsync(null!));
    }
}
```

**Исполняемые проверки**

- Остаток от текущей позиции
- EOF и пустота
- Поток остаётся открыт
- Непозиционируемый поток и отмена

<a id="15-10"></a>
## 15-10. Тест жизненного цикла сохранения

Связать завершение с await и наблюдать два состояния до/после gate.

**Ход решения**

Проверка только после завершения save пропустила бы раннее уведомление. До SetResult требуем незавершённый внешний Task и ноль вызовов. Ошибочная функция выдаёт CompletedTask и уже меняет счётчик, поэтому тест различает её детерминированно. Завершаем также gate ошибочного варианта, чтобы не оставлять незавершённую тестовую работу.

**Частая ошибка**

Нельзя доказывать правильность с помощью «подождём 100 мс»: скорость исполнения не является контрактом.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task10
{
    public static async Task SaveThenNotifyAsync(Func<Task> save, Action notify)
    {
        ArgumentNullException.ThrowIfNull(save); ArgumentNullException.ThrowIfNull(notify); await save(); notify();
    }
    private static Task Broken(Func<Task> save, Action notify) { _ = save(); notify(); return Task.CompletedTask; }
    public static async Task Run()
    {
        var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int notices = 0; var pending = SaveThenNotifyAsync(() => gate.Task, () => notices++); Check.True(!pending.IsCompleted); Check.Equal(0, notices); gate.SetResult(); await pending; Check.Equal(1, notices);
        var badGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int early = 0; var bad = Broken(() => badGate.Task, () => early++); Check.True(bad.IsCompleted); Check.Equal(1, early); badGate.SetResult(); await badGate.Task;
        await Check.ThrowsAsync<IOException>(() => SaveThenNotifyAsync(() => Task.FromException(new IOException()), () => notices++)); Check.Equal(1, notices);
        int saves = 0; await Check.ThrowsAsync<ArgumentNullException>(() => SaveThenNotifyAsync(() => { saves++; return Task.CompletedTask; }, null!)); Check.Equal(0, saves);
    }
}
```

**Исполняемые проверки**

- До/после gate
- Неверный вариант реально обнаружен
- Ошибка сохранения подавляет notify
- Проверка зависимостей до работы

<a id="15-11"></a>
## 15-11. Ограниченное число операций

Один semaphore на вызов; локальная async-обёртка для каждого значения.

**Ход решения**

WaitAsync должен быть до try, чтобы Release выполнялся только после захвата слота. Внутри try ожидаем work; finally возвращает слот при любом завершении. WhenAll удерживает метод до конца всех обёрток, поэтому semaphore не уничтожается раньше времени. Тест закрывает первые слоты управляемыми gate и проверяет фактические запуски, не измеряя секунды.

**Частая ошибка**

Release только на успешном пути приводит к зависанию последующих элементов после ошибки.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task11
{
    public static async Task<int[]> MapBoundedAsync(int[] values, int maximum, Func<int, Task<int>> work, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(values); ArgumentNullException.ThrowIfNull(work); if (maximum < 1 || maximum > 100) throw new ArgumentOutOfRangeException(nameof(maximum)); if (values.Length > 1000) throw new ArgumentOutOfRangeException(nameof(values)); token.ThrowIfCancellationRequested();
        using var semaphore = new SemaphoreSlim(maximum);
        async Task<int> One(int value) { await semaphore.WaitAsync(token); try { return await work(value); } finally { semaphore.Release(); } }
        return await Task.WhenAll(values.Select(One));
    }
    public static async Task Run()
    {
        var gates = new[] { new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously), new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously) };
        var startedThird = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); int starts = 0;
        var pending = MapBoundedAsync(new[] { 1, 2, 3 }, 2, value => { Interlocked.Increment(ref starts); if (value == 3) { startedThird.SetResult(); return Task.FromResult(30); } return gates[value - 1].Task; });
        Check.Equal(2, Volatile.Read(ref starts)); gates[1].SetResult(20); await startedThird.Task; Check.Equal(3, Volatile.Read(ref starts)); gates[0].SetResult(10); Check.SequenceEqual(new[] { 10, 20, 30 }, await pending);
        int afterFault = 0; await Check.ThrowsAsync<IOException>(() => MapBoundedAsync(new[] { 1, 2 }, 1, value => { afterFault++; return value == 1 ? Task.FromException<int>(new IOException()) : Task.FromResult(2); })); Check.Equal(2, afterFault);
        int cancelledCalls = 0; await Check.ThrowsAsync<OperationCanceledException>(() => MapBoundedAsync(new[] { 1 }, 1, x => { cancelledCalls++; return Task.FromResult(x); }, new CancellationToken(true))); Check.Equal(0, cancelledCalls);
        await Check.ThrowsAsync<ArgumentOutOfRangeException>(() => MapBoundedAsync(Array.Empty<int>(), 0, Task.FromResult));
    }
}
```

**Исполняемые проверки**

- До открытия gate запущены ровно два
- Результаты сохраняют исходный порядок
- Ошибка освобождает слот для следующего
- Предварительная отмена и неверные параметры

<a id="15-12"></a>
## 15-12. Ограниченные повторные попытки

Ограничить цикл и фильтровать catch по номеру оставшейся попытки.

**Ход решения**

Счётчик попыток включает первый запуск. Catch с условием attempt<attempts-1 повторяет только когда остаётся разрешённый запуск. Перед следующим кругом отмена проверяется заново. Отмена, случившаяся в обработчике IOException, не должна привести к ещё одной операции. Без задержек тесты полностью детерминированы.

**Частая ошибка**

catch(Exception) повторяет ошибки формата и программные дефекты, которые повтором не исправляются.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task12
{
    public static async Task<string> RetryAsync(Func<CancellationToken, Task<string>> load, int attempts, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(load); if (attempts < 1 || attempts > 5) throw new ArgumentOutOfRangeException(nameof(attempts));
        for (int attempt = 0; ; attempt++) { token.ThrowIfCancellationRequested(); try { return await load(token); } catch (IOException) when (attempt < attempts - 1) { } }
    }
    public static async Task Run()
    {
        int calls = 0; Check.Equal("ok", await RetryAsync(_ => ++calls < 3 ? Task.FromException<string>(new IOException()) : Task.FromResult("ok"), 3)); Check.Equal(3, calls);
        calls = 0; await Check.ThrowsAsync<IOException>(() => RetryAsync(_ => { calls++; throw new IOException(); }, 2)); Check.Equal(2, calls);
        calls = 0; await Check.ThrowsAsync<FormatException>(() => RetryAsync(_ => { calls++; throw new FormatException(); }, 5)); Check.Equal(1, calls);
        using var cts = new CancellationTokenSource(); calls = 0; await Check.ThrowsAsync<OperationCanceledException>(() => RetryAsync(_ => { calls++; cts.Cancel(); throw new IOException(); }, 3, cts.Token)); Check.Equal(1, calls);
        await Check.ThrowsAsync<ArgumentOutOfRangeException>(() => RetryAsync(_ => Task.FromResult("x"), 0));
    }
}
```

**Исполняемые проверки**

- Третий запуск успешен
- Последняя IOException распространяется
- FormatException без повтора
- Отмена между попытками и предел attempts

<a id="15-13"></a>
## 15-13. Асинхронный поток непустых строк

Асинхронный итератор хранит положение между yield, проверяет token перед каждым чтением.

**Ход решения**

IAsyncEnumerable представляет серию будущих элементов, а не один Task массива. Тело итератора начинается при продвижении, поэтому и null-ошибка возникает в этот момент. await foreach последовательно ожидает элементы. Атрибут EnumeratorCancellation связывает параметр токена с отменой перечислителя. Проверка вручную показывает ленивую границу между первой строкой и остальным входом.

**Частая ошибка**

Если сначала ReadToEndAsync, источник уже прочитан полностью до выдачи первого элемента.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task13
{
    public static async IAsyncEnumerable<string> NonBlankLinesAsync(TextReader reader, [EnumeratorCancellation] CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(reader);
        while (true) { token.ThrowIfCancellationRequested(); var line = await reader.ReadLineAsync(token); if (line is null) yield break; if (!string.IsNullOrWhiteSpace(line)) yield return line; }
    }
    public static async Task Run()
    {
        using var reader = new StringReader("\n a \n \nb\n"); var result = new List<string>(); await foreach (var line in NonBlankLinesAsync(reader)) result.Add(line); Check.SequenceEqual(new[] { " a ", "b" }, result); Check.Equal("", reader.ReadToEnd());
        using var cts = new CancellationTokenSource(); using var remaining = new StringReader("first\nsecond");
        await using (var iterator = NonBlankLinesAsync(remaining, cts.Token).GetAsyncEnumerator()) { Check.True(await iterator.MoveNextAsync()); Check.Equal("first", iterator.Current); cts.Cancel(); await Check.ThrowsAsync<OperationCanceledException>(async () => { await iterator.MoveNextAsync(); }); }
        Check.Equal("second", remaining.ReadLine());
        await Check.ThrowsAsync<ArgumentNullException>(async () => { await foreach (var line in NonBlankLinesAsync(null!)) { _ = line; } });
    }
}
```

**Исполняемые проверки**

- Пробельные строки пропускаются без изменения остальных
- Первый элемент доступен отдельно
- Отмена после первой выдачи
- Reader остаётся открыт и null проверен

<a id="15-14"></a>
## 15-14. Освобождение асинхронного ресурса

Создать await using scope, затем проверить отмену и await work.

**Ход решения**

Проверка зависимостей вне области владения делает политику неверной конфигурации явной. Отмена находится внутри await using, поэтому уже принятый ресурс освобождается даже без старта work. Возвращаемый Task охватывает также DisposeAsync: простого вызова DisposeAsync без ожидания недостаточно. Тестовый lease с закрытым gate доказывает это отдельно.

**Частая ошибка**

using для IAsyncDisposable или забытый await не гарантируют завершения асинхронного освобождения.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task14
{
    public static async Task<int> RunWithLeaseAsync(IAsyncDisposable lease, Func<CancellationToken, Task<int>> work, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(lease); ArgumentNullException.ThrowIfNull(work);
        await using (lease) { token.ThrowIfCancellationRequested(); return await work(token); }
    }
    private sealed class Lease : IAsyncDisposable
    {
        public int Count { get; private set; } public Task Disposal { get; set; } = Task.CompletedTask;
        public ValueTask DisposeAsync() { Count++; return new ValueTask(Disposal); }
    }
    public static async Task Run()
    {
        var success = new Lease(); Check.Equal(7, await RunWithLeaseAsync(success, _ => Task.FromResult(7))); Check.Equal(1, success.Count);
        var fault = new Lease(); await Check.ThrowsAsync<IOException>(() => RunWithLeaseAsync(fault, _ => Task.FromException<int>(new IOException()))); Check.Equal(1, fault.Count);
        int starts = 0; var cancelled = new Lease(); await Check.ThrowsAsync<OperationCanceledException>(() => RunWithLeaseAsync(cancelled, _ => { starts++; return Task.FromResult(0); }, new CancellationToken(true))); Check.Equal(0, starts); Check.Equal(1, cancelled.Count);
        var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); var delayed = new Lease { Disposal = gate.Task }; var pending = RunWithLeaseAsync(delayed, _ => Task.FromResult(9)); Check.True(!pending.IsCompleted); Check.Equal(1, delayed.Count); gate.SetResult(); Check.Equal(9, await pending);
        var invalid = new Lease(); await Check.ThrowsAsync<ArgumentNullException>(() => RunWithLeaseAsync(invalid, null!)); Check.Equal(0, invalid.Count);
    }
}
```

**Исполняемые проверки**

- Успех и ошибка work освобождают ровно раз
- Предварительная отмена без вызова work
- Метод ждёт DisposeAsync
- Неверная конфигурация не принимает владение

<a id="15-15"></a>
## 15-15. Асинхронный импорт проверенного снимка

Отделить проверку снимка от необратимых действий; связать Task со всеми сохранениями.

**Ход решения**

DeserializeAsync и валидация должны закончиться раньше первого callback, иначе неверная последняя строка оставит лишние записи. После проверки последовательный await задаёт порядок и не допускает одновременных save. Ошибки уже выполненных внешних действий метод отменить не может, поэтому контракт честно допускает частичный эффект при сбое save. Отмена наблюдается на границах действий, а поток остаётся у вызывающего.

**Частая ошибка**

WhenAll сохранений нарушает оговорённый последовательный порядок и увеличивает число действий после первой ошибки.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task15
{
    public sealed class Item { public int Id { get; set; } public string? Name { get; set; } }
    public static async Task<int> ImportAsync(Stream source, Func<Item, CancellationToken, Task> save, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(save); token.ThrowIfCancellationRequested();
        var items = await JsonSerializer.DeserializeAsync<Item?[]>(source, cancellationToken: token) ?? throw new JsonException("Array required."); var ids = new HashSet<int>();
        foreach (var item in items) if (item is null || item.Id <= 0 || string.IsNullOrWhiteSpace(item.Name) || !ids.Add(item.Id)) throw new JsonException("Invalid item.");
        foreach (var item in items) { token.ThrowIfCancellationRequested(); await save(item!, token); } token.ThrowIfCancellationRequested(); return items.Length;
    }
    private static MemoryStream Json(string text) => new(Encoding.UTF8.GetBytes(text));
    public static async Task Run()
    {
        using var source = Json("[{\"Id\":2,\"Name\":\"B\"},{\"Id\":1,\"Name\":\"A\"}]"); var seen = new List<int>();
        Check.Equal(2, await ImportAsync(source, (item, token) => { Check.Equal(CancellationToken.None, token); seen.Add(item.Id); return Task.CompletedTask; })); Check.SequenceEqual(new[] { 2, 1 }, seen); Check.True(source.CanRead);
        foreach (var text in new[] { "[{\"Id\":1,\"Name\":\"A\"},{\"Id\":1,\"Name\":\"B\"}]", "[{\"Id\":1,\"Name\":\"A\"},null]" })
        {
            using var invalid = Json(text); int calls = 0; await Check.ThrowsAsync<JsonException>(() => ImportAsync(invalid, (_, _) => { calls++; return Task.CompletedTask; })); Check.Equal(0, calls);
        }
        using var failure = Json("[{\"Id\":1,\"Name\":\"A\"},{\"Id\":2,\"Name\":\"B\"},{\"Id\":3,\"Name\":\"C\"}]"); var saved = new List<int>();
        await Check.ThrowsAsync<IOException>(() => ImportAsync(failure, (item, _) => { if (item.Id == 2) throw new IOException(); saved.Add(item.Id); return Task.CompletedTask; })); Check.SequenceEqual(new[] { 1 }, saved);
        using var empty = Json("[]"); Check.Equal(0, await ImportAsync(empty, (_, _) => Task.CompletedTask));
        using var cancel = Json("[]"); await Check.ThrowsAsync<OperationCanceledException>(() => ImportAsync(cancel, (_, _) => Task.CompletedTask, new CancellationToken(true))); Check.Equal(0L, cancel.Position);
    }
}
```

**Исполняемые проверки**

- Порядок и количество сохранений
- Дубликат и неверная последняя запись без save
- Ошибка save прекращает дальнейшие
- Отмена, пустой снимок и незакрытый source

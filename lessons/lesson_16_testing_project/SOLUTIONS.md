# Урок 16. Отладка, тестирование и структура приложения: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="16-01"></a>
## 16-01. Границы и мутация условия

Нужны значения -1, 0, 59, 60, 100, 101; порог 60 уничтожает мутант со строгим сравнением.

**Ход решения**

Сначала отделяем допустимость числа от результата проверки. Затем берём соседей порога: 59 и 60. Для ограничения диапазона важны ровно допустимые края и по одному соседу за ними. Все числа внутри класса проверять по отдельности необязательно; один специально выбранный контрпример полезнее десятка случайных значений.

**Частая ошибка**

Проверять только 70: и правильная функция, и ошибочное строгое сравнение дадут true.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
namespace Course.Lesson16;

public static class Task01
{
    public static bool IsPassing(int score)
    {
        if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));
        return score >= 60;
    }
    public static void Run()
    {
        Check.True(!IsPassing(0)); Check.True(!IsPassing(59));
        Check.True(IsPassing(60)); Check.True(IsPassing(100));
        Check.Throws<ArgumentOutOfRangeException>(() => IsPassing(-1));
        Check.Throws<ArgumentOutOfRangeException>(() => IsPassing(101));
        Func<int,bool> mutant = score => score > 60;
        Check.True(mutant(60) != IsPassing(60));
    }
}
```

**Исполняемые проверки**

- Нижний и верхний допустимые края
- Соседи порога 59/60
- Исключения за диапазоном
- Мутант &gt;60 даёт другой результат

<a id="16-02"></a>
## 16-02. Общее состояние тестов

Статическое поле разделяется всеми вызовами. Каждый новый объект со своим полем обеспечивает независимый старт.

**Ход решения**

Два Next после одного Reset изменяют одно поле: 0→1→2. Во второй части между вызовами есть сброс, поэтому оба наблюдения равны 1. Экземпляр Counter принадлежит одному тесту; отсутствие общей переменной устраняет зависимость от порядка и облегчает параллельный запуск.

**Частая ошибка**

Считать, что static означает неизменяемое состояние.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
namespace Course.Lesson16;

public static class Task02
{
    private static class Shared
    {
        private static int value;
        public static int Next() => ++value;
        public static void Reset() => value = 0;
    }
    public sealed class Counter { private int value; public int Next() => ++value; }
    public static (int,int) SharedPair() { var shared = new Counter(); return (shared.Next(), shared.Next()); }
    public static void Run()
    {
        Shared.Reset(); Check.Equal((1,2),(Shared.Next(),Shared.Next()));
        Shared.Reset(); int observed=Shared.Next(); Shared.Reset();
        Check.Equal((1,1),(observed,Shared.Next()));
        Check.Equal((1,2), SharedPair());
        var first = new Counter(); var second = new Counter();
        Check.Equal(1,first.Next()); Check.Equal(1,second.Next()); Check.Equal(2,first.Next());
    }
}
```

**Исполняемые проверки**

- Общий объект даёт 1,2
- Два отдельных экземпляра дают 1,1
- Повторные обращения к первому экземпляру сохраняют его состояние

<a id="16-03"></a>
## 16-03. Табличный тест парсера порта

Строгую грамматику проверяем до принятия числа; TryParse с обычными настройками мог бы принять пробелы или знак.

**Ход решения**

Накопитель умножается на 10 и получает следующую цифру. Максимум пять цифр исключает переполнение int, а итоговая граница 65535 проверяется отдельно. Выходной параметр присваивается принятому числу только после всех проверок, так что отказ не оставляет промежуточного значения.

**Частая ошибка**

Использовать char.IsDigit: он принимает больше символов, чем ASCII-грамматика.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
namespace Course.Lesson16;

public static class Task03
{
    public static bool TryPort(string? text, out int port)
    {
        port = 0;
        if (text is null || text.Length is < 1 or > 5) return false;
        int value = 0;
        foreach (char c in text) { if (c is < '0' or > '9') return false; value = value * 10 + c - '0'; }
        if (value is < 1 or > 65535) return false;
        port = value; return true;
    }
    public static void Run()
    {
        (string? Text,bool Success,int Port)[] cases = {
            ("1",true,1),("00443",true,443),("65535",true,65535),("0",false,0),
            ("65536",false,0),("+80",false,0),(" 80",false,0),("８０",false,0),
            ("123456",false,0),("",false,0),(null,false,0) };
        foreach (var row in cases) { Check.Equal(row.Success,TryPort(row.Text,out int port)); Check.Equal(row.Port,port); }
        foreach (var row in new (string Text,bool Success,int Port)[] {
            ("00001",true,1),("00000",false,0),("000001",false,0),("80 ",false,0),("8a0",false,0) })
        {
            Check.Equal(row.Success,TryPort(row.Text,out int port)); Check.Equal(row.Port,port);
        }
    }
}
```

**Исполняемые проверки**

- Нули, края 1 и 65535
- Выход за верхнюю границу
- Знак, пробел, Unicode-цифры, null
- На отказе out всегда 0
- 00001 допустимо, но000001 отклоняется по длине; out на отказе0
- 80 с пробелом справа и8a0 отклоняются

<a id="16-04"></a>
## 16-04. Регрессия переполнения процента

Нужно начать вычисление с double: 100.0 * done / total, предварительно проверив вход.

**Ход решения**

Литерал 100 оставляет умножение и деление целыми. Поэтому 1/3 теряет дробь, а 100*int.MaxValue выходит за диапазон int до преобразования результата. Замена типа слева ничего не исправляет. Проверки дополняются нулём выполненных пунктов и некорректным total; сравнение дроби использует допуск.

**Частая ошибка**

Писать (double)(100 * done): преобразование происходит уже после переполнения.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
namespace Course.Lesson16;

public static class Task04
{
    public static double Percent(int done,int total)
    {
        if (total <= 0 || done < 0 || done > total) throw new ArgumentOutOfRangeException(nameof(total));
        return 100.0 * done / total;
    }
    public static void Run()
    {
        Check.Near(100.0/3,Percent(1,3)); Check.Near(100,Percent(int.MaxValue,int.MaxValue));
        Check.Near(0,Percent(0,1)); Check.Near(25,Percent(1,4));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(1,0));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(3,2));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(-1,2));
        Check.Throws<ArgumentOutOfRangeException>(()=>Percent(0,-1));
        Func<int,int,double> truncated=(done,total)=>100*done/total;
        Func<int,int,double> overflowed=(done,total)=>unchecked(100*done)/total;
        Check.True(Math.Abs(truncated(1,3)-Percent(1,3))>0.1);
        Check.True(overflowed(int.MaxValue,int.MaxValue)!=Percent(int.MaxValue,int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Дробный результат
- Верхний предел int
- Ноль выполненных
- Нулевой total и done&gt;total
- Отрицательные done иtotal
- Два исполняемых ошибочных варианта отдельно показывают усечение и переполнение

<a id="16-05"></a>
## 16-05. Сообщение после сохранения

Сначала выполняется операция, затем подтверждение. Исключение автоматически прерывает последующие строки.

**Ход решения**

TextWriter позволяет наблюдать вывод без глобальной подмены Console. Счётчик в callback доказывает, что повтора записи не было. В ошибочном сценарии проверяем сразу три свойства: тип ошибки, отсутствие ложного подтверждения, ровно одну попытку. Такой тест проверяет контракт взаимодействия, а не конкретный текст исходника. Проверка обеих зависимостей до первого эффекта исключает сохранение при заведомо непригодной конфигурации. Отказ writer означает, что запись уже могла состояться: бездумный retry повторил бы эффект. Тест отдельно различает отказ сохранения и отказ подтверждения.

**Частая ошибка**

Печатать сообщение до save либо ловить IOException и продолжать как при успехе.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
namespace Course.Lesson16;

public static class Task05
{
    public static void Commit(Action save,TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(save);
        ArgumentNullException.ThrowIfNull(output);
        save();
        output.WriteLine("Сохранено");
    }
    private sealed class FailingWriter : TextWriter
    {
        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
        public override void WriteLine(string? value) => throw new IOException("output");
    }
    public static void Run()
    {
        int calls=0; using var success=new StringWriter();
        Check.Throws<ArgumentNullException>(()=>Commit(()=>calls++,null!));
        Check.Equal(0,calls);
        Check.Throws<ArgumentNullException>(()=>Commit(null!,success));
        Check.Equal("",success.ToString());
        Commit(()=>calls++,success); Check.Equal(1,calls); Check.Equal("Сохранено"+Environment.NewLine,success.ToString());
        success.Write("tail"); Check.True(success.ToString().EndsWith("tail",StringComparison.Ordinal));
        using var failure=new StringWriter(); calls=0;
        Check.Throws<IOException>(()=>Commit(()=>{calls++;throw new IOException("disk");},failure));
        Check.Equal(1,calls); Check.Equal("",failure.ToString());
        calls=0;
        Check.Throws<FormatException>(()=>Commit(()=>{calls++;throw new FormatException("data");},failure));
        Check.Equal(1,calls); Check.Equal("",failure.ToString());
        using var outputFailure=new FailingWriter(); calls=0;
        Check.Throws<IOException>(()=>Commit(()=>calls++,outputFailure));
        Check.Equal(1,calls);
    }
}
```

**Исполняемые проверки**

- Успешный callback вызывается один раз
- Отказ доходит до вызывающего кода
- При отказе нет сообщения об успехе
- null output не запускает save; null save не пишет вывод
- Произвольная ошибка callback не маскируется
- Ошибка writer после одного save не вызывает повтор
- Writer остаётся открыт

<a id="16-06"></a>
## 16-06. Часы как явная зависимость

Передаём «сегодня» явно и сравниваем DateOnly. Получение системной даты остаётся на границе приложения.

**Ход решения**

Причина нестабильности — скрытый ввод из часов. Чистая функция получает обе даты, поэтому тест фиксирует их самостоятельно. Переход года проверяет, что сравниваются даты целиком, а не только номер дня. Граница «срок сегодня» уточняет строгость сравнения.

**Частая ошибка**

Использовать DateTime.Now в ожидаемом результате теста и тем самым повторить скрытую зависимость.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
namespace Course.Lesson16;

public static class Task06
{
    public static bool IsOverdue(DateOnly due,DateOnly today)=>due<today;
    public static void Run()
    {
        var today=new DateOnly(2026,1,1);
        Check.True(IsOverdue(new(2025,12,31),today)); Check.True(!IsOverdue(today,today));
        Check.True(!IsOverdue(new(2026,1,2),today));
        Check.True(IsOverdue(new(2024,2,29),new(2024,3,1)));
    }
}
```

**Исполняемые проверки**

- Вчера/сегодня/завтра
- Переход года
- 29 февраля високосного года

<a id="16-07"></a>
## 16-07. Место наблюдения async-ошибки

Создание уже завершившейся с ошибкой Task не выбрасывает её ошибку синхронно; await наблюдает её.

**Ход решения**

Work возвращает объект задачи. До await выполняется добавление A. В await ошибка IOException распространяется в catch и добавляет B; управление не возвращается к строке с X. Это объясняет, почему тест асинхронного метода должен ожидать задачу, а не только вызывать метод.

**Частая ошибка**

Проверять лишь факт возврата Task и оставлять ошибку ненаблюдаемой.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
namespace Course.Lesson16;

public static class Task07
{
    public static Task<int> Work()=>Task.FromException<int>(new IOException("read"));
    public static async Task<string> Observe()
    {
        string trace="";
        try { Task<int> pending=Work(); trace+="A"; await pending; trace+="X"; }
        catch(IOException) { trace+="B"; }
        return trace;
    }
    public static void Run()
    {
        Task<int> pending=Work(); Check.True(pending.IsFaulted);
        Check.Throws<IOException>(()=>pending.GetAwaiter().GetResult());
        Check.Equal("AB",Observe().GetAwaiter().GetResult());
    }
}
```

**Исполняемые проверки**

- Точная последовательность AB
- Синхронный этап Work действительно возвращает faulted Task

<a id="16-08"></a>
## 16-08. Свойства сортировки вместо одного примера

Отдельно проверяем порядок, отдельно — частоты значений. Одна сумма не описывает мультимножество.

**Ход решения**

Сначала размеры должны совпасть. Затем каждый сосед в result проверяется на неубывание. Словарь частот исходника увеличивает счётчик, а результат уменьшает его; отсутствие значения или лишнее вхождение немедленно дают false. Нулевые остатки следуют из равенства длин. Сложность O(n) по времени в среднем, O(k) по памяти.

**Частая ошибка**

Принять [2,3] вместо [1,4], потому что сумма и длина совпадают.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
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
```

**Исполняемые проверки**

- Пустые массивы
- Повторяющиеся значения
- Подмена при той же сумме
- Неправильный порядок
- int.MinValue и int.MaxValue
- Одинаковое множество при разной кратности1/2 не принимается
- Оба входных массива неизменны

<a id="16-09"></a>
## 16-09. Переводы строк в сравнении вывода

Заменяем сначала CRLF, затем оставшиеся CR. Пустые строки не трогаем.

**Ход решения**

Если сначала заменить CR отдельно, пара CRLF превратится в два LF и добавит лишнюю строку. Trim меняет наблюдаемый контракт, убирая пробелы и завершающий перевод. Проверка должна разрешать только оговорённую разницу платформ, а не любые различия форматирования.

**Частая ошибка**

Нормализовать через Split с RemoveEmptyEntries и потерять пустые строки.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
namespace Course.Lesson16;

public static class Task09
{
    public static string NormalizeNewlines(string text)=>text.Replace("\r\n","\n").Replace("\r","\n");
    public static void Run()
    {
        Check.Equal("A\n\nB\n",NormalizeNewlines("A\r\n\r\nB\r\n"));
        Check.Equal(" A\nB ",NormalizeNewlines(" A\rB "));
        Check.Equal("",NormalizeNewlines("")); Check.Equal("A\n",NormalizeNewlines("A\n"));
        Check.True(NormalizeNewlines("A\n\nB")!=NormalizeNewlines("A\nB"));
    }
}
```

**Исполняемые проверки**

- CRLF и одиночный CR
- Сохранение двойного перевода
- Сохранение пробелов и финального LF
- Пустая строка

<a id="16-10"></a>
## 16-10. Грамматика двух CLI-опций

Парсер проходит аргументы один раз, хранит два флага наличия и потребляет значение после --limit.

**Ход решения**

Грамматику разбираем раньше выполнения приложения. seenLimit отделён от значения limit: явный --limit 10 нельзя перепутать с отсутствием опции. Для --reverse значение false может смениться только на true, поэтому само reverse одновременно отмечает уже встреченный флаг. Это не две независимые переменные seenReverse/reverse. Число состоит из ASCII-цифр и проходит диапазон; ведущие нули не меняют его. Время O(суммарной длины аргументов), дополнительная память O(1).

**Частая ошибка**

При повторе незаметно оставлять последнее значение, хотя контракт требует отказа.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
namespace Course.Lesson16;

public static class Task10
{
    public sealed record Options(int Limit,bool Reverse);
    public static Options Parse(string[] args)
    {
        int limit=10; bool reverse=false,seenLimit=false;
        for(int i=0;i<args.Length;i++)
        {
            if(args[i]=="--reverse"&&!reverse){reverse=true;continue;}
            if(args[i]=="--limit"&&!seenLimit&&i+1<args.Length)
            {
                string value=args[++i]; seenLimit=true;
                if(value.Length==0||value.Any(c=>c is < '0' or > '9')||!int.TryParse(value,out limit)||limit is < 1 or > 100)
                    throw new ArgumentException("Неверный limit.");
                continue;
            }
            throw new ArgumentException("Неизвестная или повторная опция.");
        }
        return new(limit,reverse);
    }
    public static void Run()
    {
        Check.Equal(new Options(10,false),Parse([]));
        Check.Equal(new Options(3,true),Parse(["--reverse","--limit","3"]));
        Check.Equal(new Options(1,true),Parse(["--limit","1","--reverse"]));
        Check.Equal(100,Parse(["--limit","100"]).Limit);
        Check.Equal(1,Parse(["--limit",new string('0',100)+"1"]).Limit);
        Check.Equal(new Options(10,true),Parse(["--reverse"]));
        string[][] wrong=[["--reverse","--reverse"],["--limit"],["--limit","0"],["--limit","101"],["--limit","+2"],["other"],["--limit","10","--limit","2"]];
        foreach(var row in wrong)Check.Throws<ArgumentException>(()=>Parse(row));
        foreach(string value in new[]{""," 2","2 ","２","2147483648","--reverse"})
            Check.Throws<ArgumentException>(()=>Parse(["--limit",value]));
    }
}
```

**Исполняемые проверки**

- Оба порядка опций
- Значения по умолчанию
- Повтор, неизвестная опция и пропущенное значение
- Границы 1/100 и недопустимые 0/101/+2
- Ведущие нули, включая длинную строку; отдельный --reverse; пустое N, пробелы, Unicode-цифра, переполнение и другая опция вместо N.

<a id="16-11"></a>
## 16-11. Поток вывода вместо Console

Принимаем поток параметром и явно задаём порядок ключей.

**Ход решения**

Dictionary не является контрактом сортировки. Ordinal делает порядок независимым от культуры. StringWriter принадлежит одному тесту, поэтому другие тесты не увидят подменённую консоль. Writer закрывает владелец: метод WriteReport не должен вызывать Dispose на переданном потоке.

**Частая ошибка**

Закрыть writer внутри WriteReport и не дать вызывающему коду дописать данные.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
namespace Course.Lesson16;

public static class Task11
{
    public static void WriteReport(IReadOnlyDictionary<string,int> rows,TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(writer);
        foreach(var row in rows.OrderBy(p=>p.Key,StringComparer.Ordinal))writer.WriteLine($"{row.Key}={row.Value}");
    }
    public static void Run()
    {
        using var writer=new StringWriter(){NewLine="\n"};
        WriteReport(new Dictionary<string,int>{{"b",2},{"a",1}},writer);
        Check.Equal("a=1\nb=2\n",writer.ToString()); writer.Write("tail");
        Check.True(writer.ToString().EndsWith("tail",StringComparison.Ordinal));
        using var empty=new StringWriter(); WriteReport(new Dictionary<string,int>(),empty); Check.Equal("",empty.ToString());
        Check.Throws<ArgumentNullException>(()=>WriteReport(null!,empty));
        Check.Throws<ArgumentNullException>(()=>WriteReport(new Dictionary<string,int>(),null!));
        var culture=System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture=new("tr-TR");
            using var ordinal=new StringWriter(){NewLine="\n"};
            WriteReport(new Dictionary<string,int>{{"ı",4},{"İ",3},{"i",2},{"I",1}},ordinal);
            Check.Equal("I=1\ni=2\nİ=3\nı=4\n",ordinal.ToString());
        }
        finally{System.Globalization.CultureInfo.CurrentCulture=culture;}
    }
}
```

**Исполняемые проверки**

- Сортировка a перед b
- Пустой словарь без строки-заглушки
- Поток остаётся пригодным для дальнейшей записи
- Ordinal-порядок I/i/İ/ı сохраняется при tr-TR
- Обе null-зависимости отклоняются

<a id="16-12"></a>
## 16-12. Модель очереди и ошибочный стек

Список служит простой моделью: добавление в конец, чтение и удаление нулевого элемента.

**Ход решения**

Проверка после каждого действия локализует момент расхождения. Разные добавленные значения делают порядок наблюдаемым: одинаковые элементы не отличили бы FIFO от LIFO. Проверка Count обнаруживает потери, а проверка извлечённого значения — ошибочный порядок. Исключение на пустой структуре проверяется после полного опустошения.

**Частая ошибка**

Проверять только итоговый Count=0: стек прошёл бы такой тест.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
namespace Course.Lesson16;

public static class Task12
{
    public static void Run()
    {
        var queue=new Queue<int>(); var model=new List<int>(); var observed=new List<int>();
        int?[] operations=[4,7,null,9,null,null];
        foreach(int? operation in operations)
        {
            if(operation is int value){queue.Enqueue(value);model.Add(value);}
            else{int expected=model[0];model.RemoveAt(0);int actual=queue.Dequeue();Check.Equal(expected,actual);observed.Add(actual);}
            Check.Equal(model.Count,queue.Count);
        }
        Check.SequenceEqual(new[]{4,7,9},observed);
        Check.Throws<InvalidOperationException>(()=>queue.Dequeue());
        var mutant=new Stack<int>();mutant.Push(4);mutant.Push(7);Check.True(mutant.Pop()!=observed[0]);
    }
}
```

**Исполняемые проверки**

- Размер после каждой операции
- Точная последовательность 4,7,9
- Пустое извлечение
- Стек отличается на первом извлечении

<a id="16-13"></a>
## 16-13. Отсутствие JSON-флага и false

DTO хранит bool?, после десериализации null означает отсутствие обязательного значения.

**Ход решения**

Nullable отделяет false от отсутствия. Оба могут быть представлены разными состояниями модели. Сначала парсер проверяет синтаксис и типы JSON, затем код проверяет предметную обязательность поля. Преобразование всех ошибок в false скрыло бы повреждение файла, поэтому категории ошибок сохранены.

**Частая ошибка**

Заменить отсутствие на false через ?? false и потерять требование обязательности.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
namespace Course.Lesson16;

public static class Task13
{
    public sealed class FlagDto { public bool? Enabled{get;set;} }
    public static bool ReadFlag(string json)
    {
        var dto=System.Text.Json.JsonSerializer.Deserialize<FlagDto>(json,new System.Text.Json.JsonSerializerOptions{PropertyNameCaseInsensitive=true});
        return dto?.Enabled??throw new InvalidDataException("Обязателен enabled.");
    }
    public static void Run()
    {
        Check.True(!ReadFlag("{\"enabled\":false}"));Check.True(ReadFlag("{\"ENABLED\":true}"));
        foreach(string json in new[]{"{}","null","{\"enabled\":null}"})Check.Throws<InvalidDataException>(()=>ReadFlag(json));
        Check.Throws<System.Text.Json.JsonException>(()=>ReadFlag("{\"enabled\":\"yes\"}"));
        Check.Throws<ArgumentNullException>(()=>ReadFlag(null!));
        foreach(string json in new[]{"{broken","[]","true","{\"enabled\":1}"})
            Check.Throws<System.Text.Json.JsonException>(()=>ReadFlag(json));
        Check.True(ReadFlag("{\"extra\":{\"any\":[]},\"EnAbLeD\":true}"));
    }
}
```

**Исполняемые проверки**

- Явные true и false
- Отсутствующее и null поле
- JSON null
- Неверный тип флага
- Массив/логическое значение вместо объекта и повреждённыйJSON
- null-ссылка отличается от JSON null
- Неизвестный вложенный объект не мешает чтению флага

<a id="16-14"></a>
## 16-14. Пакет обновлений без частичного применения

Сначала валидируем весь пакет с HashSet, после этого отдельным проходом выполняем присваивания.

**Ход решения**

Ввод может оказаться неправильным на последнем элементе. Поэтому изменение stock в первом же проходе нарушает контракт. Промежуточное множество отслеживает повторы, а проверка существования и диапазона ничего не записывает. Только после успешной первой фазы начинается вторая; O(n) времени и O(k) памяти. Это гарантия для проверки входа в одном потоке, без обещания транзакционности при внешних сбоях.

**Частая ошибка**

Изменять первую позицию, затем обнаружить ошибку во второй и оставить половину пакета.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
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
```

**Исполняемые проверки**

- Ошибка второго элемента не меняет первый
- Неизвестный продукт и повтор
- Успешный пакет
- Пустой пакет
- Недопустимые101/null/пустой ключ во второй позиции сохраняют весь снимок
- 101 уникальное обновление отклоняется до записи;100 уникальных принимаются

<a id="16-15"></a>
## 16-15. Приёмка собранного приложения

Приёмка собирает отдельные компоненты: аргументы, бизнес-правила, сериализацию и повторную загрузку.

**Ход решения**

Новый экземпляр CLI между командами исключает ложный успех за счёт памяти процесса. Успешный stats после read доказывает сохранение флага и загрузку. Затем намеренно портим файл в своём временном каталоге: команда должна сообщить об ошибке и сохранить исходные байты для диагностики. Очистка finally нужна и при падении проверки.

**Частая ошибка**

Проверить только библиотеку в памяти и объявить хранение данных работающим.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
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
```

**Исполняемые проверки**

- Добавление и чтение новым экземпляром
- Статистика после загрузки
- Повреждённый JSON даёт код 1
- Повреждённые байты сохранены

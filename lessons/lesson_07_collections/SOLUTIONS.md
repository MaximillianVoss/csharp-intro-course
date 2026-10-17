# Урок 7. Коллекции и выбор структуры данных: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="07-01"></a>
## 07-01. Вставка в изменяемый список

Конструктор List от source создаёт копию; Insert выполняет сдвиг элементов.

**Ход решения**

Массив имеет фиксированную длину, List хранит изменяемое число элементов. При вставке индекс вправе совпадать с Count: после последнего элемента есть позиция для новой записи. Копирование списка int отделяет изменение результата от источника. Стоимость копирования O(n), вставки O(n) из-за сдвига; память O(n).

**Частая ошибка**

Путать Count с Capacity; запрещать вставку на позиции Count; возвращать изменённый source.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
namespace Course.Lesson07;

public static class Task01
{
    public static List<int> InsertCopy(List<int> source, int index, int value)
    {
        List<int> result = new(source);
        result.Insert(index, value);
        return result;
    }
    public static void Run()
    {
        List<int> source = new() { 10, 30 };
        List<int> result = InsertCopy(source, 1, 20);
        Check.SequenceEqual(new[] { 10, 20, 30 }, result);
        Check.SequenceEqual(new[] { 7 }, InsertCopy(new List<int>(), 0, 7));
        Check.SequenceEqual(new[] { 10, 30, 40 }, InsertCopy(source, 2, 40));
        result[0] = 99; Check.SequenceEqual(new[] { 10, 30 }, source);
    }
}
```

**Исполняемые проверки**

- Вставка в середину
- Вставка в пустой
- Добавление в конец
- Источник сохранён и независим

<a id="07-02"></a>
## 07-02. Частоты кодов без учёта регистра

Создайте словарь со StringComparer.OrdinalIgnoreCase и обновляйте значение через TryGetValue.

**Ход решения**

Словарь решает задачу поиска значения по ключу. Компаратор задаётся при создании, поэтому одинаковые по контракту коды попадают в одну запись без изменения исходных строк. TryGetValue сообщает отсутствие и возвращает ноль для нового счётчика. Ожидаемое O(n) времени и O(u) памяти.

**Частая ошибка**

Использовать стандартный компаратор или ToLower без указания культуры; проверять порядок словаря вместо значения по ключу.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
namespace Course.Lesson07;

public static class Task02
{
    public static Dictionary<string, int> CountCodes(string[] codes)
    {
        Dictionary<string, int> counts = new(StringComparer.OrdinalIgnoreCase);
        foreach (string code in codes)
        {
            counts.TryGetValue(code, out int count);
            counts[code] = count + 1;
        }
        return counts;
    }
    public static void Run()
    {
        Dictionary<string, int> result = CountCodes(new[] { "ab", "AB", "x" });
        Check.Equal(2, result["aB"]); Check.Equal(1, result["X"]); Check.Equal(2, result.Count);
        Check.Equal(0, CountCodes(Array.Empty<string>()).Count);
        Check.Equal(2, CountCodes(new[] { "ab", "a-b" }).Count);
    }
}
```

**Исполняемые проверки**

- Разный регистр объединён
- Поиск результата в другом регистре
- Пустой массив
- Дефис отличен от отсутствия дефиса

<a id="07-03"></a>
## 07-03. Уникальные в порядке появления

Добавляйте значение в результат, только если HashSet.Add вернул true.

**Ход решения**

Множество отвечает на вопрос «видели ли значение», а список представляет требуемый порядок. Add одновременно проверяет и добавляет, поэтому отдельный Contains не нужен. При повторении множество не меняется, список тоже. Ожидаемое O(n) времени, O(u) памяти.

**Частая ошибка**

Вернуть перечисление множества и принять его текущий порядок за гарантию; сортировать и потерять порядок появления.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
namespace Course.Lesson07;

public static class Task03
{
    public static List<int> DistinctInOrder(int[] values)
    {
        HashSet<int> seen = new(); List<int> result = new();
        foreach (int value in values) if (seen.Add(value)) result.Add(value);
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 4, 2, 3 }, DistinctInOrder(new[] { 4, 2, 4, 3, 2 }));
        Check.SequenceEqual(new[] { 7 }, DistinctInOrder(new[] { 7, 7, 7 }));
        Check.SequenceEqual(Array.Empty<int>(), DistinctInOrder(Array.Empty<int>()));
        Check.SequenceEqual(new[] { int.MaxValue, int.MinValue }, DistinctInOrder(new[] { int.MaxValue, int.MinValue, int.MaxValue }));
    }
}
```

**Исполняемые проверки**

- Повторы и порядок
- Все одинаковые
- Пустой массив
- Крайние int

<a id="07-04"></a>
## 07-04. Кто следующий в очереди

FIFO извлекает наиболее ранний оставшийся элемент; Peek только просматривает его.

**Ход решения**

После двух Enqueue очередь [10,20]. Dequeue возвращает 10, остаётся [20]. После добавления 30 имеем [20,30]. Peek возвращает 20, но Count остаётся 2. Следующий Dequeue снова возвращает 20. Проверки выполняют тот же фрагмент и подтверждают оставшийся 30.

**Частая ошибка**

Применять порядок стека и ожидать 30; считать Peek удалением.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
namespace Course.Lesson07;

public static class Task04
{
    public static int[] ReplayQueue()
    {
        Queue<int> queue = new();
        queue.Enqueue(10); queue.Enqueue(20);
        int first = queue.Dequeue(); queue.Enqueue(30);
        int next = queue.Peek(); int count = queue.Count;
        int second = queue.Dequeue();
        Check.Equal(30, queue.Peek()); Check.Equal(1, queue.Count);
        return new[] { first, next, count, second };
    }
    public static void Run() => Check.SequenceEqual(new[] { 10, 20, 2, 20 }, ReplayQueue());
}
```

**Исполняемые проверки**

- Точный фрагмент возвращает [10,20,2,20]
- После просмотра число элементов не меняется
- Последний оставшийся элемент равен 30

<a id="07-05"></a>
## 07-05. Баланс трёх видов скобок

Храните открывающие скобки в стеке и требуйте совпадения при закрытии.

**Ход решения**

Вложенная структура закрывается в обратном порядке, поэтому нужен LIFO. Пустой стек при закрытии означает отсутствие пары. Несовпадение типа нельзя исправить последующими символами, поэтому допустим ранний false. В конце незакрытые скобки также дают false. O(n), память O(глубина).

**Частая ошибка**

Считать только количества: ([)] имеет равные количества, но неверную вложенность; вызывать Pop у пустого стека.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
namespace Course.Lesson07;

public static class Task05
{
    public static bool IsBalanced(string text)
    {
        Stack<char> opens = new();
        foreach (char symbol in text)
        {
            if (symbol == '(' || symbol == '[' || symbol == '{') opens.Push(symbol);
            else if (symbol == ')' || symbol == ']' || symbol == '}')
            {
                if (opens.Count == 0) return false;
                char open = opens.Pop();
                if ((symbol == ')' && open != '(') || (symbol == ']' && open != '[') || (symbol == '}' && open != '{')) return false;
            }
        }
        return opens.Count == 0;
    }
    public static void Run()
    {
        Check.True(IsBalanced("a([b]{c})")); Check.True(!IsBalanced("([)]"));
        Check.True(!IsBalanced("(")); Check.True(!IsBalanced(")")); Check.True(IsBalanced(""));
    }
}
```

**Исполняемые проверки**

- Вложенные скобки с текстом
- Перекрёстные пары
- Незакрытая открывающая
- Лишняя закрывающая
- Пустая строка

<a id="07-06"></a>
## 07-06. Удаление без пропущенного соседа

Обходите индексы от Count-1 к нулю, удаляя отрицательные.

**Ход решения**

После удаления индекса 0 значение -2 перемещается в 0, но исходный цикл переходит к 1. При обратном обходе удаление сдвигает только уже просмотренный хвост. Порядок сохраняет сам RemoveAt. Память O(1), время в худшем O(n²) из-за сдвигов; для больших данных лучше уплотнение одним проходом или RemoveAll после знакомства с предикатами.

**Частая ошибка**

Изменять список внутри foreach; считать обратный обход линейным при произвольных удалениях.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
namespace Course.Lesson07;

public static class Task06
{
    public static void RemoveNegatives(List<int> values)
    {
        for (int i = values.Count - 1; i >= 0; i--) if (values[i] < 0) values.RemoveAt(i);
    }
    public static void Run()
    {
        List<int> wrong = new() { -1, -2, 3 };
        for (int i = 0; i < wrong.Count; i++) if (wrong[i] < 0) wrong.RemoveAt(i);
        Check.SequenceEqual(new[] { -2, 3 }, wrong);
        List<int> values = new() { -1, -2, 3, 0, -4 }; RemoveNegatives(values);
        Check.SequenceEqual(new[] { 3, 0 }, values);
        List<int> all = new() { -1, -2 }; RemoveNegatives(all); Check.Equal(0, all.Count);
        List<int> empty = new(); RemoveNegatives(empty); Check.Equal(0, empty.Count);
    }
}
```

**Исполняемые проверки**

- Точное воспроизведение пропуска
- Соседние отрицательные удалены
- Нули сохранены
- Все отрицательные
- Пустой список

<a id="07-07"></a>
## 07-07. Общие элементы в порядке первого списка

Создайте множество из правого массива; при успешном Remove(value) добавьте value в ответ.

**Ход решения**

Множество из right хранит доступные общие значения. При обходе left успешное удаление означает и наличие справа, и первое выданное вхождение. Повторное значение больше не удалится. Порядок диктуется только left. Ожидаемое O(n+m), память O(u+k), где u — число различных правых значений, k — размер результата.

**Частая ошибка**

Вернуть дубликаты left при использовании только Contains; перечислить множество вместо left.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
namespace Course.Lesson07;

public static class Task07
{
    public static List<int> CommonInLeftOrder(int[] left, int[] right)
    {
        HashSet<int> available = new(right); List<int> result = new();
        foreach (int value in left) if (available.Remove(value)) result.Add(value);
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 4, 2 }, CommonInLeftOrder(new[] { 4, 2, 4, 3, 2 }, new[] { 2, 4, 4 }));
        Check.SequenceEqual(Array.Empty<int>(), CommonInLeftOrder(new[] { 1 }, new[] { 2 }));
        Check.SequenceEqual(Array.Empty<int>(), CommonInLeftOrder(Array.Empty<int>(), new[] { 1 }));
        Check.SequenceEqual(Array.Empty<int>(), CommonInLeftOrder(new[] { 1 }, Array.Empty<int>()));
    }
}
```

**Исполняемые проверки**

- Повторы в обоих входах
- Нет пересечения
- Пустой left
- Пустой right

<a id="07-08"></a>
## 07-08. Склад: резервирование целиком

В первом проходе проверьте возможность всего запроса, во втором примените списания.

**Ход решения**

Если списывать сразу при проверке, поздний недостаток оставит частично изменённый склад. Две фазы обеспечивают атомарность на уровне однопоточного учебного метода. Нулевые позиции пропускаем: они не требуют товара. После успешной проверки второй проход не может встретить недостачу при отсутствии параллельных изменений. Ожидаемое O(k), память O(1).

**Частая ошибка**

Мутировать до окончания проверки; добавлять отсутствующий ключ с нулём; обещать многопоточную транзакцию.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
namespace Course.Lesson07;

public static class Task08
{
    public static bool TryReserve(Dictionary<string, int> stock, Dictionary<string, int> request)
    {
        foreach (var pair in request)
            if (pair.Value > 0 && (!stock.TryGetValue(pair.Key, out int count) || count < pair.Value)) return false;
        foreach (var pair in request) if (pair.Value > 0) stock[pair.Key] -= pair.Value;
        return true;
    }
    public static void Run()
    {
        Dictionary<string, int> stock = new(StringComparer.Ordinal) { ["A"] = 5, ["B"] = 2 };
        Check.True(!TryReserve(stock, new(StringComparer.Ordinal) { ["A"] = 3, ["B"] = 3 }));
        Check.Equal(5, stock["A"]); Check.Equal(2, stock["B"]);
        Check.True(TryReserve(stock, new(StringComparer.Ordinal) { ["A"] = 3 })); Check.Equal(2, stock["A"]);
        Check.True(!TryReserve(stock, new(StringComparer.Ordinal) { ["C"] = 1 }));
        Check.True(TryReserve(stock, new(StringComparer.Ordinal) { ["C"] = 0 })); Check.True(!stock.ContainsKey("C"));
        Check.True(TryReserve(stock, new(StringComparer.Ordinal)));
    }
}
```

**Исполняемые проверки**

- Поздний недостаток не меняет ранний товар
- Успешный запрос списывает
- Отсутствующий положительный товар
- Нулевой отсутствующий товар не добавлен
- Пустой запрос

<a id="07-09"></a>
## 07-09. Круговое обслуживание

Очередь хранит индексы, отдельная копия массива — оставшееся время.

**Ход решения**

Первый круг: остатки 3,0,1, работа 1 завершилась. Второй круг: работа 0 остаётся с 1, работа 2 завершается. Третий завершает 0. FIFO сохраняет равноправную смену. Число ходов равно сумме округлённых вверх durations[i]/quantum. Память O(n).

**Частая ошибка**

Повторно ставить завершённую работу; хранить только длительности и потерять идентификаторы; менять вход.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
namespace Course.Lesson07;

public static class Task09
{
    public static List<int> CompletionOrder(int[] durations, int quantum)
    {
        int[] remaining = (int[])durations.Clone(); Queue<int> queue = new(); List<int> result = new();
        for (int i = 0; i < remaining.Length; i++) queue.Enqueue(i);
        while (queue.Count > 0)
        {
            int id = queue.Dequeue(); remaining[id] -= Math.Min(quantum, remaining[id]);
            if (remaining[id] == 0) result.Add(id); else queue.Enqueue(id);
        }
        return result;
    }
    public static void Run()
    {
        int[] durations = { 5, 2, 3 };
        Check.SequenceEqual(new[] { 1, 2, 0 }, CompletionOrder(durations, 2));
        Check.SequenceEqual(new[] { 0, 1, 2 }, CompletionOrder(durations, 10));
        Check.SequenceEqual(new[] { 0 }, CompletionOrder(new[] { 9 }, 1));
        Check.SequenceEqual(Array.Empty<int>(), CompletionOrder(Array.Empty<int>(), 1));
        Check.SequenceEqual(new[] { 5, 2, 3 }, durations);
    }
}
```

**Исполняемые проверки**

- Порядок [1,2,0]
- Один квант завершает все по очереди
- Одна работа
- Пустой вход
- Источник сохранён

<a id="07-10"></a>
## 07-10. Убрать квадратичный поиск допуска

Замените внутренний цикл на Contains заранее построенного HashSet.

**Ход решения**

Вложенный цикл повторно просматривает allowed для каждого запроса. Множество делает индекс членства один раз. Перечислять нужно requested, чтобы сохранить его порядок и дубликаты. Нельзя переносить запросы в множество: это изменит результат. Ожидаемое время O(n+m), память O(m).

**Частая ошибка**

Заменить фильтрацию на пересечение уникальных множеств и потерять повторные запросы.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
namespace Course.Lesson07;

public static class Task10
{
    public static List<int> FilterAllowed(int[] requested, int[] allowed)
    {
        HashSet<int> index = new(allowed); List<int> result = new();
        foreach (int value in requested) if (index.Contains(value)) result.Add(value);
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 3, 3, 2 }, FilterAllowed(new[] { 3, 1, 3, 2 }, new[] { 3, 3, 2 }));
        Check.SequenceEqual(new[] { 1 }, FilterAllowed(new[] { 1 }, new[] { 1, 1 }));
        Check.Equal(0, FilterAllowed(new[] { 1, 2 }, Array.Empty<int>()).Count);
        Check.Equal(0, FilterAllowed(Array.Empty<int>(), new[] { 1 }).Count);
    }
}
```

**Исполняемые проверки**

- Повтор requested сохранён
- Повтор allowed не размножает результат
- Все запрещены
- Пустой requested

<a id="07-11"></a>
## 07-11. Группы позиций по метке

Создавайте отдельный список при первом появлении каждой метки и добавляйте текущий индекс.

**Ход решения**

Dictionary&lt;string,List&lt;int&gt;&gt; означает, что значением является список; квадратные скобки универсальных типов не создают отдельную группу автоматически. TryGetValue даёт существующий список либо повод создать новый. Один список на все ключи смешал бы группы. Ожидаемое O(n), память O(n).

**Частая ошибка**

Создать один общий список до цикла; нечаянно игнорировать регистр; искать каждый индекс заново.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
namespace Course.Lesson07;

public static class Task11
{
    public static Dictionary<string, List<int>> PositionsByTag(string[] tags)
    {
        Dictionary<string, List<int>> result = new(StringComparer.Ordinal);
        for (int i = 0; i < tags.Length; i++)
        {
            if (!result.TryGetValue(tags[i], out List<int>? positions))
            {
                positions = new List<int>(); result.Add(tags[i], positions);
            }
            positions.Add(i);
        }
        return result;
    }
    public static void Run()
    {
        Dictionary<string, List<int>> result = PositionsByTag(new[] { "red", "blue", "red", "Red" });
        Check.SequenceEqual(new[] { 0, 2 }, result["red"]); Check.SequenceEqual(new[] { 3 }, result["Red"]);
        result["red"].Add(99); Check.SequenceEqual(new[] { 1 }, result["blue"]);
        Check.Equal(0, PositionsByTag(Array.Empty<string>()).Count);
    }
}
```

**Исполняемые проверки**

- Все позиции повторяющейся метки
- Чувствительность к регистру
- Независимость списков
- Пустой ввод

<a id="07-12"></a>
## 07-12. Равенство с учётом кратности

После проверки длин подсчитайте частоты left и уменьшайте их для каждого элемента right.

**Ход решения**

Множество теряет кратность. Словарь хранит оставшееся число «талонов» каждого значения. Если справа требуется отсутствующий или уже погашенный талон, массивы различаются. При равной длине и успешном погашении всех правых элементов незакрытых талонов остаться не может. Ожидаемое O(n+m), память O(u).

**Частая ошибка**

Сравнивать только длины и уникальные значения; допускать отрицательные счётчики.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
namespace Course.Lesson07;

public static class Task12
{
    public static bool SameMultiset(int[] left, int[] right)
    {
        if (left.Length != right.Length) return false;
        Dictionary<int, int> counts = new();
        foreach (int value in left) { counts.TryGetValue(value, out int count); counts[value] = count + 1; }
        foreach (int value in right)
        {
            if (!counts.TryGetValue(value, out int count) || count == 0) return false;
            counts[value] = count - 1;
        }
        return true;
    }
    public static void Run()
    {
        Check.True(SameMultiset(new[] { 1, 2, 1 }, new[] { 2, 1, 1 }));
        Check.True(!SameMultiset(new[] { 1, 1, 2 }, new[] { 1, 2, 2 }));
        Check.True(!SameMultiset(new[] { 1 }, Array.Empty<int>()));
        Check.True(SameMultiset(Array.Empty<int>(), Array.Empty<int>()));
        Check.True(!SameMultiset(new[] { int.MinValue }, new[] { int.MaxValue }));
    }
}
```

**Исполняемые проверки**

- Перестановка с повторами
- Те же уникальные значения с другой кратностью
- Разная длина
- Оба пусты
- Отсутствующее значение

<a id="07-13"></a>
## 07-13. Отмена и повтор изменения

Перед переходом в прошлое сохраняйте текущее в redo, перед возвратом — в undo; set начинает новую ветвь.

**Ход решения**

Стек undo хранит прежние состояния, redo — состояния, отменённые последними. Перемещение между ними всегда записывает текущее значение в противоположный стек. Новая команда set делает старую будущую ветвь недействительной. Важно хранить снимки, а не только число операций. O(n), память O(n).

**Частая ошибка**

Не очищать redo после set; сохранять новое значение вместо предыдущего; Pop без проверки Count.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
namespace Course.Lesson07;

public static class Task13
{
    public static int ReplayEditor(string[] commands)
    {
        int current = 0; Stack<int> undo = new(); Stack<int> redo = new();
        foreach (string command in commands)
        {
            if (command.StartsWith("set ", StringComparison.Ordinal))
            {
                undo.Push(current); current = int.Parse(command.Substring(4), System.Globalization.CultureInfo.InvariantCulture); redo.Clear();
            }
            else if (command == "undo" && undo.Count > 0) { redo.Push(current); current = undo.Pop(); }
            else if (command == "redo" && redo.Count > 0) { undo.Push(current); current = redo.Pop(); }
        }
        return current;
    }
    public static void Run()
    {
        Check.Equal(8, ReplayEditor(new[] { "set 4", "set 8", "undo", "redo" }));
        Check.Equal(7, ReplayEditor(new[] { "set 4", "undo", "set 7", "redo" }));
        Check.Equal(0, ReplayEditor(new[] { "undo", "redo" }));
        Check.Equal(0, ReplayEditor(new[] { "set 1", "set 2", "undo", "undo" }));
        Check.Equal(4, ReplayEditor(new[] { "set 4", "set 4", "undo" }));
    }
}
```

**Исполняемые проверки**

- Undo затем redo
- Новая ветвь удаляет redo
- Пустые истории безопасны
- Несколько undo подряд
- Повторная запись того же числа

<a id="07-14"></a>
## 07-14. Проверка компаратора ключей

Создайте HashSet со StringComparer.OrdinalIgnoreCase; отличающий тест содержит хотя бы два регистра одного кода.

**Ход решения**

Тест [id,id] не различает компараторы, а [id,ID] различает. Значимые знаки проверяют, что реализация не нормализует лишнее. Пустота отличает неверную начальную единицу счётчика. Ожидаемое O(n), память O(u). Ошибочный вариант действительно вычисляется и даёт 3 на общем примере вместо 2.

**Частая ошибка**

Доказывать ошибку словами, не исполняя плохой вариант; тестировать только одинаковые строки.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
namespace Course.Lesson07;

public static class Task14
{
    public static int UniqueCodeCount(string[] codes) => new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase).Count;
    public static void Run()
    {
        Check.Equal(0, UniqueCodeCount(Array.Empty<string>()));
        Check.Equal(1, UniqueCodeCount(new[] { "id", "id" }));
        string[] sample = { "id", "ID", "id-1" };
        Check.Equal(2, UniqueCodeCount(sample)); Check.Equal(2, UniqueCodeCount(new[] { "id", "id " }));
        int wrong = new HashSet<string>(sample).Count;
        Check.Equal(3, wrong); Check.True(wrong != UniqueCodeCount(sample));
    }
}
```

**Исполняемые проверки**

- Пустой массив
- Точный повтор
- Разные регистры и значимый дефис
- Пробел остаётся значим
- Плохой компаратор реально различён

<a id="07-15"></a>
## 07-15. Кратчайший маршрут по клеткам

Обход в ширину помечает расстояние при первом добавлении клетки в очередь.

**Ход решения**

FIFO обрабатывает сначала клетки расстояния d, затем d+1. Поэтому первый найденный путь к клетке кратчайший. Пометка при постановке препятствует многократной постановке одной клетки из разных соседей. Координаты кодируем числом row*columns+column, восстанавливаем / и %. Каждая клетка обрабатывается не более раза: O(r*c), память O(r*c).

**Частая ошибка**

Использовать стек и получить произвольный, а не кратчайший путь; помечать после извлечения; позволять диагонали.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
namespace Course.Lesson07;

public static class Task15
{
    public static int ShortestPath(bool[,] blocked, int startRow, int startColumn, int endRow, int endColumn)
    {
        if (blocked[startRow, startColumn] || blocked[endRow, endColumn]) return -1;
        int rows = blocked.GetLength(0), columns = blocked.GetLength(1);
        int[,] distance = new int[rows, columns];
        for (int r = 0; r < rows; r++) for (int c = 0; c < columns; c++) distance[r, c] = -1;
        Queue<int> queue = new(); queue.Enqueue(startRow * columns + startColumn); distance[startRow, startColumn] = 0;
        int[] dr = { -1, 1, 0, 0 }; int[] dc = { 0, 0, -1, 1 };
        while (queue.Count > 0)
        {
            int id = queue.Dequeue(); int r = id / columns, c = id % columns;
            if (r == endRow && c == endColumn) return distance[r, c];
            for (int direction = 0; direction < 4; direction++)
            {
                int nr = r + dr[direction], nc = c + dc[direction];
                if (nr < 0 || nr >= rows || nc < 0 || nc >= columns || blocked[nr, nc] || distance[nr, nc] >= 0) continue;
                distance[nr, nc] = distance[r, c] + 1; queue.Enqueue(nr * columns + nc);
            }
        }
        return -1;
    }
    public static void Run()
    {
        bool[,] map = { { false, true, false }, { false, false, false } };
        Check.Equal(4, ShortestPath(map, 0, 0, 0, 2));
        Check.Equal(-1, ShortestPath(new[,] { { false, true, false } }, 0, 0, 0, 2));
        Check.Equal(0, ShortestPath(new bool[1, 1], 0, 0, 0, 0));
        Check.Equal(-1, ShortestPath(new[,] { { true, false } }, 0, 0, 0, 1));
        Check.True(map[0, 1] && !map[1, 1]);
    }
}
```

**Исполняемые проверки**

- Обход стены длиной 4
- Недостижимый конец
- Одна свободная клетка
- Заблокированный старт
- Карта сохранена

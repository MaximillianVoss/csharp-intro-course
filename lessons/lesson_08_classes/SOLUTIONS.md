# Урок 8. Классы, свойства, инварианты и композиция: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="08-01"></a>
## 08-01. Счётчик с верхней границей

Свойства с private set оставляют изменение внутри класса; сравнение до ++ защищает инвариант.

**Ход решения**

Инвариант 0<=Value<=Maximum должен выполняться после конструктора и каждой публичной операции. На границе не вычисляем Value+1, поэтому int.MaxValue не переполняется. Maximum неизменяем. Для проверки крайнего значения не нужно выполнять миллиарды инкрементов: проверяется сохранённая граница и обычное продвижение. Операция O(1).

**Частая ошибка**

Открыть setter или проверять после увеличения; считать отказом значение Maximum, хотя оно допустимо.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
namespace Course.Lesson08;

public static class Task01
{
    public sealed class BoundedCounter
    {
        public int Maximum { get; }
        public int Value { get; private set; }
        public BoundedCounter(int maximum) { Maximum = maximum; }
        public bool TryIncrement()
        {
            if (Value == Maximum) return false;
            Value++; return true;
        }
    }
    public static void Run()
    {
        BoundedCounter counter = new(2);
        Check.True(counter.TryIncrement()); Check.True(counter.TryIncrement());
        Check.True(!counter.TryIncrement()); Check.Equal(2, counter.Value);
        BoundedCounter zero = new(0); Check.True(!zero.TryIncrement()); Check.Equal(0, zero.Value);
        BoundedCounter large = new(int.MaxValue); Check.Equal(int.MaxValue, large.Maximum); Check.True(large.TryIncrement());
    }
}
```

**Исполняемые проверки**

- Две успешные операции и отказ
- Нулевая граница
- Большая граница хранится точно

<a id="08-02"></a>
## 08-02. Размеры меняются вместе

Проверяйте оба ограничения перед записью, Area вычисляйте по текущим Width и Height.

**Ход решения**

Согласованное состояние состоит из двух размеров. Отказ после изменения первого нарушил бы обещание сохранения объекта. Производная площадь не хранится отдельно: тогда невозможно забыть обновить кэш. Допустимые размеры гарантируют, что произведение помещается в decimal. Все операции O(1).

**Частая ошибка**

Частично менять состояние до проверки height; кэшировать Area без синхронизации.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
namespace Course.Lesson08;

public static class Task02
{
    public sealed class Rectangle
    {
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Area => Width * Height;
        public Rectangle(decimal width, decimal height) { Width = width; Height = height; }
        public bool TryResize(decimal width, decimal height)
        {
            if (width < 0 || height < 0 || width > 1_000_000 || height > 1_000_000) return false;
            Width = width; Height = height; return true;
        }
    }
    public static void Run()
    {
        Rectangle rectangle = new(2, 3); Check.Equal(6m, rectangle.Area);
        Check.True(!rectangle.TryResize(4, -1)); Check.Equal(2m, rectangle.Width); Check.Equal(3m, rectangle.Height);
        Check.True(rectangle.TryResize(4, 5)); Check.Equal(20m, rectangle.Area);
        Check.True(rectangle.TryResize(0, 3)); Check.Equal(0m, rectangle.Area);
        Check.True(!rectangle.TryResize(1_000_001, 1));
    }
}
```

**Исполняемые проверки**

- Начальная площадь
- Недопустимая высота сохраняет оба размера
- Успешное изменение
- Нулевая площадь
- Верхняя граница

<a id="08-03"></a>
## 08-03. Объект и значение при присваивании

Box хранится через ссылку на общий объект; Point является значимым типом и копируется как значение.

**Ход решения**

b получает ту же ссылку, что a, поэтому изменение Number наблюдается через обе переменные. q получает копию Point. Выражение with создаёт новое значение q с заменённым X; p остаётся прежним. record struct добавляет удобные операции, но ключевой факт здесь — struct. Точное выполнение подтверждает [8,8,2,9].

**Частая ошибка**

Считать class-переменную самим объектом; ожидать, что запись нового Point изменит ранее скопированный p.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
namespace Course.Lesson08;

public static class Task03
{
    public sealed class Box { public int Number { get; set; } }
    public readonly record struct Point(int X, int Y);
    public static int[] Replay()
    {
        Box a = new() { Number = 1 }; Box b = a; b.Number = 8;
        Point p = new(2, 3); Point q = p; q = q with { X = 9 };
        Check.True(ReferenceEquals(a, b)); Check.Equal(new Point(2, 3), p);
        return new[] { a.Number, b.Number, p.X, q.X };
    }
    public static void Run() => Check.SequenceEqual(new[] { 8, 8, 2, 9 }, Replay());
}
```

**Исполняемые проверки**

- Точный результат фрагмента
- Обе ссылки указывают на один Box
- Point по значению сохраняет исходную координату

<a id="08-04"></a>
## 08-04. Снятие без отрицательного баланса

Инкапсулируйте баланс; ожидаемый отказ обозначайте bool до изменения состояния.

**Ход решения**

Условие amount<=0 исключает бесплатную и отрицательную трату, которая иначе пополнила бы баланс. Проверка amount>Balance защищает от отрицательного остатка. При успехе сумма вычитается ровно один раз. decimal позволяет точно представить приведённые десятичные суммы. O(1).

**Частая ошибка**

Вычитать отрицательную сумму; double для денежных десятичных примеров; открытый setter Balance.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
namespace Course.Lesson08;

public static class Task04
{
    public sealed class Wallet
    {
        public decimal Balance { get; private set; }
        public Wallet(decimal initialBalance) { Balance = initialBalance; }
        public bool TrySpend(decimal amount)
        {
            if (amount <= 0 || amount > Balance) return false;
            Balance -= amount; return true;
        }
    }
    public static void Run()
    {
        Wallet wallet = new(10.50m); Check.True(wallet.TrySpend(3.25m)); Check.Equal(7.25m, wallet.Balance);
        Check.True(!wallet.TrySpend(8)); Check.Equal(7.25m, wallet.Balance);
        Check.True(!wallet.TrySpend(0)); Check.True(!wallet.TrySpend(-2));
        Check.True(wallet.TrySpend(7.25m)); Check.Equal(0m, wallet.Balance); Check.True(!wallet.TrySpend(1));
    }
}
```

**Исполняемые проверки**

- Дробная трата
- Недостаточный баланс сохранён
- Ноль и отрицательная сумма
- Трата всего остатка
- Баланс 0

<a id="08-05"></a>
## 08-05. Граница ёмкости под тестом

Сравните amount со свободным местом Capacity-Volume, а не складывайте int до проверки.

**Ход решения**

Инвариант 0<=Volume<=Capacity гарантирует неотрицательную разницу. amount>Capacity-Volume обнаруживает и int.MaxValue без опасного сложения. Ошибка < вместо <= проявляется на границе равенства, но не на обычном малом добавлении. После отказа отдельно проверяем Volume. O(1).

**Частая ошибка**

Проверять только внутренние точки; сначала сложить int и получить переполнение; принимать отрицательный долив.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
namespace Course.Lesson08;

public static class Task05
{
    public sealed class Tank
    {
        public int Capacity { get; }
        public int Volume { get; private set; }
        public Tank(int capacity) { Capacity = capacity; }
        public bool TryFill(int amount)
        {
            if (amount < 0 || amount > Capacity - Volume) return false;
            Volume += amount; return true;
        }
    }
    public static void Run()
    {
        Tank tank = new(10); Check.True(tank.TryFill(10)); Check.Equal(10, tank.Volume);
        Check.True(!tank.TryFill(1)); Check.Equal(10, tank.Volume);
        Check.True(tank.TryFill(0)); Check.True(!tank.TryFill(-1));
        Tank empty = new(10); Check.True(!empty.TryFill(int.MaxValue)); Check.Equal(0, empty.Volume);
        bool wrongExactFill = 10 >= 0 && 10 < empty.Capacity - empty.Volume;
        Check.True(!wrongExactFill); Check.True(empty.TryFill(10));
    }
}
```

**Исполняемые проверки**

- Точное заполнение
- Перелив сохраняет состояние
- Нулевой и отрицательный долив
- int.MaxValue не переполняет
- Плохой предикат различён

<a id="08-06"></a>
## 08-06. Изменение записи через with

record хранит данные и сравнивает их по значениям; with создаёт новый объект с заменой свойства.

**Ход решения**

Позиционный record class генерирует init-свойства и равенство по компонентам. Новая цена валидируется до создания копии. Даже при равной цене with создаёт другой объект, поэтому Equals может быть true одновременно с ReferenceEquals=false. null сообщает ожидаемый отказ; вызывающий проверяет его до использования. O(1) по числу полей.

**Частая ошибка**

Мутировать исходник через открытые set; путать равенство данных и тождественность объектов.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
namespace Course.Lesson08;

public static class Task06
{
    public sealed record Product(string Code, string Title, decimal Price);
    public static Product? Reprice(Product original, decimal newPrice)
    {
        if (newPrice < 0 || newPrice > 1_000_000) return null;
        return original with { Price = newPrice };
    }
    public static void Run()
    {
        Product original = new("P1", "Книга", 100);
        Product? changed = Reprice(original, 120);
        Check.True(changed is not null); Check.Equal(120m, changed!.Price); Check.Equal(100m, original.Price);
        Check.True(Reprice(original, -1) is null); Check.True(Reprice(original, 1_000_001) is null);
        Product? same = Reprice(original, 100); Check.Equal(original, same); Check.True(!ReferenceEquals(original, same));
        Check.Equal(0m, Reprice(original, 0)!.Price);
    }
}
```

**Исполняемые проверки**

- Новая цена и сохранённый исходник
- Отрицательная и слишком большая цена
- Та же цена: равны данные, различны ссылки
- Нулевая цена

<a id="08-07"></a>
## 08-07. Корзина состоит из строк

Cart владеет списком неизменяемых CartLine и вычисляет сумму их Total.

**Ход решения**

Композиция выражает отношение «корзина содержит строки». У каждой строки своя формула, корзина только суммирует. Проверка всех параметров предшествует созданию строки; максимум строк ограничивает размер и сумму. Разные операции одного кода остаются отдельными по контракту. TryAdd ожидаемо O(1), подсчёт Total O(n).

**Частая ошибка**

Наследовать корзину от строки; случайно объединять повторные коды; вернуть изменяемый внутренний List.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
namespace Course.Lesson08;

public static class Task07
{
    public sealed record CartLine(string Code, decimal UnitPrice, int Quantity)
    {
        public decimal Total => UnitPrice * Quantity;
    }
    public sealed class Cart
    {
        private readonly List<CartLine> lines = new();
        public int Count => lines.Count;
        public decimal Total { get { decimal total = 0; foreach (CartLine line in lines) total += line.Total; return total; } }
        public bool TryAdd(string? code, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(code) || price < 0 || price > 1_000_000 || quantity < 1 || quantity > 1000 || lines.Count == 1000) return false;
            lines.Add(new CartLine(code, price, quantity)); return true;
        }
    }
    public static void Run()
    {
        Cart cart = new(); Check.Equal(0m, cart.Total);
        Check.True(cart.TryAdd("A", 2.50m, 2)); Check.True(cart.TryAdd("A", 2.50m, 1));
        Check.Equal(2, cart.Count); Check.Equal(7.50m, cart.Total);
        Check.True(!cart.TryAdd(null, 1, 1)); Check.True(!cart.TryAdd(" ", 1, 1));
        Check.True(!cart.TryAdd("B", 1, 0)); Check.Equal(2, cart.Count);
        Check.True(cart.TryAdd("free", 0, 1)); Check.Equal(7.50m, cart.Total);
    }
}
```

**Исполняемые проверки**

- Две строки с одним кодом и правильный итог
- Некорректный код
- Нулевая quantity
- Нулевая цена допустима
- Отказы не добавляют строки

<a id="08-08"></a>
## 08-08. Одна температура, две шкалы

Храните только Kelvin, а вторую шкалу получайте формулой; проверяйте допустимый вход до вычисления.

**Ход решения**

Две независимые изменяемые переменные легко рассогласовать. Одно хранимое значение обеспечивает связность представлений. Проверка границ Celsius предшествует прибавлению: даже предельный decimal не приводит к арифметической ошибке в штатном отказе. Все операции O(1).

**Частая ошибка**

Проверять только Celsius>=0; хранить две шкалы и обновлять одну; складывать до валидации.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
namespace Course.Lesson08;

public static class Task08
{
    public sealed class Temperature
    {
        public decimal Kelvin { get; private set; }
        public decimal Celsius => Kelvin - 273.15m;
        public Temperature(decimal kelvin) { Kelvin = kelvin; }
        public bool TrySetCelsius(decimal celsius)
        {
            if (celsius < -273.15m || celsius > 9726.85m) return false;
            Kelvin = celsius + 273.15m; return true;
        }
    }
    public static void Run()
    {
        Temperature value = new(273.15m); Check.Equal(0m, value.Celsius);
        Check.True(value.TrySetCelsius(-273.15m)); Check.Equal(0m, value.Kelvin);
        Check.True(!value.TrySetCelsius(-273.16m)); Check.Equal(0m, value.Kelvin);
        Check.True(value.TrySetCelsius(9726.85m)); Check.Equal(10000m, value.Kelvin);
        Check.True(!value.TrySetCelsius(decimal.MaxValue)); Check.Equal(10000m, value.Kelvin);
    }
}
```

**Исполняемые проверки**

- Точка замерзания
- Абсолютный ноль
- Ниже нижней границы сохраняет Kelvin
- Верхняя допустимая точка
- decimal.MaxValue даёт false

<a id="08-09"></a>
## 08-09. Общее поле вместо состояния экземпляра

Поле, принадлежащее отдельному измерителю, должно быть экземплярным, а не static.

**Ход решения**

static создаёт одно поле на тип, поэтому оба объекта увеличивают одну переменную. Экземплярное поле присутствует у каждого объекта отдельно. Для повторяемого запуска исходный дефект проверяется по разнице с начальным общим значением; в свежем процессе это именно [2,2]. Исправленный вариант тестируется двумя объектами и дополнительным Tick только одного.

**Частая ошибка**

Заменить static-свойство, оставив static-поле; обнулять общий счётчик в конструкторе, стирая состояние других объектов.

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
namespace Course.Lesson08;

public static class Task09
{
    public sealed class BrokenMeter
    {
        private static int value;
        public int Value => value;
        public void Tick() { value++; }
    }
    public sealed class Meter
    {
        private int value;
        public int Value => value;
        public void Tick() { value++; }
    }
    public static void Run()
    {
        BrokenMeter a = new(); BrokenMeter b = new(); int baseline = a.Value;
        a.Tick(); b.Tick(); Check.Equal(baseline + 2, a.Value); Check.Equal(a.Value, b.Value);
        Meter first = new(); Meter second = new(); first.Tick(); second.Tick();
        Check.Equal(1, first.Value); Check.Equal(1, second.Value);
        first.Tick(); Check.Equal(2, first.Value); Check.Equal(1, second.Value);
    }
}
```

**Исполняемые проверки**

- Исходный дефект дважды увеличивает общее значение
- Исправленные объекты имеют [1,1]
- Дополнительный Tick не меняет второй объект

<a id="08-10"></a>
## 08-10. Каталог не отдаёт свои внутренности

Список сделайте private readonly, изменение проведите через проверенный метод, чтение — через копию.

**Ход решения**

readonly запрещает переназначить поле, но не запрещает Clear у List. Поэтому одного readonly недостаточно для защиты от внешнего кода. ToArray копирует ссылки на неизменяемые string, что достаточно для независимого изменения набора и позиций. Старый снимок не дополняется после новых добавлений. TryAdd O(1) в среднем, GetTitles O(n).

**Частая ошибка**

Возвращать тот же List под другим именем; считать readonly списка запретом изменения элементов.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
namespace Course.Lesson08;

public static class Task10
{
    public sealed class Catalog
    {
        private readonly List<string> titles = new();
        public int Count => titles.Count;
        public bool TryAdd(string? title)
        {
            if (string.IsNullOrWhiteSpace(title) || titles.Count == 1000) return false;
            titles.Add(title); return true;
        }
        public string[] GetTitles() => titles.ToArray();
    }
    public static void Run()
    {
        Catalog catalog = new(); Check.True(catalog.TryAdd("A"));
        string[] snapshot = catalog.GetTitles(); snapshot[0] = "X"; Check.Equal("A", catalog.GetTitles()[0]);
        Check.True(catalog.TryAdd("A")); Check.Equal(1, snapshot.Length); Check.SequenceEqual(new[] { "A", "A" }, catalog.GetTitles());
        Check.True(!catalog.TryAdd(null)); Check.True(!catalog.TryAdd("  ")); Check.Equal(2, catalog.Count);
    }
}
```

**Исполняемые проверки**

- Внешнее изменение снимка не влияет
- Новый элемент не меняет старый снимок
- Невалидные строки отклонены
- Порядок и повтор сохранены

<a id="08-11"></a>
## 08-11. Структура с общей ссылкой внутри

Копирование struct копирует поля; поле-массив остаётся ссылкой на общий объект. Для независимости клонируйте его.

**Ход решения**

Revision — int, поэтому изменяется только у b. Values — ссылка, поэтому скопированные ссылки указывают на один массив. Сам факт struct не делает граф объектов независимым. DeepCopy явно создаёт новый массив int и сохраняет Revision. O(n) времени и памяти для копирования массива.

**Частая ошибка**

Обещать глубокую копию после обычного присваивания struct; считать with универсальным глубоким копированием.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
namespace Course.Lesson08;

public static class Task11
{
    public struct Snapshot { public int Revision; public int[] Values; }
    public static int[] Replay()
    {
        Snapshot a = new() { Revision = 1, Values = new[] { 10 } };
        Snapshot b = a; b.Revision = 2; b.Values[0] = 99;
        return new[] { a.Revision, b.Revision, a.Values[0], b.Values[0] };
    }
    public static Snapshot DeepCopy(Snapshot source) => new() { Revision = source.Revision, Values = (int[])source.Values.Clone() };
    public static void Run()
    {
        Check.SequenceEqual(new[] { 1, 2, 99, 99 }, Replay());
        Snapshot source = new() { Revision = 7, Values = new[] { 10 } }; Snapshot copy = DeepCopy(source);
        copy.Values[0] = 20; Check.Equal(10, source.Values[0]); Check.Equal(7, copy.Revision);
        Check.Equal(0, DeepCopy(new Snapshot { Revision = 0, Values = Array.Empty<int>() }).Values.Length);
    }
}
```

**Исполняемые проверки**

- Точное воспроизведение [1,2,99,99]
- DeepCopy независим
- Revision сохранён
- Пустой массив копируется

<a id="08-12"></a>
## 08-12. Интервал создаётся корректным

Фабрика не даёт получить неверный объект, а пересечение выражается Start<other.End && other.Start<End.

**Ход решения**

Закрытый конструктор ограничивает все пути создания фабрикой. Для пересечения оба интервала должны начинаться до завершения другого; равенство исключено из-за полуоткрытой модели. Формула симметрична и работает при полном вложении. O(1); nullable требует проверки до обращения.

**Частая ошибка**

Открытый конструктор в обход фабрики; <= превращает соприкосновение в пересечение.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
namespace Course.Lesson08;

public static class Task12
{
    public sealed class TimeSlot
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        private TimeSlot(DateTime start, DateTime end) { Start = start; End = end; }
        public static TimeSlot? Create(DateTime start, DateTime end) => start < end ? new TimeSlot(start, end) : null;
        public bool Overlaps(TimeSlot other) => Start < other.End && other.Start < End;
    }
    public static void Run()
    {
        DateTime day = new(2030, 1, 1);
        TimeSlot a = TimeSlot.Create(day.AddHours(10), day.AddHours(11))!;
        TimeSlot touching = TimeSlot.Create(day.AddHours(11), day.AddHours(12))!;
        TimeSlot cross = TimeSlot.Create(day.AddHours(10.5), day.AddHours(11.5))!;
        Check.True(!a.Overlaps(touching)); Check.True(a.Overlaps(cross) && cross.Overlaps(a));
        Check.True(a.Overlaps(TimeSlot.Create(day.AddHours(10.2), day.AddHours(10.4))!));
        Check.True(TimeSlot.Create(day, day) is null); Check.True(TimeSlot.Create(day.AddHours(1), day) is null);
    }
}
```

**Исполняемые проверки**

- Соприкосновение не пересекается
- Обычное пересечение симметрично
- Вложенный интервал
- Нулевой и обратный интервалы отклонены

<a id="08-13"></a>
## 08-13. Заказ проходит разрешённые состояния

Каждая операция проверяет разрешённый переход и записывает только новое допустимое состояние.

**Ход решения**

Автомат описывает историю объекта, поэтому одного bool IsDone недостаточно для трёх различимых фаз. Централизованные переходы запрещают внешнему коду воскресить отменённый заказ. Повторные операции не считаются успешными по явному контракту. Все переходы O(1).

**Частая ошибка**

Общий setter State обходит правила; Submit из Submitted снова сообщает успех; отмена делает Draft.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
namespace Course.Lesson08;

public static class Task13
{
    public enum OrderState { Draft, Submitted, Cancelled }
    public sealed class Order
    {
        public OrderState State { get; private set; } = OrderState.Draft;
        public bool TrySubmit()
        {
            if (State != OrderState.Draft) return false;
            State = OrderState.Submitted; return true;
        }
        public bool TryCancel()
        {
            if (State == OrderState.Cancelled) return false;
            State = OrderState.Cancelled; return true;
        }
    }
    public static void Run()
    {
        Order order = new(); Check.Equal(OrderState.Draft, order.State);
        Check.True(order.TrySubmit()); Check.True(!order.TrySubmit()); Check.Equal(OrderState.Submitted, order.State);
        Check.True(order.TryCancel()); Check.True(!order.TrySubmit()); Check.True(!order.TryCancel()); Check.Equal(OrderState.Cancelled, order.State);
        Order draft = new(); Check.True(draft.TryCancel()); Check.Equal(OrderState.Cancelled, draft.State);
    }
}
```

**Исполняемые проверки**

- Начальное Draft
- Отправка и запрет повтора
- Отмена отправленного
- Запрет после Cancelled
- Отмена черновика

<a id="08-14"></a>
## 08-14. Нормализованная дробь

Сначала обработайте запрет нулевого знаменателя, затем знак и НОД.

**Ход решения**

Каноническая форма упрощает дальнейшие сравнения компонентов. Для 0 ненулевой знаменатель становится его модулем НОД, поэтому деление сразу даёт 0/1. Заданные границы исключают проблему Math.Abs(int.MinValue). Алгоритм Евклида уменьшает остаток, время O(log max(|n|,|d|)), память O(1).

**Частая ошибка**

Сократить знак, но оставить отрицательный знаменатель; делить на нулевой НОД; игнорировать ограничение int.MinValue.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
namespace Course.Lesson08;

public static class Task14
{
    public sealed class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }
        private Fraction(int numerator, int denominator) { Numerator = numerator; Denominator = denominator; }
        public static Fraction? Create(int numerator, int denominator)
        {
            if (denominator == 0) return null;
            if (denominator < 0) { numerator = -numerator; denominator = -denominator; }
            int a = Math.Abs(numerator), b = denominator;
            while (b != 0) { int remainder = a % b; a = b; b = remainder; }
            return new Fraction(numerator / a, denominator / a);
        }
    }
    public static void Run()
    {
        Fraction a = Fraction.Create(6, -8)!; Check.Equal(-3, a.Numerator); Check.Equal(4, a.Denominator);
        Fraction zero = Fraction.Create(0, -5)!; Check.Equal(0, zero.Numerator); Check.Equal(1, zero.Denominator);
        Check.Equal(3, Fraction.Create(-6, -8)!.Numerator); Check.Equal(7, Fraction.Create(3, 7)!.Denominator);
        Check.True(Fraction.Create(1, 0) is null);
    }
}
```

**Исполняемые проверки**

- Сокращение и отрицательный знаменатель
- Ноль нормализован
- Оба аргумента отрицательные
- Несократимая дробь
- Нулевой знаменатель

<a id="08-15"></a>
## 08-15. Перенос между контейнерами

Обе проверки до мутации гарантируют отказ целиком, ReferenceEquals отдельно обрабатывает перенос в себя.

**Ход решения**

Метод взаимодействует с двумя объектами, каждый из которых должен сохранить 0<=Quantity<=Capacity. Сначала отсекаем отрицательный ввод и нехватку источника, затем случай одного объекта, затем свободное место. Это предотвращает частичное списание и лишнее ограничение свободного места при переносе в себя. Суммарное количество при успешном переносе неизменно. O(1).

**Частая ошибка**

Списывать до проверки target; проверять свободное место при переносе в себя; складывать amount с Quantity до валидации.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
namespace Course.Lesson08;

public static class Task15
{
    public sealed class Bin
    {
        public int Capacity { get; }
        public int Quantity { get; private set; }
        public Bin(int capacity, int quantity) { Capacity = capacity; Quantity = quantity; }
        public static bool TryTransfer(Bin source, Bin target, int amount)
        {
            if (amount < 0 || amount > source.Quantity) return false;
            if (ReferenceEquals(source, target) || amount == 0) return true;
            if (amount > target.Capacity - target.Quantity) return false;
            source.Quantity -= amount; target.Quantity += amount; return true;
        }
    }
    public static void Run()
    {
        Bin source = new(10, 7); Bin target = new(5, 2);
        Check.True(Bin.TryTransfer(source, target, 3)); Check.Equal(4, source.Quantity); Check.Equal(5, target.Quantity);
        Check.Equal(9, source.Quantity + target.Quantity);
        Check.True(!Bin.TryTransfer(source, target, 1)); Check.Equal(4, source.Quantity); Check.Equal(5, target.Quantity);
        Check.True(!Bin.TryTransfer(source, target, 5)); Check.True(!Bin.TryTransfer(source, target, -1));
        Check.True(Bin.TryTransfer(source, target, 0)); Check.True(Bin.TryTransfer(source, source, 4)); Check.Equal(4, source.Quantity);
        Check.True(!Bin.TryTransfer(source, source, 5));
    }
}
```

**Исполняемые проверки**

- Успех и сохранение общей суммы
- Недостаток места сохраняет оба
- Недостаток источника
- Отрицательный и нулевой перенос
- Перенос в себя полный и сверх остатка

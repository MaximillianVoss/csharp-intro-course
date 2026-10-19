# Урок 9. Интерфейсы, наследование и подстановка: задачи

Сначала решите задачу самостоятельно. Ответы находятся в отдельном SOLUTIONS.md.
Уровни: 1 — базовый, 2 — прикладной, 3 — повышенный.
Код пишите в templates/Tasks.cs; каждый TaskNN.Run — независимое рабочее место.
В TaskNN уже даны предметные сигнатуры; заполните методы и свой Run, сохранив контракт.
В задачах на чтение и тестирование запишите объяснение и воспроизводимый эксперимент.

| ID | Уровень | Формат | Задача |
| --- | --- | --- | --- |
| 09-01 | 1 | Реализация | [Общая операция разных фигур](#09-01) |
| 09-02 | 1 | Чтение кода | [Скрытие метода и переопределение](#09-02) |
| 09-03 | 1 | Реализация | [Налоговая политика как зависимость](#09-03) |
| 09-04 | 1 | Реализация | [Одинаковая команда разным устройствам](#09-04) |
| 09-05 | 1 | Проектирование тестов | [Часы для повторяемого теста](#09-05) |
| 09-06 | 2 | Реализация | [Общая рамка расчёта доставки](#09-06) |
| 09-07 | 2 | Поиск ошибки | [Перегрузка не заменила виртуальный метод](#09-07) |
| 09-08 | 2 | Рефакторинг | [Узкий интерфейс для читателя](#09-08) |
| 09-09 | 2 | Реализация | [Интерфейс не отменяет владение данными](#09-09) |
| 09-10 | 2 | Рефакторинг | [Один контракт для листа и папки](#09-10) |
| 09-11 | 3 | Реализация | [Добавление поведения обёрткой](#09-11) |
| 09-12 | 3 | Реализация | [Резервный источник вызывается по необходимости](#09-12) |
| 09-13 | 3 | Реализация | [Два представления одного отчёта](#09-13) |
| 09-14 | 3 | Проектирование тестов | [Совпадение сигнатур ещё не очередь](#09-14) |
| 09-15 | 3 | Прикладная задача | [Локальная служба уведомлений](#09-15) |

<a id="09-01"></a>
## 09-01. Общая операция разных фигур

Уровень 1 · Реализация · Навык: Полиморфный вызов через интерфейс

Объявите public interface IShape { double Area { get; } }, public sealed class Rectangle(double width,double height) и public sealed class Triangle(double width,double height), реализующие IShape. Обычные конструкторы эквивалентны указанной краткой форме. Размеры подаются в 0..1000. Реализуйте public static double TotalArea(IShape[] shapes): массив и элементы не null, длина 0..1000. Площадь прямоугольника width*height, треугольника width*height/2. Типы внутри TotalArea не проверять; пустой массив даёт 0.

**Пример и ожидаемое поведение**

[Rectangle(3,4), Triangle(3,4)] -&gt; 18; [] -&gt; 0.

<a id="09-02"></a>
## 09-02. Скрытие метода и переопределение

Уровень 1 · Чтение кода · Навык: Статический выбор скрытого члена и виртуальная диспетчеризация

Предскажите три строки и воспроизведите код в public static string[] Replay(). Все приведённые типы поместите внутрь Task02. Порядок вызовов сохраняется.
```csharp
public class Printer
{
    public string Describe() => "base description";
    public virtual string Render() => "base render";
}
public sealed class FancyPrinter : Printer
{
    public new string Describe() => "fancy description";
    public override string Render() => "fancy render";
}
FancyPrinter derived = new();
Printer parent = derived;
// вернуть parent.Describe(), parent.Render(), derived.Describe()
```

**Пример и ожидаемое поведение**

Replay возвращает string[3] в порядке parent.Describe(), parent.Render(), derived.Describe(); конкретные строки предскажите до выполнения.

<a id="09-03"></a>
## 09-03. Налоговая политика как зависимость

Уровень 1 · Реализация · Навык: Внедрение стратегии через конструктор

Объявите ITaxPolicy с decimal Tax(decimal net), реализации NoTax (0) и RateTax(decimal rate) (net*rate). rate подаётся в 0..1, net в 0..1000000. Создайте public sealed class Invoice с конструктором Invoice(ITaxPolicy policy), policy не null, и decimal Total(decimal net), возвращающим net+налог. Класс Invoice не создаёт конкретную политику и не проверяет её тип. Округление в этой задаче не выполняется.

**Пример и ожидаемое поведение**

Invoice(new RateTax(0.2m)).Total(100) -&gt; 120; NoTax -&gt; 100.

<a id="09-04"></a>
## 09-04. Одинаковая команда разным устройствам

Уровень 1 · Реализация · Навык: Сбор результатов полиморфных операций

Создайте public interface ICommand { string Execute(); }, public sealed class EchoCommand(string text), возвращающий text, и public sealed class SumCommand(int left,int right), возвращающий сумму в инвариантной десятичной записи. Слагаемые в -1000000..1000000, text не null. Реализуйте public static string[] ExecuteAll(ICommand[] commands), массив и элементы не null, длина 0..1000. Каждую команду выполнить ровно один раз в порядке массива, результаты вернуть в новом массиве. Пустой ввод -&gt; пустой результат.

**Пример и ожидаемое поведение**

[EchoCommand("готово"),SumCommand(2,3)] -&gt; ["готово","5"].

<a id="09-05"></a>
## 09-05. Часы для повторяемого теста

Уровень 1 · Проектирование тестов · Навык: Замена изменчивой зависимости фиксированными часами

Создайте IClock { DateTime Now { get; } }, FixedClock(DateTime now) и Greeting(IClock clock) с string Get(): для Hour&lt;12 возвращать "утро", иначе "день". clock не null. Используйте только внедрённое время. Напишите тесты на 00:00, 11:59, 12:00, 23:59. Отдельно выполните ошибочный вариант Hour&lt;=12 и покажите расхождение ровно в 12:00. Реальные часы и ожидания времени не использовать.

**Пример и ожидаемое поведение**

FixedClock(2030-01-01 11:59) -&gt; "утро"; 12:00 -&gt; "день".

<a id="09-06"></a>
## 09-06. Общая рамка расчёта доставки

Уровень 2 · Реализация · Навык: Абстрактный класс и шаблонный метод

Создайте public abstract class ShippingPolicy с public decimal Quote(decimal kilograms), protected abstract decimal VariableCost(decimal kilograms), protected abstract decimal BaseFee {get;}, protected abstract decimal Minimum {get;}. Quote возвращает max(Minimum,BaseFee+VariableCost(kilograms)) и не виртуален. kilograms в 0..10000. Ground: BaseFee=20, Minimum=50, переменная часть 5*kg. Air: BaseFee=100, Minimum=100, переменная часть 20*kg. Создайте sealed реализации. Внешний код работает через ShippingPolicy; округление не нужно.

**Пример и ожидаемое поведение**

Ground.Quote(2) -&gt; 50, Ground.Quote(10) -&gt; 70; Air.Quote(2) -&gt; 140.

<a id="09-07"></a>
## 09-07. Перегрузка не заменила виртуальный метод

Уровень 2 · Поиск ошибки · Навык: Совпадение сигнатуры при override

Воспроизведите ошибку и исправьте public sealed class Dog : Animal так, чтобы вызов Speak() через Animal возвращал "woof". Тип Animal и ошибочный BrokenDog сохраняются для проверки. Исходный код:
```csharp
public class Animal { public virtual string Speak() => "unknown"; }
public sealed class BrokenDog : Animal
{
    public string Speak(string suffix) => "woof" + suffix;
}
Animal animal = new BrokenDog();
// animal.Speak() ?
```
В исправлении не используйте проверку типа, приведение или new. Дополнительные входы отсутствуют.

**Пример и ожидаемое поведение**

Исходный animal.Speak() -&gt; "unknown"; исправленный new Dog() через Animal -&gt; "woof".

<a id="09-08"></a>
## 09-08. Узкий интерфейс для читателя

Уровень 2 · Рефакторинг · Навык: Разделение контрактов чтения и записи

Переработайте зависимость функции отчёта. Создайте IReadName { string Read(); }, IWriteName { void Write(string value); }, MemoryName(string initial), реализующий оба интерфейса, и public static string BuildHeading(IReadName source), возвращающий "Имя: " + source.Read(). Все ссылки и строки не null. Отчёт должен компилироваться с объектом, который умеет только читать. Исходный чрезмерный контракт:
```csharp
public interface INameStore { string Read(); void Write(string value); }
// BuildHeading требует INameStore, хотя никогда не пишет.
``` Создайте также public sealed class ReadOnlyName(string value), реализующий только IReadName и возвращающий переданное значение; допустим обычный эквивалентный конструктор.

**Пример и ожидаемое поведение**

MemoryName("A") -&gt; "Имя: A"; после Write("B") -&gt; "Имя: B"; ReadOnlyName("C") тоже подходит.

<a id="09-09"></a>
## 09-09. Интерфейс не отменяет владение данными

Уровень 2 · Реализация · Навык: Контракт снимка при передаче массива через абстракцию

Объявите IIntRepository { void Save(int[] values); int[] Load(); } и MemoryRepository. Массивы не null, длина 0..10000. До Save метод Load возвращает новый пустой массив; Save полностью заменяет данные. Контракт требует изоляции: изменение исходного массива после Save и изменение результата Load не влияют на хранилище. Использование через IIntRepository должно сохранять эти гарантии. LINQ не применять.

**Пример и ожидаемое поведение**

Save([1,2]); изменить исходный[0]=9; Load() -&gt; [1,2]; изменить загруженный[1]=8; следующий Load() -&gt; [1,2].

<a id="09-10"></a>
## 09-10. Один контракт для листа и папки

Уровень 2 · Рефакторинг · Навык: Композиция дерева через интерфейс вместо фиксированных уровней

Замените расчёт «папка с файлами только одного уровня» моделью произвольного дерева. Объявите INode { long Size { get; } }, FileNode(long size), FolderNode с void Add(INode node) и вычисляемым Size. Размер листа в 0..1000000000; узлы не null, общее число до 1000, глубина до 100; вход образует дерево без циклов и без повторного владения узлом. Пустая папка имеет размер 0. Внешний код суммирует через INode, не разбирая типы. Исходная ограниченная идея:
```csharp
long total = 0;
foreach (long fileSize in directFiles) total += fileSize;
// вложенная папка в эту модель не помещается
```

**Пример и ожидаемое поведение**

folder=[File(3), subfolder=[File(5),File(7)]] -&gt; 15; пустая папка -&gt; 0.

<a id="09-11"></a>
## 09-11. Добавление поведения обёрткой

Уровень 3 · Реализация · Навык: Декоратор интерфейса и порядок композиции

Объявите ITextTransform { string Transform(string text); }, Identity, TrimDecorator(ITextTransform inner), PrefixDecorator(ITextTransform inner,string prefix). Все аргументы не null; строки длиной до 1000. Identity возвращает текст; TrimDecorator обрезает края результата inner; PrefixDecorator добавляет prefix перед результатом inner. Реализуйте классы без наследования друг от друга и без проверки типов. Покажите, что порядок обёрток меняет результат.

**Пример и ожидаемое поведение**

Prefix(Trim(Identity)," &gt; ").Transform("  a  ") -&gt; " &gt; a"; Trim(Prefix(Identity," &gt; ")).Transform("  a  ") -&gt; "&gt;   a".

<a id="09-12"></a>
## 09-12. Резервный источник вызывается по необходимости

Уровень 3 · Реализация · Навык: Композиция Try-контрактов и ленивое обращение к запасной зависимости

Объявите IValueSource { bool TryGet(string key,out string value); }. key не null. При отсутствии источник возвращает false и value=""; пустая строка при true — найденное значение. Создайте DictionarySource(Dictionary&lt;string,string&gt; data) (словарь не null, Ordinal, не меняется снаружи во время работы) и FallbackSource(IValueSource primary,IValueSource backup). Обе зависимости не null. Fallback обращается к backup только при false от primary; при true сохраняет результат даже если он пустой. Проверьте число вызовов зависимостей тестовым источником.

**Пример и ожидаемое поведение**

primary={A:""}, backup={A:"запас",B:"второй"}: A -&gt; true,"" без вызова backup; B -&gt; true,"второй".

<a id="09-13"></a>
## 09-13. Два представления одного отчёта

Уровень 3 · Реализация · Навык: Явная реализация совпадающих членов интерфейсов

Создайте ICompactReport { string Render(); }, IDetailedReport { string Render(); } и sealed Report(string title,int count), реализующий оба интерфейса явно. title не null, count в 0..1000000. Компактный Render возвращает title, подробный — title+": "+count в инвариантной десятичной записи. Публичного Report.Render() не создавайте. Реализуйте public static string[] BothViews(Report report), возвращающий компактный и подробный результаты в таком порядке. report не null.

**Пример и ожидаемое поведение**

Report("Задачи",3) -&gt; ["Задачи","Задачи: 3"].

<a id="09-14"></a>
## 09-14. Совпадение сигнатур ещё не очередь

Уровень 3 · Проектирование тестов · Навык: Поведенческий контракт подстановки

Объявите IIntQueue { void Add(int value); bool TryTake(out int value); }. FIFO обязателен. На пустоте false и value=0. Создайте FifoQueue на Queue&lt;int&gt; и заведомо ошибочную LifoQueue на Stack&lt;int&gt; с теми же сигнатурами. Напишите public static bool SatisfiesContract(IIntQueue queue), принимающий новый пустой экземпляр и проверяющий пустоту, добавление 1,2, получение 1, добавление 3, получение 2,3 и конечную пустоту. В Run докажите, что FifoQueue проходит, LifoQueue отвергается. Числа любые int, до 1000 операций.

**Пример и ожидаемое поведение**

SatisfiesContract(new FifoQueue()) -&gt; true; new LifoQueue() -&gt; false.

<a id="09-15"></a>
## 09-15. Локальная служба уведомлений

Уровень 3 · Прикладная задача · Навык: Сборка приложения из трёх зависимостей вручную

Создайте IRecipients { string[] Get(); }, IMessageTemplate { string Build(string recipient); }, IChannel { bool TrySend(string recipient,string message); }, public readonly record struct DeliverySummary(int Attempted,int Succeeded) и NotificationService с конструктором из трёх указанных зависимостей. Все зависимости/результаты/строки не null; адресатов 0..1000, дубликаты значимы. Метод DeliverySummary SendAll() вызывает Get ровно раз, для каждого адресата по порядку Build и TrySend ровно раз, продолжает после false. Внешние сети не использовать: реализации тестовые в памяти. Пустой список -&gt; (0,0).

**Пример и ожидаемое поведение**

Адресаты ["a","b","a"], канал отвергает "b" -&gt; Attempted=3, Succeeded=2; три попытки в том же порядке.

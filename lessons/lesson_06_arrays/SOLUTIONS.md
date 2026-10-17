# Урок 6. Массивы, индексы и алгоритмы поиска: решения

Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.
Каждое решение запускается по ID; Run содержит исполняемые проверки.

<a id="06-01"></a>
## 06-01. Первый максимум

Сравнивайте очередной элемент с текущим максимумом через &gt;, сохраняя первый индекс при равенстве.

**Ход решения**

Для пустого массива нет допустимого индекса, поэтому возвращаем -1. Для непустого начинаем с индекса 0. Перед каждой итерацией best указывает на первый максимум уже просмотренного префикса. Строгое сравнение сохраняет этот инвариант и при дубликатах. Время O(n), дополнительная память O(1).

**Частая ошибка**

Инициализация максимума нулём неверна для полностью отрицательного массива; &gt;= выберет последний максимум.

Полный исходник: [solutions/Task01.cs](solutions/Task01.cs).

```csharp
namespace Course.Lesson06;

public static class Task01
{
    public static int FirstMaximumIndex(int[] values)
    {
        if (values.Length == 0) return -1;
        int best = 0;
        for (int i = 1; i < values.Length; i++)
            if (values[i] > values[best]) best = i;
        return best;
    }
    public static void Run()
    {
        Check.Equal(1, FirstMaximumIndex(new[] { 3, 8, 8, -2 }));
        Check.Equal(1, FirstMaximumIndex(new[] { -8, -2, -9 }));
        Check.Equal(-1, FirstMaximumIndex(Array.Empty<int>()));
        Check.Equal(0, FirstMaximumIndex(new[] { int.MinValue }));
    }
}
```

**Исполняемые проверки**

- Два одинаковых максимума выбирают первый
- Полностью отрицательные значения
- Пустой массив
- Один int.MinValue

<a id="06-02"></a>
## 06-02. Псевдоним и независимая копия

alias и a ссылаются на один массив; copy хранит отдельный массив значений.

**Ход решения**

Первое присваивание не копирует элементы. Поэтому alias[0] меняет a[0]. Clone создаёт новый массив; для int этого достаточно для независимости значений. Присваивание copy[1] не затрагивает a[1]. Итог [9,4,7] подтверждается выполнением точного фрагмента. Вариант без Clone даёт [9,7,7], что отдельная проверка различает.

**Частая ошибка**

Считать присваивание массива копированием содержимого; обобщать независимость Clone на вложенные ссылочные объекты.

Полный исходник: [solutions/Task02.cs](solutions/Task02.cs).

```csharp
namespace Course.Lesson06;

public static class Task02
{
    public static int[] Replay()
    {
        int[] a = { 2, 4 };
        int[] alias = a;
        int[] copy = (int[])a.Clone();
        alias[0] = 9;
        copy[1] = 7;
        return new[] { a[0], a[1], copy[1] };
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 9, 4, 7 }, Replay());
        int[] a = { 2, 4 };
        int[] alias = a;
        int[] copy = a;
        alias[0] = 9;
        copy[1] = 7;
        Check.SequenceEqual(new[] { 9, 7, 7 }, new[] { a[0], a[1], copy[1] });
        Check.True(a[1] != Replay()[1]);
    }
}
```

**Исполняемые проверки**

- Точный исходный фрагмент даёт [9,4,7]
- Ошибочный вариант без Clone даёт [9,7,7] и различается

<a id="06-03"></a>
## 06-03. Последний допустимый индекс

Условие должно быть i &lt; values.Length; аккумулятор long защищает сумму от переполнения int.

**Ход решения**

Длина — число элементов, а не допустимый индекс. Исходный цикл достигает i=Length; даже при пустом массиве он пытается обратиться к элементу 0. Исправленный цикл не входит в тело для пустого массива и посещает последний элемент ровно один раз. До 100000 int сумма помещается в long. Время O(n), память O(1).

**Частая ошибка**

Менять &lt;= на &lt; Length-1: это пропускает последний элемент. long total не исправит выход за границу само по себе.

Полный исходник: [solutions/Task03.cs](solutions/Task03.cs).

```csharp
namespace Course.Lesson06;

public static class Task03
{
    public static long Sum(int[] values)
    {
        long total = 0;
        for (int i = 0; i < values.Length; i++) total += values[i];
        return total;
    }
    public static void Run()
    {
        Check.Equal(10L, Sum(new[] { 5, -2, 7 }));
        Check.Equal(0L, Sum(Array.Empty<int>()));
        Check.Equal(4294967294L, Sum(new[] { int.MaxValue, int.MaxValue }));
        Check.Equal(-9L, Sum(new[] { -9 }));
    }
}
```

**Исполняемые проверки**

- Смешанные знаки
- Пустой массив
- Два int.MaxValue
- Одноэлементный массив

<a id="06-04"></a>
## 06-04. Стабильный отбор положительных

Сначала подсчитайте подходящие элементы, затем выделите массив и заполните его отдельным индексом.

**Ход решения**

Размер исходного массива не равен числу отобранных элементов. Первый проход находит точный размер. Во втором индекс write увеличивается только после записи положительного числа; порядок сохраняется автоматически. Проверка изменения результата доказывает, что исходный массив не является тем же объектом. Время O(n), память O(k).

**Частая ошибка**

Возвращать массив длины n с лишними нулями; сортировать перед фильтрацией и терять порядок.

Полный исходник: [solutions/Task04.cs](solutions/Task04.cs).

```csharp
namespace Course.Lesson06;

public static class Task04
{
    public static int[] PositiveCopy(int[] values)
    {
        int count = 0;
        foreach (int value in values) if (value > 0) count++;
        int[] result = new int[count];
        int write = 0;
        foreach (int value in values) if (value > 0) result[write++] = value;
        return result;
    }
    public static void Run()
    {
        int[] source = { -2, 4, 0, 3, 4 };
        int[] result = PositiveCopy(source);
        Check.SequenceEqual(new[] { 4, 3, 4 }, result);
        Check.SequenceEqual(Array.Empty<int>(), PositiveCopy(new[] { 0, -1 }));
        Check.SequenceEqual(Array.Empty<int>(), PositiveCopy(Array.Empty<int>()));
        result[0] = 99;
        Check.SequenceEqual(new[] { -2, 4, 0, 3, 4 }, source);
    }
}
```

**Исполняемые проверки**

- Порядок и дубликаты сохранены
- Ноль исключён
- Пустой массив
- Изменение результата не меняет источник

<a id="06-05"></a>
## 06-05. Тест, который замечает разворот

Меняйте местами симметричные элементы до середины, а тесты сравнивайте с заранее рассчитанными результатами.

**Ход решения**

Левая и правая части уже находятся на правильных местах; на каждом шаге границы сдвигаются к центру. Чётная длина проверяет встречу двух индексов, нечётная — нетронутый центр. Неравный своим обращению массив [1,2,3,4] отличает no-op. Время O(n), память O(1).

**Частая ошибка**

Проверять только палиндром или только свойство двух разворотов; оба теста пропускают отсутствие действий.

Полный исходник: [solutions/Task05.cs](solutions/Task05.cs).

```csharp
namespace Course.Lesson06;

public static class Task05
{
    public static void Reverse(int[] values)
    {
        for (int left = 0, right = values.Length - 1; left < right; left++, right--)
        {
            int temporary = values[left];
            values[left] = values[right];
            values[right] = temporary;
        }
    }
    public static void Run()
    {
        int[] empty = Array.Empty<int>(); Reverse(empty);
        Check.SequenceEqual(Array.Empty<int>(), empty);
        int[] one = { 6 }; Reverse(one); Check.SequenceEqual(new[] { 6 }, one);
        int[] even = { 1, 2, 3, 4 }; Reverse(even);
        Check.SequenceEqual(new[] { 4, 3, 2, 1 }, even);
        int[] odd = { 1, 1, 3 }; Reverse(odd); Check.SequenceEqual(new[] { 3, 1, 1 }, odd);
        int[] wrongNoOp = { 1, 2, 3, 4 };
        Check.True(wrongNoOp[0] != even[0], "Асимметричный тест различает no-op.");
    }
}
```

**Исполняемые проверки**

- Пустой и один элемент
- Чётная и нечётная длина
- Дубликаты
- Явное различение ошибочного no-op

<a id="06-06"></a>
## 06-06. Циклический сдвиг

Для непустого массива уменьшите k по модулю длины и вычислите новый индекс каждого элемента.

**Ход решения**

Каждый элемент получает ровно одно место, поскольку добавление одного и того же остатка задаёт перестановку индексов. k может быть большим, но нормализованный шаг меньше n, поэтому i+shift мал. Пустой массив возвращаем до операции %. Сложность O(n), память O(n).

**Частая ошибка**

Вычислять k%n до проверки n=0; сдвигать влево вместо вправо; менять источник.

Полный исходник: [solutions/Task06.cs](solutions/Task06.cs).

```csharp
namespace Course.Lesson06;

public static class Task06
{
    public static int[] RotateRight(int[] values, int k)
    {
        int n = values.Length;
        int[] result = new int[n];
        if (n == 0) return result;
        int shift = k % n;
        for (int i = 0; i < n; i++) result[(i + shift) % n] = values[i];
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 40, 10, 20, 30 }, RotateRight(new[] { 10, 20, 30, 40 }, 5));
        int[] source = { 1, 2 }; int[] copy = RotateRight(source, 0); copy[0] = 9;
        Check.Equal(1, source[0]);
        Check.SequenceEqual(Array.Empty<int>(), RotateRight(Array.Empty<int>(), 7));
        Check.SequenceEqual(new[] { 2, 1 }, RotateRight(new[] { 1, 2 }, int.MaxValue));
    }
}
```

**Исполняемые проверки**

- Шаг больше длины
- Нулевой шаг создаёт независимую копию
- Пустой массив
- Максимальный k

<a id="06-07"></a>
## 06-07. Кодирование серий

Подсчитайте число начал серий; затем запишите значение каждой серии и увеличивайте её длину.

**Ход решения**

Началом серии является индекс 0 либо индекс, чей элемент отличается от предыдущего. В первом проходе это даёт число строк результата. Во втором новая серия открывает строку, каждый элемент добавляет один к её счётчику. Разделённые двойки остаются в двух разных сериях. Время O(n), память O(r).

**Частая ошибка**

Сортировка или общий подсчёт частот меняет смысл непрерывной серии.

Полный исходник: [solutions/Task07.cs](solutions/Task07.cs).

```csharp
namespace Course.Lesson06;

public static class Task07
{
    public static int[,] EncodeRuns(int[] values)
    {
        int runs = 0;
        for (int i = 0; i < values.Length; i++) if (i == 0 || values[i] != values[i - 1]) runs++;
        int[,] result = new int[runs, 2];
        int row = -1;
        for (int i = 0; i < values.Length; i++)
        {
            if (i == 0 || values[i] != values[i - 1]) { row++; result[row, 0] = values[i]; }
            result[row, 1]++;
        }
        return result;
    }
    public static void Run()
    {
        int[,] encoded = EncodeRuns(new[] { 2, 2, 5, 2, 2, 2 });
        Check.Equal(3, encoded.GetLength(0));
        Check.SequenceEqual(new[] { 2, 2, 5, 1, 2, 3 }, Flatten(encoded));
        Check.Equal(4, EncodeRuns(new[] { 7, 7, 7, 7 })[0, 1]);
        int[,] empty = EncodeRuns(Array.Empty<int>());
        Check.Equal(0, empty.GetLength(0)); Check.Equal(2, empty.GetLength(1));
        Check.Equal(3, EncodeRuns(new[] { 1, 2, 1 }).GetLength(0));
    }

    private static int[] Flatten(int[,] matrix)
    {
        int[] result = new int[matrix.Length]; int index = 0;
        foreach (int value in matrix) result[index++] = value;
        return result;
    }
}
```

**Исполняемые проверки**

- Три серии с повторным значением
- Одна серия
- Пустая матрица формы 0x2
- Чередование значений

<a id="06-08"></a>
## 06-08. Слияние без повторной сортировки

Сравнивайте первые ещё не перенесённые элементы; как только один массив кончится, перенесите остаток второго.

**Ход решения**

Каждый источник уже отсортирован, поэтому минимальный оставшийся элемент находится в одной из двух голов. Выбранный указатель увеличивается, а другой остаётся. В результате каждый элемент читается и записывается один раз. Дубликаты не пропадают, так как равенство переносит только один элемент за шаг. O(n+m) времени и памяти.

**Частая ошибка**

При равенстве увеличивать оба указателя, теряя дубликат; забывать остаток длинного массива.

Полный исходник: [solutions/Task08.cs](solutions/Task08.cs).

```csharp
namespace Course.Lesson06;

public static class Task08
{
    public static int[] MergeSorted(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0, j = 0, write = 0;
        while (i < left.Length && j < right.Length)
            result[write++] = left[i] <= right[j] ? left[i++] : right[j++];
        while (i < left.Length) result[write++] = left[i++];
        while (j < right.Length) result[write++] = right[j++];
        return result;
    }
    public static void Run()
    {
        Check.SequenceEqual(new[] { 1, 2, 3, 3, 3 }, MergeSorted(new[] { 1, 3, 3 }, new[] { 2, 3 }));
        Check.SequenceEqual(new[] { 4 }, MergeSorted(Array.Empty<int>(), new[] { 4 }));
        Check.SequenceEqual(Array.Empty<int>(), MergeSorted(Array.Empty<int>(), Array.Empty<int>()));
        Check.SequenceEqual(new[] { -4, -2, 1, 5 }, MergeSorted(new[] { -2, 1, 5 }, new[] { -4 }));
    }
}
```

**Исполняемые проверки**

- Пересекающиеся значения и дубликаты
- Левый пуст
- Оба пусты
- Правый заканчивается первым

<a id="06-09"></a>
## 06-09. Самое левое совпадение

Найдите первую позицию со значением не меньше target; затем проверьте равенство.

**Ход решения**

Всё левее lo строго меньше target, всё начиная с hi не меньше target. Проверка середины уменьшает интервал, не исключая возможное самое левое совпадение. В конце lo=hi. Эта позиция может равняться Length, поэтому сначала проверяем границу, затем значение. O(log n), память O(1).

**Частая ошибка**

Возвращать середину сразу при равенстве — индекс может оказаться не первым; читать values[Length].

Полный исходник: [solutions/Task09.cs](solutions/Task09.cs).

```csharp
namespace Course.Lesson06;

public static class Task09
{
    public static int FirstIndexSorted(int[] values, int target)
    {
        int lo = 0, hi = values.Length;
        while (lo < hi)
        {
            int middle = lo + (hi - lo) / 2;
            if (values[middle] < target) lo = middle + 1;
            else hi = middle;
        }
        return lo < values.Length && values[lo] == target ? lo : -1;
    }
    public static void Run()
    {
        Check.Equal(1, FirstIndexSorted(new[] { 1, 2, 2, 2, 8 }, 2));
        Check.Equal(-1, FirstIndexSorted(new[] { 1, 2 }, 3));
        Check.Equal(-1, FirstIndexSorted(Array.Empty<int>(), 1));
        Check.Equal(0, FirstIndexSorted(new[] { int.MinValue, 0 }, int.MinValue));
        Check.Equal(-1, FirstIndexSorted(new[] { 1, 4 }, 2));
    }
}
```

**Исполняемые проверки**

- Серия из трёх совпадений
- Значение больше всех
- Пустой массив
- Минимальный int и левый край
- Значение между соседями

<a id="06-10"></a>
## 06-10. Среднее без скрытого переполнения

Суммируйте в long и выполните (double)total / Length после обработки пустого массива.

**Ход решения**

Исходный total может переполниться, а деление двух int отбрасывает дробную часть до присваивания double. long помещает сумму допустимого числа элементов. Приведение одного операнда перед / делает деление вещественным. Пустой массив обрабатывается явно. O(n), память O(1).

**Частая ошибка**

Приведение (double)(total / Length) делается слишком поздно; NaN нельзя проверять равенством.

Полный исходник: [solutions/Task10.cs](solutions/Task10.cs).

```csharp
namespace Course.Lesson06;

public static class Task10
{
    public static double Average(int[] values)
    {
        if (values.Length == 0) return double.NaN;
        long total = 0;
        foreach (int value in values) total += value;
        return (double)total / values.Length;
    }
    public static void Run()
    {
        Check.Near(1.5, Average(new[] { 1, 2 }));
        Check.Near(int.MaxValue, Average(new[] { int.MaxValue, int.MaxValue }));
        Check.True(double.IsNaN(Average(Array.Empty<int>())));
        Check.Near(-2.5, Average(new[] { -2, -3 }));
    }
}
```

**Исполняемые проверки**

- Дробное среднее
- Сумма больше int.MaxValue
- Пустота через double.IsNaN
- Отрицательные значения

<a id="06-11"></a>
## 06-11. Строка с максимальной суммой

По каждой строке независимо сложите столбцы, затем сравните сумму с лучшей.

**Ход решения**

GetLength(0) задаёт строки, GetLength(1) — столбцы. В первой непустой строке устанавливаем начальный максимум, чтобы отрицательные суммы не проиграли выдуманному нулю. Строгое сравнение сохраняет первый индекс. Время O(r*c), память O(1).

**Частая ошибка**

Использовать Length как число строк; один общий total без сброса на новой строке; переполнение суммы.

Полный исходник: [solutions/Task11.cs](solutions/Task11.cs).

```csharp
namespace Course.Lesson06;

public static class Task11
{
    public static int MaxSumRow(int[,] matrix)
    {
        int rows = matrix.GetLength(0), columns = matrix.GetLength(1);
        if (rows == 0 || columns == 0) return -1;
        long bestSum = long.MinValue; int best = -1;
        for (int row = 0; row < rows; row++)
        {
            long total = 0;
            for (int column = 0; column < columns; column++) total += matrix[row, column];
            if (total > bestSum) { bestSum = total; best = row; }
        }
        return best;
    }
    public static void Run()
    {
        Check.Equal(1, MaxSumRow(new[,] { { 1, 2, 3 }, { 8, -1, 0 } }));
        Check.Equal(1, MaxSumRow(new[,] { { -9, -1 }, { -2, -3 } }));
        Check.Equal(0, MaxSumRow(new[,] { { 1, 2 }, { 2, 1 } }));
        Check.Equal(-1, MaxSumRow(new int[2, 0])); Check.Equal(-1, MaxSumRow(new int[0, 2]));
        Check.Equal(0, MaxSumRow(new[,] { { int.MaxValue, int.MaxValue }, { 1, 2 } }));
    }
}
```

**Исполняемые проверки**

- Прямоугольная матрица
- Отрицательные суммы
- Равенство сохраняет первую строку
- Ноль строк или столбцов
- Сумма двух int.MaxValue

<a id="06-12"></a>
## 06-12. Транспонирование прямоугольника

Поменяйте местами размеры нового массива и координаты при записи.

**Ход решения**

В прямоугольной матрице число строк и столбцов различается. Перестановка только индексов без перестановки формы вызовет ошибку. Циклы по исходной форме посещают каждую ячейку один раз. Проверка повторного транспонирования возвращает значения и форму исходника. O(r*c) времени и памяти.

**Частая ошибка**

Проверять только квадрат 2x2: неверная форма останется незамеченной; менять исходник.

Полный исходник: [solutions/Task12.cs](solutions/Task12.cs).

```csharp
namespace Course.Lesson06;

public static class Task12
{
    public static int[,] Transpose(int[,] matrix)
    {
        int[,] result = new int[matrix.GetLength(1), matrix.GetLength(0)];
        for (int row = 0; row < matrix.GetLength(0); row++)
            for (int column = 0; column < matrix.GetLength(1); column++)
                result[column, row] = matrix[row, column];
        return result;
    }
    public static void Run()
    {
        int[,] source = { { 1, 2, 3 }, { 4, 5, 6 } };
        int[,] result = Transpose(source);
        Check.Equal(3, result.GetLength(0)); Check.Equal(2, result.GetLength(1));
        Check.Equal(5, result[1, 1]); Check.Equal(3, result[2, 0]);
        int[,] empty = Transpose(new int[0, 3]);
        Check.Equal(3, empty.GetLength(0)); Check.Equal(0, empty.GetLength(1));
        int[,] twice = Transpose(result); Check.Equal(6, twice[1, 2]);
        result[0, 0] = 99; Check.Equal(1, source[0, 0]);
    }
}
```

**Исполняемые проверки**

- Матрица 2x3 и значения
- Нулевая размерность 0x3
- Двойное транспонирование
- Независимость копии

<a id="06-13"></a>
## 06-13. Строгие вершины сетки

Для каждой ячейки независимо исключайте вершину, когда любой допустимый сосед &gt;= неё.

**Ход решения**

На границе меньше соседей, поэтому каждое сравнение сначала проверяет координату. Пропущенное направление создаёт ложные вершины, а &gt; вместо &gt;= принимает плато. Для 1x1 ни одно исключающее условие не выполняется: универсальное условие истинно. O(r*c), маска O(r*c).

**Частая ошибка**

Сравнивать диагонали вопреки условию; приравнивать вершину к глобальному максимуму.

Полный исходник: [solutions/Task13.cs](solutions/Task13.cs).

```csharp
namespace Course.Lesson06;

public static class Task13
{
    public static bool[,] StrictPeaks(int[,] matrix)
    {
        int rows = matrix.GetLength(0), columns = matrix.GetLength(1);
        bool[,] result = new bool[rows, columns];
        for (int r = 0; r < rows; r++) for (int c = 0; c < columns; c++)
        {
            int value = matrix[r, c]; bool peak = true;
            if (r > 0 && matrix[r - 1, c] >= value) peak = false;
            if (r + 1 < rows && matrix[r + 1, c] >= value) peak = false;
            if (c > 0 && matrix[r, c - 1] >= value) peak = false;
            if (c + 1 < columns && matrix[r, c + 1] >= value) peak = false;
            result[r, c] = peak;
        }
        return result;
    }
    public static void Run()
    {
        bool[,] result = StrictPeaks(new[,] { { 4, 1 }, { 2, 3 } });
        Check.True(result[0, 0] && result[1, 1] && !result[0, 1] && !result[1, 0]);
        bool[,] plateau = StrictPeaks(new[,] { { 5, 5 } }); Check.True(!plateau[0, 0] && !plateau[0, 1]);
        Check.True(StrictPeaks(new[,] { { int.MinValue } })[0, 0]);
        Check.Equal(0, StrictPeaks(new int[0, 2]).Length);
        Check.True(!StrictPeaks(new[,] { { 2 }, { 3 } })[0, 0]);
    }
}
```

**Исполняемые проверки**

- Две диагональные локальные вершины
- Плато из равных соседей
- Одна ячейка
- Пустая форма
- Край с большим нижним соседом

<a id="06-14"></a>
## 06-14. Перекрывающийся сдвиг

Идите с конца, чтобы источник каждой записи ещё не был затёрт.

**Ход решения**

Исходный цикл читает уже изменённые ячейки: после первой записи [1,1,3,4], затем [1,1,1,4], затем [1,1,1,1]; после заполнения [0,1,1,1]. Обход справа налево сохраняет непрочитанный префикс. places=0 оставляет массив как есть, places=n делает только заполнение. Время O(n), память O(1).

**Частая ошибка**

Исправить только первый индекс; направление обхода существенно при перекрытии.

Полный исходник: [solutions/Task14.cs](solutions/Task14.cs).

```csharp
namespace Course.Lesson06;

public static class Task14
{
    public static void ShiftRight(int[] values, int places)
    {
        for (int i = values.Length - 1; i >= places; i--) values[i] = values[i - places];
        for (int i = 0; i < places; i++) values[i] = 0;
    }
    public static void Run()
    {
        int[] wrong = { 1, 2, 3, 4 }; int places = 1;
        for (int i = places; i < wrong.Length; i++) wrong[i] = wrong[i - places];
        for (int i = 0; i < places; i++) wrong[i] = 0;
        Check.SequenceEqual(new[] { 0, 1, 1, 1 }, wrong);
        int[] right = { 1, 2, 3, 4 }; ShiftRight(right, 1); Check.SequenceEqual(new[] { 0, 1, 2, 3 }, right);
        ShiftRight(right, 4); Check.SequenceEqual(new[] { 0, 0, 0, 0 }, right);
        int[] unchanged = { 5, 6 }; ShiftRight(unchanged, 0); Check.SequenceEqual(new[] { 5, 6 }, unchanged);
        int[] empty = Array.Empty<int>(); ShiftRight(empty, 0); Check.Equal(0, empty.Length);
    }
}
```

**Исполняемые проверки**

- Точный ошибочный фрагмент даёт [0,1,1,1]
- Сдвиг на один исправлен
- Полный и нулевой сдвиги
- Пустой массив

<a id="06-15"></a>
## 06-15. Семья в одном ряду

Сбрасывайте длину серии на занятом месте и при начале нового ряда.

**Ход решения**

Обход строк по возрастанию сразу обеспечивает приоритет ряда. Внутри строки первое достижение нужной длины соответствует самому левому блоку; его начало c-groupSize+1. Счётчик начинается с нуля в каждом ряду, поэтому перенос через границу невозможен. O(r*c), дополнительная память O(1), кроме двухэлементного ответа.

**Частая ошибка**

Продолжать серию между рядами или искать только полные ряды; менять карту при поиске.

Полный исходник: [solutions/Task15.cs](solutions/Task15.cs).

```csharp
namespace Course.Lesson06;

public static class Task15
{
    public static int[] FindSeatBlock(bool[,] occupied, int groupSize)
    {
        for (int row = 0; row < occupied.GetLength(0); row++)
        {
            int run = 0;
            for (int column = 0; column < occupied.GetLength(1); column++)
            {
                run = occupied[row, column] ? 0 : run + 1;
                if (run == groupSize) return new[] { row, column - groupSize + 1 };
            }
        }
        return Array.Empty<int>();
    }
    public static void Run()
    {
        bool[,] map = { { false, true, false, false }, { false, false, false, true } };
        Check.SequenceEqual(new[] { 0, 2 }, FindSeatBlock(map, 2));
        Check.SequenceEqual(Array.Empty<int>(), FindSeatBlock(map, 4));
        Check.SequenceEqual(Array.Empty<int>(), FindSeatBlock(new[,] { { true, false }, { false, true } }, 2));
        Check.SequenceEqual(Array.Empty<int>(), FindSeatBlock(new bool[0, 3], 1));
        Check.SequenceEqual(new[] { 0, 0 }, FindSeatBlock(map, 1));
        Check.True(!map[0, 2]);
    }
}
```

**Исполняемые проверки**

- Первый блок в первой подходящей строке
- Группа шире всех свободных блоков
- Нет переноса серии между рядами
- Нулевая высота
- Группа из одного

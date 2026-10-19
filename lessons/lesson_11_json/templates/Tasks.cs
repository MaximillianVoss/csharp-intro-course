// Авторский предметный каркас. Модели и API заданы; алгоритмы и проверки предстоит написать.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Course.Lesson11;
// 11-01. Карточка в JSON
// Условие: ../TASKS.md, раздел 11-01.
public static class Task01
{
    public sealed class Card
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }

    public static string Serialize(Card card)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте точное восстановление имени после разбора JSON и самостоятельные ошибки модели.
        throw new NotImplementedException();
    }
}

// 11-02. Чтение показания
// Условие: ../TASKS.md, раздел 11-02.
public static class Task02
{
    public sealed class Reading
    {
        public decimal Value { get; set; }
        public string? Unit { get; set; }
    }

    public static Reading Read(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Сопоставьте допустимый JSON с моделью, затем независимо проверьте синтаксис, тип и доменные ограничения.
        throw new NotImplementedException();
    }
}

// 11-03. Что действительно прочитано
// Условие: ../TASKS.md, раздел 11-03.
public static class Task03
{
    public sealed class Settings
    {
        public int Limit { get; set; } = 9;

        public int Counter = 4;
    }

    public static (int Limit, int Counter) Observe(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: До запуска запишите прогноз для исходника и контрольного JSON с другим регистром свойства.
        throw new NotImplementedException();
    }
}

// 11-04. Исправление времени жизни JsonElement
// Условие: ../TASKS.md, раздел 11-04.
public static class Task04
{
    public static JsonElement ReadDetached(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Воспроизведите ошибку исходного using, затем проверьте чтение вложенных значений после возврата метода.
        throw new NotImplementedException();
    }
}

// 11-05. Нет поля и поле null
// Условие: ../TASKS.md, раздел 11-05.
public static class Task05
{
    public static string Label(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Подберите независимые входы для отсутствия, null, пустой строки и неверного типа.
        throw new NotImplementedException();
    }
}

// 11-06. Настройки в собственном файле
// Условие: ../TASKS.md, раздел 11-06.
public static class Task06
{
    public sealed class Settings
    {
        public int Volume { get; set; } = 50;
    }

    public static void Save(string path, Settings settings)
    {
        throw new NotImplementedException();
    }

    public static Settings Load(string path)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Работайте в собственном временном каталоге; отдельно проверьте отсутствие файла и повреждение существующего.
        throw new NotImplementedException();
    }
}

// 11-07. Сумма вложенного заказа
// Условие: ../TASKS.md, раздел 11-07.
public static class Task07
{
    public sealed class Order
    {
        public Line?[]? Lines { get; set; }
    }

    public sealed class Line
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public static decimal Total(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте корень, коллекцию и элемент как три самостоятельные границы, затем точную денежную сумму.
        throw new NotImplementedException();
    }
}

// 11-08. Координаты без DTO
// Условие: ../TASKS.md, раздел 11-08.
public static class Task08
{
    public static (double Lat, double Lon) ReadPoint(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте обе координаты на границах и число с чрезмерной экспонентой.
        throw new NotImplementedException();
    }
}

// 11-09. Убираем склейку JSON
// Условие: ../TASKS.md, раздел 11-09.
public static class Task09
{
    public static string BuildMessage(string text)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Покажите вход, ломающий склейку; проверяйте восстановленный текст и количество свойств объекта.
        throw new NotImplementedException();
    }
}

// 11-10. Тесты сохранения порядка
// Условие: ../TASKS.md, раздел 11-10.
public static class Task10
{
    public static int[] RoundTrip(int[] values)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Выберите вход, на котором сортировка заметна, и отдельно проверьте независимость массивов.
        throw new NotImplementedException();
    }
}

// 11-11. Миграция версии настроек
// Условие: ../TASKS.md, раздел 11-11.
public static class Task11
{
    public sealed record Config(string Name, int Retries);
    public static Config Read(string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Составьте отдельные примеры каждой версии и ошибки обязательных полей; не угадывайте формат по label.
        throw new NotImplementedException();
    }
}

// 11-12. Построчный импорт NDJSON
// Условие: ../TASKS.md, раздел 11-12.
public static class Task12
{
    public sealed class Item
    {
        public int Id { get; set; }
    }

    public sealed class Report
    {
        public List<int> Ids { get; } = new();
        public List<int> ErrorLines { get; } = new();
    }

    public static Report Import(TextReader reader)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте номера после пустых строк, сохранение повторов и отличие ошибки записи от отказа чтения.
        throw new NotImplementedException();
    }
}

// 11-13. Статус как строка
// Условие: ../TASKS.md, раздел 11-13.
public static class Task13
{
    public enum Status
    {
        Pending,
        Done
    }

    public static Status Read(string json)
    {
        throw new NotImplementedException();
    }

    public static string Write(Status status)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Разделите пробелы синтаксиса JSON и содержимого строки; проверьте запрещённые составные имена.
        throw new NotImplementedException();
    }
}

// 11-14. Частичное обновление профиля
// Условие: ../TASKS.md, раздел 11-14.
public static class Task14
{
    public sealed record Profile(string Name, string? Bio);
    public static Profile Patch(Profile original, string json)
    {
        throw new NotImplementedException();
    }

    public static void Run()
    {
        // TODO: Проверьте различие пропуска и сброса поля; ошибка последнего поля не должна менять исходный профиль.
        throw new NotImplementedException();
    }
}

// 11-15. Каталог с безопасным импортом
// Условие: ../TASKS.md, раздел 11-15.
public static class Task15
{
    public sealed class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public sealed class Catalog
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Import(string json)
        {
            throw new NotImplementedException();
        }

        public string Export()
        {
            throw new NotImplementedException();
        }
    }

    public static void Run()
    {
        // TODO: Сохраните экспорт до ошибочного позднего элемента и сравните весь снимок после отказа.
        throw new NotImplementedException();
    }
}

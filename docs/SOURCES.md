# Источники и подход

Материалы написаны для этого курса. Условия, решения и тесты образуют собственный учебный комплект.
Структура «теория → пример → самостоятельная практика → подробный разбор → проверка»
ориентируется на [cpp-intro-course](https://github.com/MaximillianVoss/cpp-intro-course).
Доступность этого репозитория определяется его владельцем; для прохождения C#-курса он не нужен.

При подготовке были изучены локальные уроки о типах и ООП, методика,
а также README, дерево и характерные исходники удалённого C++-репозитория.
Удалённая версия образца c49d7cafae0e8e55f216d1de1b388a941a3bf552
отличалась от локального расширенного варианта с 650 задачами.
Исходный C++ checkout использовался только для чтения.

В C# темы перенесены по смыслу: управляемые ссылочные объекты, IDisposable/using для внешних ресурсов,
nullable, decimal, System.Text.Json, делегаты, LINQ, async/await.
Указатели и RAII не используются как искусственная основа начального C#.

## Официальная документация

- [C# language documentation](https://learn.microsoft.com/en-us/dotnet/csharp/) — язык и ссылки на справочник.
- [Типы C#](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/) — значения, ссылки и система типов.
- [Строки](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/) — строковые операции.
- [Коллекции](https://learn.microsoft.com/en-us/dotnet/standard/collections/) — выбор структур данных.
- [Исключения](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/) — обработка ошибок.
- [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) — JSON.
- [Обобщения](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/generics) — generic-типы.
- [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) — запросы к данным.
- [Асинхронное программирование](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/) — async/await.
- [C# и MSTest](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-mstest) — тестовые проекты.
- [Установка .NET на Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows) — SDK и совместимость IDE.
- [Поддержка .NET](https://learn.microsoft.com/en-us/dotnet/core/releases-and-support) — выбор .NET 10 LTS.

Официальные страницы проверены при подготовке 17.10.2026.
Сроки поддержки, версии SDK и интерфейс установщика могут изменяться: при последующей установке сверяйтесь с Microsoft.

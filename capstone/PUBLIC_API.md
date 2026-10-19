# Публичный API для учебной приёмки

Пространство имён — `Course.Library`. Проект урока 16 уже ссылается на
`Библиотека.Ядро`. Для отдельного тестового проекта добавьте ссылку на
`capstone/Библиотека.Ядро/Библиотека.Ядро.csproj`; нужен .NET 10.

Сигнатуры ниже описывают существующий API, а не код для повторной реализации.

```csharp
public sealed class CliApplication
{
    public CliApplication(Func<string, ILibraryStore> storeFactory);
    public Task<int> RunAsync(string[] args, TextWriter output,
        TextWriter error, CancellationToken token = default);
}

public interface ILibraryStore
{
    Task<LibraryCatalog> LoadAsync(CancellationToken token = default);
    Task SaveAsync(LibraryCatalog catalog, CancellationToken token = default);
}

public sealed class JsonLibraryStore : ILibraryStore
{
    public JsonLibraryStore(string path);
    public Task<LibraryCatalog> LoadAsync(CancellationToken token = default);
    public Task SaveAsync(LibraryCatalog catalog, CancellationToken token = default);
}
```

Минимальная настройка приложения:

```csharp
var app = new Course.Library.CliApplication(
    path => new Course.Library.JsonLibraryStore(path));
using var output = new StringWriter();
using var error = new StringWriter();
// Сформируйте args по условию задачи и SPEC.md, затем:
// int exitCode = await app.RunAsync(args, output, error);
// Сравните exitCode, output.ToString(), error.ToString() и состояние файла.
```

`RunAsync` принимает аргументы без имени исполняемого файла. Путь передаётся
двумя элементами `"--data", absolutePath`. Команды и коды завершения определены
в [SPEC.md](SPEC.md). Новый экземпляр `CliApplication` с той же фабрикой и тем же
путём читает сохранённые данные с диска. Дождитесь каждого вызова через `await`;
параллельная запись в один файл в контракт не входит.

В приёмке используйте собственный уникальный временный каталог. Удаляйте в
`finally` только его; не подставляйте пользовательскую библиотеку. Полный
сценарий и его ожидаемые ответы ученик составляет по условию 16-15.

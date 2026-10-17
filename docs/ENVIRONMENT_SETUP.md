# Настройка среды

## Требования

Основной путь курса — Windows 11, .NET **SDK** 10, PowerShell и редактор.
SDK включает инструменты сборки; одного Runtime недостаточно.
.NET 10 выбран как LTS; согласно Microsoft, поддержка продолжается до ноября 2028 года.
Перед установкой проверьте актуальную [политику поддержки](https://learn.microsoft.com/en-us/dotnet/core/releases-and-support).

Установите актуальный стабильный SDK 10 с [официальной страницы](https://dotnet.microsoft.com/download/dotnet/10.0).
Для x64-компьютера выберите Windows x64 SDK. Затем откройте новый терминал:

```powershell
dotnet --info
dotnet --list-sdks
dotnet --version
```

Внутри курса должен выбираться SDK 10.0.xxx. `global.json` допускает стабильные feature bands 10.0,
начиная с 10.0.100; переход на другую основную версию автоматически не разрешён.
Проверенная локальная версия фиксируется в отчёте проверки, а не считается обязательной для всех учеников.

## Получение курса

Через Git:

```powershell
git clone https://github.com/MaximillianVoss/csharp-intro-course.git
cd csharp-intro-course
dotnet build "Курс CSharp.slnx" -c Release
```

Без Git: на GitHub выберите Code → Download ZIP, распакуйте полностью и откройте терминал в корне.
Корень содержит README.md, global.json и «Курс CSharp.slnx».
Папка может иметь русское имя; пути с пробелами всегда заключайте в кавычки.

## Первый запуск и практика

```powershell
dotnet run --project "lessons/lesson_01_basics/examples/Урок01.Примеры.csproj"
dotnet run --project "lessons/lesson_01_basics/templates/Урок01.Заготовки.csproj" -- --list
dotnet run --project "lessons/lesson_01_basics/solutions/Урок01.Решения.csproj" -- 01-01
```

Сначала прочитайте условие и напишите свою попытку в templates/Tasks.cs.
Вызов заготовки с ID до выполнения работы намеренно заканчивается NotImplementedException.
Не переносите готовый код в заготовку до собственной попытки.

## Тесты и интернет

```powershell
dotnet test "capstone/Библиотека.Тесты/Библиотека.Тесты.csproj" -c Release
```

Первое восстановление загружает MSTest 4.1.0 и его зависимости с NuGet.
Версии зависимостей сохранены в packages.lock.json.
Повторный запуск после восстановления может работать без сети с `--no-restore`.
Не добавляйте этот флаг при самой первой сборке чистой копии.

## Linux и macOS

Исходники используют переносимые .NET API. На Linux/macOS установите SDK 10 для своей ОС,
клонируйте репозиторий и выполните те же команды dotnet. Для полного скрипта Verify.ps1 нужен PowerShell 7.
Заявленный объём фактических проверок платформ указан в [VERIFICATION.md](VERIFICATION.md);
наличие переносимых исходников само по себе не подтверждает запуск на каждой ОС.

## Если не получилось

| Симптом | Что проверить |
| --- | --- |
| dotnet не найден | Установка SDK, новый терминал, PATH |
| SDK не подходит global.json | Установлен ли стабильный SDK 10, выполняется ли команда из корня |
| NETSDK1045 | Выбран старый SDK, проверьте dotnet --info |
| NuGet недоступен | Интернет/прокси, источник nuget.org, повторное dotnet restore |
| Запущен не тот урок | Полный путь --project и выбранный стартовый проект IDE |
| NotImplementedException в шаблоне | Это незаполненное место самостоятельной работы |
| FAIL в решении | Прочитайте ID и первую ошибку; не меняйте ожидаемое значение ради зелёного запуска |
| Русский текст искажён | UTF-8 файлов и современный терминал |

Официальная инструкция: [установка .NET в Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows).

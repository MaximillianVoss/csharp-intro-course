"""Render docs/project scaffolds. Teaching C# sources are never generated."""
from pathlib import Path
import argparse, json, html, re
ROOT = Path(__file__).resolve().parents[1]
FORMATS = {"implementation":"Реализация", "reading":"Чтение кода", "debugging":"Поиск ошибки",
"refactoring":"Рефакторинг", "testing":"Проектирование тестов", "mini-project":"Прикладная задача"}
parser=argparse.ArgumentParser()
parser.add_argument("--check",action="store_true")
parser.add_argument("--strict",action="store_true")
args=parser.parse_args()
differences=[]; projects=[]; learner_projects=[]; lessons=[]
def cell(value):
    return str(value).replace('|','\\|').replace('\n',' ')

def prose(value):
    # Markdown treats bare <int> and <T> in C# signatures as HTML tags.
    # Keep fenced/inline code literal, protect angle brackets in ordinary prose.
    parts=re.split(r"(```[\s\S]*?```|`[^`\n]*`)",str(value))
    return ''.join(part if i%2 else part.replace('<','&lt;').replace('>','&gt;')
                   for i,part in enumerate(parts))
def write(path,content):
    path=ROOT/path; content=content.rstrip()+"\n"
    if args.check:
        if not path.exists() or path.read_text(encoding="utf-8-sig")!=content:
            differences.append(str(path.relative_to(ROOT)))
    else:
        path.parent.mkdir(parents=True,exist_ok=True)
        path.write_text(content,encoding="utf-8",newline="\n")

for source in sorted((ROOT/"lessons").glob("lesson_*/tasks.json")):
    lesson=json.loads(source.read_text(encoding="utf-8-sig"))
    n=int(lesson["number"]); number=f"{n:02}"; folder=source.parent.relative_to(ROOT)
    lesson["_folder"]=folder.as_posix(); lessons.append(lesson); tasks=lesson["tasks"]
    assert len(tasks)==15,(source,"need 15 tasks")
    assert [t["id"] for t in tasks]==[f"{number}-{i:02}" for i in range(1,16)],source
    title=lesson["title"]
    conditions=[f"# Урок {n}. {title}: задачи","",
    "Сначала решите задачу самостоятельно. Ответы находятся в отдельном SOLUTIONS.md.",
    "Уровни: 1 — базовый, 2 — прикладной, 3 — повышенный.",
    "Код пишите в templates/Tasks.cs; каждый TaskNN.Run — независимое рабочее место.",
    "Методы из условия можно объявить внутри класса TaskNN.",
    "В задачах на чтение и тестирование запишите объяснение и воспроизводимый эксперимент.",""]
    answers=[f"# Урок {n}. {title}: решения","",
    "Сначала выполните свою попытку по TASKS.md. Здесь приведены ответы.",
    "Каждое решение запускается по ID; Run содержит исполняемые проверки.",""]
    table=["| ID | Уровень | Формат | Задача |","| --- | --- | --- | --- |"]
    for i,t in enumerate(tasks,1):
        for field in ("title","level","format","skill","algorithm","statement","example","solution","reasoning","pitfalls","checks"):
            assert field in t and t[field],(t["id"],field)
        assert t["format"] in FORMATS,t
        ident=t["id"]; table.append(f'| {ident} | {t["level"]} | {FORMATS[t["format"]]} | [{cell(t["title"])}](#{ident}) |')
        conditions += [f'<a id="{ident}"></a>',f'## {ident}. {t["title"]}',"",
        f'Уровень {t["level"]} · {FORMATS[t["format"]]} · Навык: {prose(t["skill"])}',"",
        prose(t["statement"]),"","**Пример и ожидаемое поведение**","",prose(t["example"]),""]
        src=f"solutions/Task{i:02}.cs"
        if (args.strict or args.check) and not (ROOT/folder/src).exists():
            raise FileNotFoundError(ROOT/folder/src)
        code=(ROOT/folder/src).read_text(encoding="utf-8-sig") if (ROOT/folder/src).exists() else "// Source is being authored."
        answers += [f'<a id="{ident}"></a>',f'## {ident}. {t["title"]}',"",prose(t["solution"]),"",
        "**Ход решения**","",prose(t["reasoning"]),"","**Частая ошибка**","",prose(t["pitfalls"]),"",
        f"Полный исходник: [{src}]({src}).","","```csharp",code.rstrip(),"```","",
        "**Исполняемые проверки**",""]+["- "+prose(s) for s in t["checks"]]+[""]
    conditions[8:8]=table+[""]
    write(folder/"TASKS.md","\n".join(conditions)); write(folder/"SOLUTIONS.md","\n".join(answers))
    prereqs=", ".join(str(p) for p in lesson["prerequisites"]) or "не требуются"
    outcomes="\n".join("- "+prose(x) for x in lesson["outcomes"])
    readme=f"""# Урок {n}. {title}

Предыдущие модули: {prereqs}. Рекомендуемый объём: 2–3 встречи по 90 минут и самостоятельная практика.

{outcomes}

## Маршрут

1. Прочитайте [теорию](THEORY.md); объясните примеры своими словами.
2. Запустите пример, измените один вход и предскажите результат.
3. Откройте [условия](TASKS.md), выберите задачи разных форматов.
4. Пишите в templates/Tasks.cs. Заготовка собирается, но незаполненная задача завершится ошибкой.
5. После своей попытки откройте [подробные решения](SOLUTIONS.md).
6. Проверьте граничный случай и объясните результат.

## Команды из корня курса

```powershell
dotnet run --project "{folder.as_posix()}/examples/Урок{number}.Примеры.csproj"
dotnet run --project "{folder.as_posix()}/templates/Урок{number}.Заготовки.csproj" -- --list
dotnet run --project "{folder.as_posix()}/templates/Урок{number}.Заготовки.csproj" -- {number}-01
dotnet run --project "{folder.as_posix()}/solutions/Урок{number}.Решения.csproj" -- {number}-01
dotnet run --project "{folder.as_posix()}/solutions/Урок{number}.Решения.csproj" -- --all
```

Третий запуск сообщает NotImplementedException до выполнения работы.
Встроенные проверки решений не исчерпывают все возможные входы.
В ранних модулях классы TaskNN и Run — предоставленный каркас; его устройство изучается в уроке 8.

## Каталог

"""+"\n".join(table)
    if n==15:
        readme += "\n\nВ заготовках этого урока Run возвращает Task. Для await добавьте модификатор async: " \
            "`public static async Task Run()`. Диспетчер ожидает возвращённую задачу; async void здесь не используется.\n"
    write(folder/"README.md",readme)
    for kind,suffix in (("examples","Примеры"),("solutions","Решения"),("templates","Заготовки")):
        project=folder/kind/f"Урок{number}.{suffix}.csproj"; projects.append(project.as_posix())
        if kind=="templates": learner_projects.append(project.as_posix())
        reference='  <ItemGroup><ProjectReference Include="../../../capstone/Библиотека.Ядро/Библиотека.Ядро.csproj" /></ItemGroup>\n' if n==16 else ''
        write(project,f"""<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <RootNamespace>Course.Lesson{number}</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <Using Include="Course" />
    <Compile Include="../../../shared/Check.cs" Link="Infrastructure/Check.cs" />
    <Compile Include="../../../shared/Runner.cs" Link="Infrastructure/Runner.cs" />
  </ItemGroup>
{reference}</Project>""")
        if kind!="examples":
            is_async=n==15
            array_type="Func<Task>" if is_async else "Action"
            calls=",\n".join(f'    ("{number}-{i:02}", Task{i:02}.Run)' for i in range(1,16))
            expr="await Runner.RunAsync" if is_async else "Runner.Run"
            write(folder/kind/"Program.cs",f"""// Generated dispatcher; TaskNN.cs files are authored separately.
using Course.Lesson{number};
return {expr}(args, new (string Id, {array_type} Body)[]
{{
{calls}
}}, template: {str(kind=="templates").lower()});""")
    template=folder/"templates"/"Tasks.cs"
    # Never overwrite learner work; no reset option.
    if not (ROOT/template).exists() and not args.check:
        text=f"namespace Course.Lesson{number};\n\n"
        for i,t in enumerate(tasks,1):
            text+=f"""// {t["id"]}. {t["title"]}
// Условие: ../TASKS.md, раздел {t["id"]}.
public static class Task{i:02}
{{
    public static {"Task" if n==15 else "void"} Run()
    {{
        // TODO: выполните условие и добавьте проверки. Ответов здесь нет.
        throw new NotImplementedException("Задача {t["id"]} ещё не решена.");
    }}
}}

"""
        write(template,text)
if args.strict or args.check:
    assert [x["number"] for x in lessons]==list(range(1,17)),"need modules 01–16"
for p in sorted((ROOT/"capstone").glob("*/*.csproj")):
    projects.append(p.relative_to(ROOT).as_posix())
if (ROOT/"tools/SyntaxAudit/SyntaxAudit.csproj").exists(): projects.append("tools/SyntaxAudit/SyntaxAudit.csproj")
def slnx(paths):
    return '<Solution>\n'+"".join('  <Project Path="'+html.escape(p,quote=True)+'" />\n' for p in paths)+'</Solution>'
write("Курс CSharp.slnx",slnx(projects)); write("Практика CSharp.slnx",slnx(learner_projects))
rows=["| № | Модуль | Теория | Задачи | Ответы |","| --- | --- | --- | --- | --- |"]
for l in lessons:
    p=l["_folder"]
    rows.append(f'| {l["number"]} | [{cell(l["title"])}](../{p}/README.md) | [читать](../{p}/THEORY.md) | [15 задач](../{p}/TASKS.md) | [решения](../{p}/SOLUTIONS.md) |')
write("docs/THEORY_INDEX.md","# Оглавление курса\n\n"+"\n".join(rows))
catalog=["# Единый задачник\n","240 задач: 16 модулей × 15. Условия не содержат ответов.\n"]
for l in lessons:
    catalog += [f'## {l["number"]}. {l["title"]}',""]
    catalog += [f'- [{t["id"]}. {t["title"]}](../{l["_folder"]}/TASKS.md#{t["id"]}) — уровень {t["level"]}; {FORMATS[t["format"]]}' for t in l["tasks"]]+[""]
write("docs/TASK_INDEX.md","\n".join(catalog))
write("docs/COURSE_MAP.md","""# Карта курса

Начальная точка — умение работать с файлами и клавиатурой. Опыт программирования не требуется.
Маршрут: 1–5 синтаксис и декомпозиция → 6–9 данные и модель → 10–11 хранение и ошибки →
12–15 выразительный и асинхронный C# → 16 проверка и приложение.

Ориентир: 32–48 встреч по 90 минут плюс 50–80 часов самостоятельной работы.
15 задач модуля образуют банк выбора. На модуль рекомендуется решить 3 базовые, 3 прикладные
и 2 повышенные задачи, затем перенести навык на новый вход. Полный банк служит повторению и пересдачам.

"""+"\n".join(rows)+"""

## Контрольные точки

- После 4: проверить ввод, выделить чистый метод, проследить цикл в отладчике.
- После 7: выбрать массив, список, словарь или множество по операциям.
- После 11: загрузить JSON, проверить ограничения, сохранить исходные данные при ошибке.
- После 15: объяснить порядок await, распространение исключения и отмену.
- После 16: собрать решение с нуля, пройти тесты и показать CLI с перезапуском.

Методика — [TEACHING_GUIDE.md](TEACHING_GUIDE.md); критерии — [ASSESSMENT.md](ASSESSMENT.md).
""")
if differences: raise SystemExit("Generated files differ:\n"+"\n".join(differences))
print(f"Rendered/checked {len(lessons)} modules, {sum(len(x['tasks']) for x in lessons)} tasks, {len(projects)} projects.")

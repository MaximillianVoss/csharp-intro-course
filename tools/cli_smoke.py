"""Black-box CLI checks in separate processes, isolated temporary directory."""
from pathlib import Path
import json, os, subprocess, tempfile
ROOT=Path(__file__).resolve().parents[1]
DLL=ROOT/"capstone/Библиотека.Консоль/bin/Release/net10.0/Библиотека.Консоль.dll"
assert DLL.exists(),"build Release first"
scenarios=[]
with tempfile.TemporaryDirectory(prefix="csharp-course-cli-") as directory:
    data=Path(directory)/"books.json"
    def run(arguments,code=0,contains=None):
        result=subprocess.run(["dotnet",str(DLL),"--data",str(data),*arguments],capture_output=True,text=True,encoding="utf-8",timeout=20)
        assert result.returncode==code,(arguments,result.returncode,result.stdout,result.stderr)
        if contains is not None:assert contains in result.stdout+result.stderr,(arguments,result.stdout,result.stderr,contains)
        scenarios.append({"arguments":arguments,"exit_code":result.returncode,"passed":True})
        return result
    run(["list"],contains="Книг нет.")
    assert not data.exists()
    run(["add","Азбука 🌲","Автор","2000"],contains="#1")
    run(["add","Вторая книга","Другой автор","2020"],contains="#2")
    run(["search","АЗБУКА"],contains="Азбука 🌲")
    run(["read","1"],contains="прочитана")
    unread=run(["list","--unread"],contains="Вторая книга")
    assert "Азбука" not in unread.stdout
    run(["stats"],contains="Всего: 2; прочитано: 1; осталось: 1.")
    before=data.read_bytes()
    run(["add","","Автор","2000"],2);assert data.read_bytes()==before
    run(["remove","99"],3);assert data.read_bytes()==before
    run(["nonsense"],2);assert data.read_bytes()==before
    run(["remove","2"],contains="удалена")
    run(["add","Третья","Автор","2001"],contains="#3")
    document=json.loads(data.read_text(encoding="utf-8"));assert document["nextId"]==4
    assert [b["id"] for b in document["books"]]==[1,3]
    run(["list"],contains="[3] Третья")
    data.write_text("{broken",encoding="utf-8");before=data.read_bytes()
    run(["stats"],1,contains="Ошибка");assert data.read_bytes()==before
    run(["add","Не сохранять","Автор","2000"],1);assert data.read_bytes()==before
output={"scenarios":len(scenarios),"passed":True,"details":scenarios}
(ROOT/"artifacts").mkdir(exist_ok=True)
(ROOT/"artifacts/cli-smoke.json").write_text(json.dumps(output,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print(f"CLI black-box: {len(scenarios)}/{len(scenarios)} scenarios passed.")

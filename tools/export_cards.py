"""Export separate learner/teacher cards from reviewed canonical course sources."""
from pathlib import Path
import argparse
import hashlib
import json
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
sys.stdout.reconfigure(encoding="utf-8")


def digest(value):
    return hashlib.sha256(json.dumps(value, ensure_ascii=False, sort_keys=True).encode("utf-8")).hexdigest()


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output", type=Path, default=ROOT / "artifacts/cards")
    parser.add_argument("--source-version", default="working-tree")
    parser.add_argument("--verify-starters", action="store_true")
    args = parser.parse_args()
    result = subprocess.run(["dotnet", "run", "--project", str(ROOT / "tools/SyntaxAudit/SyntaxAudit.csproj"),
                             "-c", "Release", "--no-build", "--", str(ROOT)] + (["--verify-starters"] if args.verify_starters else []),
                            capture_output=True, text=True, encoding="utf-8", check=True)
    syntax = {row["id"]: row for row in json.loads(result.stdout)}
    learner, teacher = [], []
    for path in sorted((ROOT / "lessons").glob("lesson_*/tasks.json")):
        lesson = json.loads(path.read_text(encoding="utf-8-sig"))
        reviews = {row["id"]: row for row in json.loads((path.parent / "TASK_REVIEW.json").read_text(encoding="utf-8-sig"))}
        for task in lesson["tasks"]:
            ident = task["id"]
            source = syntax[ident]
            content_hash = digest({"task": task, "solution": source["sourceSha256"], "starter": source["starterCode"]})
            if reviews[ident].get("reviewed_sha256") != content_hash:
                raise ValueError(f"Review is stale or missing: {ident}")
            common = {"schema_version": 1, "course_id": "csharp-intro-course", "id": ident,
                      "stable_id": f"csharp-intro-course:{ident}", "lesson": lesson["number"],
                      "title": task["title"], "level": task["level"], "format": task["format"],
                      "skill": task["skill"], "content_sha256": content_hash}
            environment = {"language": "C#", "language_version": "14", "target_framework": "net10.0",
                           "implicit_usings": True, "nullable": True,
                           "entry_type": f"Course.Lesson{lesson['number']:02}.Task{ident[-2:]}",
                           "entry_method": "Run", "entry_return": "Task" if lesson["number"] == 15 else "void",
                           "support_sources": ["shared/Check.cs"],
                           "project_references": ["capstone/Библиотека.Ядро/Библиотека.Ядро.csproj"] if lesson["number"] == 16 else [],
                           "automatic_grading_adapter": "not_implemented"}
            learner.append({**common, "statement_markdown": task["statement"], "example_markdown": task["example"],
                            "source_directory": path.parent.relative_to(ROOT).as_posix(),
                            "starter_code": source["starterCode"], "environment": environment})
            teacher.append({**common, "solution": task["solution"], "reasoning": task["reasoning"],
                            "algorithm": task["algorithm"], "pitfalls": task["pitfalls"], "checks": task["checks"],
                            "reference_code": (ROOT / source["file"]).read_text(encoding="utf-8-sig"),
                            "reference_assertion_calls": source["checks"], "review": reviews[ident]})
    expected = [f"{lesson:02}-{task:02}" for lesson in range(1, 17) for task in range(1, 16)]
    if [row["id"] for row in learner] != expected or len(teacher) != 240:
        raise ValueError("The export must contain exactly the stable 240 task IDs")
    forbidden = {"solution", "reasoning", "algorithm", "pitfalls", "checks", "reference_code", "review"}
    if any(forbidden.intersection(row) for row in learner):
        raise ValueError("Teacher fields in learner export")
    verification = "not_run"
    if args.verify_starters:
        failures = {row["id"]: row.get("starterCompilationErrors", ["verification absent"])
                    for row in syntax.values() if not row.get("starterCompilationVerified")}
        if failures:
            raise RuntimeError("Isolated starter compilation failed: " + json.dumps(failures, ensure_ascii=False))
        verification = "240_excerpts_compiled_individually_warnings_as_errors"
    args.output.mkdir(parents=True, exist_ok=True)
    files = {}
    for name, rows in (("learner.cards.jsonl", learner), ("teacher.cards.jsonl", teacher)):
        data = "".join(json.dumps(row, ensure_ascii=False) + "\n" for row in rows).encode("utf-8")
        (args.output / name).write_bytes(data)
        files[name] = {"rows": len(rows), "bytes": len(data), "sha256": hashlib.sha256(data).hexdigest()}
    manifest = {"schema_version": 1, "course_id": "csharp-intro-course", "source_version": args.source_version,
                "task_count": 240, "source_content_sha256": digest([row["content_sha256"] for row in learner]),
                "files": files, "starter_verification": verification,
                "limits": ["Teacher data must not be included in the learner UI payload.",
                           "Reference Run checks test the reference implementation, not arbitrary learner submissions.",
                           "C# execution, sandboxing and grading integration in the tutor are not verified here.",
                           "Some statements use relative links; resolve them against source_directory in this course."]}
    (args.output / "manifest.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps({"tasks": 240, "output": str(args.output), "starter_verification": verification,
                      "source_content_sha256": manifest["source_content_sha256"]}, ensure_ascii=False))


if __name__ == "__main__":
    main()

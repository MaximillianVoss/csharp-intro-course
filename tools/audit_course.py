"""Contract/link checks and semantic-review candidates, using Roslyn syntax fingerprints."""
from pathlib import Path
from collections import Counter, defaultdict
from itertools import combinations
import argparse, hashlib, json, re, subprocess, sys
from urllib.parse import unquote

ROOT=Path(__file__).resolve().parents[1]
sys.stdout.reconfigure(encoding="utf-8")
p=argparse.ArgumentParser(); p.add_argument("--discover",action="store_true");p.add_argument("--partial",action="store_true")
args=p.parse_args()
ART=ROOT/"artifacts";ART.mkdir(exist_ok=True)
def digest(value):
    return hashlib.sha256(json.dumps(value,ensure_ascii=False,sort_keys=True).encode("utf-8")).hexdigest()
def normal(text):
    text=re.sub(r'"[^"\n]*"|«[^»]*»',"<text>",text.lower())
    text=re.sub(r"\b\d+(?:[.,]\d+)*\b","#",text)
    return " ".join(re.findall(r"[a-zа-яё_]+|#|<text>",text))
def grams(items,n=4):
    return {tuple(items[i:i+n]) for i in range(max(0,len(items)-n+1))}
def similarity(a,b):
    return len(a&b)/len(a|b) if a|b else 1.0
errors=[]; tasks=[]; lesson_stats=[]; task_reviews={}
for manifest in sorted((ROOT/"lessons").glob("lesson_*/tasks.json")):
    lesson=json.loads(manifest.read_text(encoding="utf-8-sig")); number=lesson["number"]; folder=manifest.parent
    these=lesson["tasks"];levels=Counter(t["level"] for t in these);formats=Counter(t["format"] for t in these)
    review_path=folder/"TASK_REVIEW.json"
    reviews=json.loads(review_path.read_text(encoding="utf-8-sig")) if review_path.exists() else []
    if [r.get("id") for r in reviews]!=[t["id"] for t in these]:errors.append(f"missing/incorrect per-task review: lesson {number}")
    for review in reviews:
        for field in ("condition","skill","distinctness","solution","starter"):
            if not isinstance(review.get(field),str) or not review[field].strip():errors.append(f"incomplete review {review.get('id')}: {field}")
        tests=review.get("tests")
        if not ((isinstance(tests,str) and tests.strip()) or
                (isinstance(tests,list) and tests and all(isinstance(item,str) and item.strip() for item in tests))):
            errors.append(f"incomplete review {review.get('id')}: tests")
        if not review.get("changes"):errors.append(f"missing review changes {review.get('id')}")
        task_reviews[review.get("id")]=review
    if levels!={1:5,2:5,3:5}:errors.append(f"level balance: {number}: {dict(levels)}")
    for kind in ("implementation","reading","debugging","refactoring","testing","mini-project"):
        if not formats[kind]:errors.append(f"missing format {kind}: lesson {number}")
    theory=(folder/"THEORY.md").read_text(encoding="utf-8-sig") if (folder/"THEORY.md").exists() else ""
    words=len(re.findall(r"\b[а-яёa-z]{2,}\b",theory.lower()))
    if words<700:errors.append(f"theory too short/missing: {number}, {words} words")
    lesson_stats.append({"lesson":number,"tasks":len(these),"levels":dict(levels),"formats":dict(formats),"theory_words":words})
    for i,t in enumerate(these,1):
        t=dict(t);t["_folder"]=folder.relative_to(ROOT).as_posix();tasks.append(t)
        if t["id"]!=f"{number:02}-{i:02}":errors.append("bad ID "+t["id"])
        if len(t["statement"])<100 or len(t["reasoning"])<100:errors.append("incomplete contract/explanation "+t["id"])
        if t["statement"].count("```") % 2:errors.append("unbalanced condition code fence "+t["id"])
        for suffix in ("TASKS.md","SOLUTIONS.md"):
            if not (folder/suffix).exists() or f'## {t["id"]}. {t["title"]}' not in (folder/suffix).read_text(encoding="utf-8-sig"):
                errors.append(f"catalog mismatch {t['id']}: {suffix}")
if not args.partial and (len(lesson_stats)!=16 or len(tasks)!=240):errors.append("expected 16 lessons / 240 tasks")
for field in ("id","title"):
    for value,count in Counter(t[field].casefold() if field=="id" else normal(t[field]) for t in tasks).items():
        if count>1:errors.append(f"duplicate {field}: {value}")
by_condition=defaultdict(list)
for t in tasks:by_condition[normal(t["statement"])].append(t["id"])
for value,ids in by_condition.items():
    if len(ids)>1:errors.append("duplicate normalized conditions: "+",".join(ids))

command=["dotnet","run","--project",str(ROOT/"tools/SyntaxAudit/SyntaxAudit.csproj"),"-c","Release","--no-build","--",str(ROOT)]
result=subprocess.run(command,capture_output=True,text=True,encoding="utf-8")
if result.returncode:raise SystemExit(result.stdout+result.stderr)
syntax=json.loads(result.stdout); by_id={x["id"]:x for x in syntax}; metadata={t["id"]:t for t in tasks}
for t in tasks:
    s=by_id.get(t["id"])
    if s is None:errors.append("missing syntax "+t["id"]);continue
    if s["checks"]<1:errors.append("no executable assertions "+t["id"])
    if "Run" not in s["methods"]:errors.append("missing Run "+t["id"])
    if s["incomplete"]:errors.append("NotImplementedException in solution "+t["id"])
    if not s["starterIncomplete"]:errors.append("starter is not left unsolved "+t["id"])
    if s["starterSubjectMembers"]<1 and t["format"] not in {"reading","testing"}:errors.append("starter has no subject API "+t["id"])
    reviewed_content=digest({"task":{k:v for k,v in t.items() if not k.startswith('_')},"solution":s["sourceSha256"],"starter":s["starterCode"]})
    if task_reviews.get(t["id"],{}).get("reviewed_sha256")!=reviewed_content:errors.append("task review not bound to current content "+t["id"])
    if t["format"] in {"reading","refactoring"} and s["statementSyntaxErrors"]:
        errors.append("C# snippet syntax in condition "+t["id"]+": "+"; ".join(s["statementSyntaxErrors"]))
candidates=[]
prepared={}
for t in tasks:
    s=by_id[t["id"]]
    prepared[t["id"]]=(grams(normal(t["statement"]).split(),2),grams(s["bodyTokens"],4),grams(normal(t["algorithm"]).split(),1))
for a,b in combinations(tasks,2):
    ia,ib=a["id"],b["id"];sa,sb=by_id[ia],by_id[ib];fa,fb=prepared[ia],prepared[ib]
    text=similarity(fa[0],fb[0]);shape=similarity(fa[1],fb[1]);algorithm=similarity(fa[2],fb[2])
    exact=sa["bodyTokens"]==sb["bodyTokens"] and len(sa["bodyTokens"])>=20
    length_ratio=min(len(sa["bodyTokens"]),len(sb["bodyTokens"]))/max(1,max(len(sa["bodyTokens"]),len(sb["bodyTokens"])))
    signals=[]
    if exact:signals.append("same-normalized-csharp-structure")
    if text>=0.60:signals.append("similar-normalized-condition")
    if shape>=0.82 and length_ratio>=0.75 and len(sa["bodyTokens"])>=40:signals.append("similar-algorithm-structure")
    if algorithm>=0.66 and len(fa[2])>=4 and len(fb[2])>=4:signals.append("similar-algorithm-description")
    if not signals:continue
    pair=ia+"~"+ib
    fingerprint=digest({"a":a,"b":b,"sourceA":sa["sourceSha256"],"sourceB":sb["sourceSha256"]})
    candidates.append({"pair":pair,"fingerprint":fingerprint,"signals":signals,
      "condition_similarity":round(text,3),"syntax_similarity":round(shape,3),"algorithm_similarity":round(algorithm,3),
      "titles":[a["title"],b["title"]],"skills":[a["skill"],b["skill"]],"algorithms":[a["algorithm"],b["algorithm"]]})
reviews_path=ROOT/"docs/SEMANTIC_REVIEW.json"
reviews=json.loads(reviews_path.read_text(encoding="utf-8-sig")) if reviews_path.exists() else {}
unreviewed=[]
for c in candidates:
    review=reviews.get(c["pair"])
    if not review or review.get("fingerprint")!=c["fingerprint"] or review.get("decision")!="distinct" or len(review.get("reason",""))<70:
        unreviewed.append(c["pair"])
    else:c["review"]=review

# Check local Markdown links outside fenced code blocks.
link_count=0
for path in ROOT.rglob("*.md"):
    if any(part in {"bin","obj",".git","artifacts"} for part in path.relative_to(ROOT).parts):continue
    text=re.sub(r"```.*?```","",path.read_text(encoding="utf-8-sig"),flags=re.S)
    for match in re.finditer(r"\[[^\]]+\]\(([^)]+)\)",text):
        target=match.group(1).strip().strip("<>")
        if re.match(r"^[a-z]+:",target,re.I):continue
        relative=unquote(target.split("#",1)[0])
        if not relative:continue
        link_count+=1
        destination=(path.parent/relative).resolve()
        if not destination.exists():errors.append(f"broken link {path.relative_to(ROOT)} -> {target}")
report={"lessons":lesson_stats,"tasks":len(tasks),"formats":dict(Counter(t["format"] for t in tasks)),
"task_reviews":len(task_reviews),
"levels":dict(Counter(t["level"] for t in tasks)),"links_checked":link_count,"errors":errors,
"syntax_records":syntax,"candidates":candidates,"unreviewed":unreviewed,
"review_complete":not errors and not unreviewed,"method":"normalized conditions + Roslyn token/node structure + algorithm descriptions; manual decisions required"}
(ART/"audit.json").write_text(json.dumps(report,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
summary={k:report[k] for k in ("tasks","formats","levels","links_checked","errors","unreviewed","review_complete")}
print(json.dumps(summary,ensure_ascii=False,indent=2))
if errors or (unreviewed and not args.discover):sys.exit(1)

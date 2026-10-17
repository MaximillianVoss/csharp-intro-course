using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// A syntax fingerprint is a review aid, never proof of semantic equivalence.
string root = args.Length == 0 ? Directory.GetCurrentDirectory() : Path.GetFullPath(args[0]);
var records = new List<object>();
foreach (string lesson in Directory.GetDirectories(Path.Combine(root, "lessons"), "lesson_*").Order())
{
    string manifest = Path.Combine(lesson,"tasks.json");
    if (!File.Exists(manifest)) continue;
    using var doc = JsonDocument.Parse(File.ReadAllText(manifest));
    int number = doc.RootElement.GetProperty("number").GetInt32();
    int index=0;
    foreach(var task in doc.RootElement.GetProperty("tasks").EnumerateArray())
    {
        index++;
        string path=Path.Combine(lesson,"solutions",$"Task{index:00}.cs");
        if(!File.Exists(path))throw new FileNotFoundException("Missing task source.",path);
        string code=File.ReadAllText(path);
        var tree=CSharpSyntaxTree.ParseText(code,new CSharpParseOptions(LanguageVersion.CSharp14));
        var errors=tree.GetDiagnostics().Where(d=>d.Severity==DiagnosticSeverity.Error).ToArray();
        if(errors.Length>0)throw new InvalidDataException(string.Join("\n",errors.Select(x=>x.ToString())));
        var syntax=tree.GetRoot();
        var statementSyntaxErrors = new List<string>();
        foreach (Match snippet in Regex.Matches(task.GetProperty("statement").GetString() ?? "", "```(?:csharp|cs)\\s*\\n([\\s\\S]*?)```"))
        {
            var snippetTree = CSharpSyntaxTree.ParseText(snippet.Groups[1].Value,
                new CSharpParseOptions(LanguageVersion.CSharp14, kind: SourceCodeKind.Script));
            statementSyntaxErrors.AddRange(snippetTree.GetDiagnostics().Where(d=>d.Severity==DiagnosticSeverity.Error).Select(d=>d.ToString()));
        }
        var methods=syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();
        var business=methods.Where(m=>m.Identifier.ValueText!="Run").ToArray();
        // If a task is itself a test-design experiment, its Run is the subject.
        var selected=business.Length>0?business:methods;
        var tokens=selected.SelectMany(m=>m.DescendantTokens()).Select(t=>
            t.IsKind(SyntaxKind.IdentifierToken)?"ID":
            t.IsKind(SyntaxKind.NumericLiteralToken)||t.IsKind(SyntaxKind.StringLiteralToken)||t.IsKind(SyntaxKind.CharacterLiteralToken)?"LITERAL":
            t.Kind().ToString()).ToArray();
        var apiTokens=selected.SelectMany(m=>m.DescendantTokens()).Select(t=>
            t.IsKind(SyntaxKind.NumericLiteralToken)||t.IsKind(SyntaxKind.StringLiteralToken)||t.IsKind(SyntaxKind.CharacterLiteralToken)?"LITERAL":
            t.IsKind(SyntaxKind.IdentifierToken)?t.ValueText:t.Kind().ToString()).ToArray();
        var bodyTokens=selected.Select(Body).SelectMany(m=>m.DescendantTokens()).Select(t=>
            t.IsKind(SyntaxKind.IdentifierToken)?"ID":
            t.IsKind(SyntaxKind.NumericLiteralToken)||t.IsKind(SyntaxKind.StringLiteralToken)||t.IsKind(SyntaxKind.CharacterLiteralToken)?"LITERAL":
            t.Kind().ToString()).ToArray();
        var shape=selected.SelectMany(m=>m.DescendantNodesAndSelf()).Select(n=>n.Kind().ToString()).ToArray();
        int checks=syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Count(i=>i.Expression.ToString().StartsWith("Check.",StringComparison.Ordinal));
        records.Add(new{
            id=task.GetProperty("id").GetString(), lesson=number,
            file=Path.GetRelativePath(root,path).Replace('\\','/'),
            methods=methods.Select(m=>m.Identifier.ValueText).ToArray(),
            tokens, bodyTokens, apiTokens, shape, checks, statementSyntaxErrors,
            sourceSha256=Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(code))).ToLowerInvariant(),
            incomplete=code.Contains("NotImplementedException",StringComparison.Ordinal)
        });
    }
}
Console.WriteLine(JsonSerializer.Serialize(records,new JsonSerializerOptions{WriteIndented=true}));

static SyntaxNode Body(MethodDeclarationSyntax method)
{
    if (method.Body is not null) return method.Body;
    if (method.ExpressionBody is not null) return method.ExpressionBody;
    return method;
}

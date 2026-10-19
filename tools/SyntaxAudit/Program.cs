using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// A syntax fingerprint is a review aid, never proof of semantic equivalence.
string root = args.Length == 0 ? Directory.GetCurrentDirectory() : Path.GetFullPath(args[0]);
bool verifyStarters = args.Skip(1).Contains("--verify-starters", StringComparer.Ordinal);
// Construct references and compile only when explicitly requested. Ordinary fingerprint
// extraction must not silently claim that the exported cards have been compiled.
var starterVerifier = verifyStarters ? new StarterVerifier(root) : null;
var records = new List<object>();
foreach (string lesson in Directory.GetDirectories(Path.Combine(root, "lessons"), "lesson_*").Order())
{
    string manifest = Path.Combine(lesson,"tasks.json");
    if (!File.Exists(manifest)) continue;
    using var doc = JsonDocument.Parse(File.ReadAllText(manifest));
    int number = doc.RootElement.GetProperty("number").GetInt32();
    string starterPath=Path.Combine(lesson,"templates","Tasks.cs");
    var starterRoot=CSharpSyntaxTree.ParseText(File.ReadAllText(starterPath),new CSharpParseOptions(LanguageVersion.CSharp14)).GetCompilationUnitRoot();
    var starterNamespace=starterRoot.DescendantNodes().OfType<BaseNamespaceDeclarationSyntax>().First();
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
        var starter=starterNamespace.Members.OfType<ClassDeclarationSyntax>().Single(c=>c.Identifier.ValueText==$"Task{index:00}");
        string starterCode=string.Concat(starterRoot.Usings.Select(u=>u.ToFullString()))
            +$"\nnamespace {starterNamespace.Name};\n"+starter.ToFullString().Trim()+"\n";
        int starterSubjectMembers=starter.Members.Count(m=>m is not MethodDeclarationSyntax method || method.Identifier.ValueText!="Run");
        string[] starterCompilationErrors = starterVerifier?.Verify(starterCode, task.GetProperty("id").GetString()!, number) ?? [];
        bool starterCompilationVerified = verifyStarters && starterCompilationErrors.Length == 0;
        records.Add(new{
            id=task.GetProperty("id").GetString(), lesson=number,
            file=Path.GetRelativePath(root,path).Replace('\\','/'),
            methods=methods.Select(m=>m.Identifier.ValueText).ToArray(),
            tokens, bodyTokens, apiTokens, shape, checks, statementSyntaxErrors,
            starterCode, starterSubjectMembers,
            starterCompilationErrors, starterCompilationVerified,
            starterIncomplete=starter.ToString().Contains("NotImplementedException",StringComparison.Ordinal),
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

sealed class StarterVerifier
{
    private static readonly CSharpParseOptions ParseOptions = new(LanguageVersion.CSharp14);
    private readonly MetadataReference[] platformReferences;
    private readonly SyntaxTree[] sharedTrees;
    private readonly string corePath;

    public StarterVerifier(string root)
    {
        var trusted = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string
            ?? throw new InvalidOperationException("Runtime did not expose TRUSTED_PLATFORM_ASSEMBLIES.");
        string runtimeDirectory = Path.GetDirectoryName(Path.GetFullPath(typeof(object).Assembly.Location))
            ?? throw new InvalidOperationException("Cannot locate the .NET runtime directory.");
        StringComparison pathComparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        // TPA also contains the host application's dependencies. Those are not part
        // of a learner card's environment: exclude SyntaxAudit and Roslyn itself.
        platformReferences = trusted.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(path => string.Equals(Path.GetDirectoryName(Path.GetFullPath(path)), runtimeDirectory, pathComparison))
            .Distinct(StringComparer.Ordinal)
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path)).ToArray();
        // Match the SDK console project's implicit usings and its explicit Course using.
        // No lesson assembly, dispatcher, neighbouring card or reference solution is loaded.
        const string implicitUsings = """
            global using System;
            global using System.Collections.Generic;
            global using System.IO;
            global using System.Linq;
            global using System.Net.Http;
            global using System.Threading;
            global using System.Threading.Tasks;
            global using Course;
            """;
        sharedTrees = [
            CSharpSyntaxTree.ParseText(implicitUsings, ParseOptions, "ImplicitUsings.g.cs"),
            CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(root, "shared", "Check.cs")), ParseOptions, "shared/Check.cs")
        ];
        corePath = Path.Combine(root, "capstone", "Библиотека.Ядро", "bin", "Release", "net10.0", "Библиотека.Ядро.dll");
    }

    public string[] Verify(string starterCode, string taskId, int lesson)
    {
        IEnumerable<MetadataReference> references = platformReferences;
        if (lesson == 16)
        {
            if (!File.Exists(corePath))
                return [$"Missing lesson 16 core reference: {corePath}. Build the capstone core in Release first."];
            references = references.Append(MetadataReference.CreateFromFile(corePath));
        }
        var card = CSharpSyntaxTree.ParseText(starterCode, ParseOptions, $"{taskId}.starter.cs");
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
            optimizationLevel: OptimizationLevel.Release,
            warningLevel: 4,
            generalDiagnosticOption: ReportDiagnostic.Error,
            nullableContextOptions: NullableContextOptions.Enable,
            deterministic: true);
        var compilation = CSharpCompilation.Create($"Starter_{taskId.Replace('-', '_')}",
            sharedTrees.Append(card), references, options);
        using var assembly = new MemoryStream();
        var emitted = compilation.Emit(assembly);
        var diagnostics = emitted.Diagnostics
            .Where(d => d.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)
            .Select(d => d.ToString()).Distinct(StringComparer.Ordinal).ToArray();
        return !emitted.Success && diagnostics.Length == 0
            ? ["Independent starter compilation failed without a reported diagnostic."]
            : diagnostics;
    }
}

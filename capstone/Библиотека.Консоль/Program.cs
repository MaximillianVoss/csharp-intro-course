using System.Text;
using Course.Library;

Console.OutputEncoding = Encoding.UTF8;
using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cancellation.Cancel(); };
return await new CliApplication(path => new JsonLibraryStore(path))
    .RunAsync(args, Console.Out, Console.Error, cancellation.Token);

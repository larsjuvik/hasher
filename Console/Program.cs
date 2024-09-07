using CommandLine;
using CommandLine.Text;
using Services;
using Services.Models;

await Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithNotParsed(errors =>
    {
        Environment.Exit(1);
    })
    .WithParsedAsync(RunAsync);
return;

static async Task RunAsync(CommandLineOptions options)
{
    var cancellationTokenSource = new CancellationTokenSource();
    var cancellationToken = cancellationTokenSource.Token;
    
    var progress = new Progress<HashingProgress>(p =>
    {
        // TODO: log progress
    });

    // Creating file stream
    if (!File.Exists(options.InputFile))
    {
        return;
    }
    var fileStream = File.OpenRead(options.InputFile);

    // Convert chosen algorithm to enum
    try
    {
        var selectedHashAlgorithm = HashService.GetAlgorithmFromString(options.Algorithm);
        var hash = await HashService.Hash(selectedHashAlgorithm, fileStream, progress, cancellationToken);
        Console.WriteLine($"{selectedHashAlgorithm.ToString().ToUpper()}: {hash}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"[ERROR] Algorithm {options.Algorithm} not recognized.");
    }
}

internal class CommandLineOptions
{
    [Option('f', "file", Required = true, HelpText = "Path to the file to hash")]
    public required string InputFile { get; set; }

    [Option('a', "algorithm", Default = "sha256", Required = false, HelpText = "Hash algorithm to use")]
    public required string Algorithm { get; set; }

    [Usage(ApplicationAlias = "hasher")]
    public static IEnumerable<Example> Examples =>
        new List<Example>
        {
            new("Hash file with SHA256", new CommandLineOptions { Algorithm = "sha256", InputFile = "input.txt" })
        };
}
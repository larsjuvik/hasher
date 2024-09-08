using CommandLine;
using CommandLine.Text;
using Services;
using Services.Models;
using ShellProgressBar;

var result = Parser.Default.ParseArguments<CommandLineOptions>(args);
await result.WithNotParsed(errors =>
{
    if (result.Value.ListAlgorithms)
    {
        Console.WriteLine("Listing algorithms");
        Environment.Exit(0);
    }

    Environment.Exit(1);
})
.WithParsedAsync(RunAsync);
return;

static void RedPrint(string text)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(text);
    Console.ResetColor();
}

static async Task RunAsync(CommandLineOptions options)
{
    // Get the algorithm chosen
    HashService.Algorithm selectedHashAlgorithm;
    try
    {
        selectedHashAlgorithm = HashService.GetAlgorithmFromString(options.Algorithm);
    }
    catch (Exception)
    {
        RedPrint($"Did not recognize algorithm: {options.Algorithm}");
        Environment.Exit(1);
        return;  // Fixes var may not be initialized warning
    }
    
    // Checking if file exists
    if (!File.Exists(options.InputFile))
    {
        RedPrint($"Input file does not exist: {options.InputFile}");
        Environment.Exit(1);
        return;  // Fixes var may not be initialized warning
    }
    
    // Get filestream
    var fileStream = File.OpenRead(options.InputFile);

    var cancellationTokenSource = new CancellationTokenSource();
    var cancellationToken = cancellationTokenSource.Token;
    
    var progressBar = new ProgressBar(1000, "Hashing", new ProgressBarOptions
    {
        CollapseWhenFinished = true,
        ShowEstimatedDuration = true
    });
    var progress = progressBar.AsProgress<HashingProgress>(_ => string.Empty, hashingProgress => hashingProgress.PercentageComplete);

    // Hash the file
    try
    {
        var hash = await HashService.Hash(selectedHashAlgorithm, fileStream, progress, cancellationToken);
        progressBar.Dispose();
        
        Console.WriteLine($"{selectedHashAlgorithm.ToString().ToUpper()}: {hash}");
        if (!string.IsNullOrEmpty(options.Verify))
        {
            var hashesMatch = options.Verify == hash;
            var hashesMatchMessage = hashesMatch ? "MATCH OK" : "MATCH FAILED";
            var color = hashesMatch ? ConsoleColor.Green : ConsoleColor.Red;
            Console.ForegroundColor = color;
            Console.WriteLine($"Verify: {hashesMatchMessage}", color);
            Console.ResetColor();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"An error occured while hashing: {e.Message}");
    }
}

internal class CommandLineOptions
{
    [Option('f', "file", Required = true, HelpText = "Path to the file to hash")]
    public required string InputFile { get; init; }

    [Option('a', "algorithm", Default = "sha256", Required = true, HelpText = "Hash algorithm to use")]
    public required string Algorithm { get; init; }
    
    [Option('v', "verify", Default = null, Required = false, HelpText = "Verify a string towards hash")]
    public string? Verify { get; init; }
    
    [Option('l', "list", Default = false, Required = false, HelpText = "List the available algorithms")]
    public bool ListAlgorithms { get; init; }

    [Usage(ApplicationAlias = "hasher")]
    public static IEnumerable<Example> Examples =>
        new List<Example>
        {
            new("Hash file with SHA256", new CommandLineOptions { Algorithm = "sha256", InputFile = "input.txt" })
        };
}
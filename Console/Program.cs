using CommandLine;
using CommandLine.Text;
using Services;
using Services.Models;
using ShellProgressBar;

const int exitSuccess = 0;
const int exitError = 1;

var result = Parser.Default.ParseArguments<CommandLineOptions>(args);
await result.WithNotParsed(_ =>
{
    Environment.Exit(exitError);
})
.WithParsedAsync(RunAsync);

Environment.Exit(exitSuccess);
return;

static void RedPrint(string text)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(text);
    Console.ResetColor();
}

static async Task RunAsync(CommandLineOptions options)
{
    // Check flags first - if any is set, do not hash, only print information
    if (options.ListAlgorithms)
    {
        PrintAvailableAlgorithmsAndExit();
    }
    
    // User is trying to hash a file - check that all fields have value
    if (string.IsNullOrEmpty(options.Algorithm))
    {
        RedPrint("Please specify a algorithm using '-a' or '--algorithm'.");
        Environment.Exit(exitError);
    }
    if (string.IsNullOrEmpty(options.InputFile))
    {
        RedPrint("Please specify a file path using '-f' or '--file'.");
        Environment.Exit(exitError);
    }
    
    // Get the algorithm chosen
    HashService.Algorithm selectedHashAlgorithm;
    try
    {
        selectedHashAlgorithm = HashService.GetAlgorithmFromString(options.Algorithm);
    }
    catch (Exception)
    {
        RedPrint($"Did not recognize algorithm: {options.Algorithm}");
        Environment.Exit(exitError);
        return;  // Fixes var may not be initialized warning
    }
    
    // Checking if file exists
    if (!File.Exists(options.InputFile))
    {
        RedPrint($"Input file does not exist: {options.InputFile}");
        Environment.Exit(exitError);
        return;  // Fixes var may not be initialized warning
    }

    try
    {
        // Get filestream
        var fileStream = File.OpenRead(options.InputFile);
        
        // Setup cancellation token
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        // Setup the progress bar
        var progressBar = new ProgressBar(1000, "Hashing", new ProgressBarOptions
        {
            CollapseWhenFinished = true,
            ShowEstimatedDuration = true
        });
        var progress = progressBar.AsProgress<HashingProgress>(_ => string.Empty, hashingProgress => hashingProgress.PercentageComplete);
        
        // Start hashing
        var hash = await HashService.Hash(selectedHashAlgorithm, fileStream, progress, cancellationToken);
        
        // Print out result
        progressBar.Dispose();
        Console.WriteLine($"{selectedHashAlgorithm.ToString().ToUpper()}: {hash}");
        
        // If verify flag is used, verify the hash
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
        Environment.Exit(exitError);
    }
}

static void PrintAvailableAlgorithmsAndExit()
{
    Console.WriteLine("Available algorithms:");
    foreach (var alg in HashService.AvailableHashAlgorithms)
    {
        Console.WriteLine(alg);
    }
    Environment.Exit(exitSuccess);
}

internal class CommandLineOptions
{
    [Option('f', "file", Required = false, HelpText = "Path to the file to hash", SetName = "Hashing")]
    public string? InputFile { get; init; }

    [Option('a', "algorithm", Default = null, Required = false, HelpText = "Hash algorithm to use", SetName = "Hashing")]
    public string? Algorithm { get; init; }
    
    [Option('v', "verify", Default = null, Required = false, HelpText = "Verify a string towards hash", SetName = "Hashing")]
    public string? Verify { get; init; }
    
    [Option('l', "list", Default = false, Required = false, HelpText = "List the available algorithms", SetName = "Metadata")]
    public bool ListAlgorithms { get; init; }

    [Usage(ApplicationAlias = "hasher")]
    public static IEnumerable<Example> Examples =>
        new List<Example>
        {
            new("Hash file with SHA256", new CommandLineOptions { Algorithm = "sha256", InputFile = "input.txt" }),
            new("Get available algorithms", new CommandLineOptions { ListAlgorithms = true })
        };
}
using CommandLine;
using CommandLine.Text;

await Parser.Default.ParseArguments<CommandLineOptions>(Environment.GetCommandLineArgs())
    .WithNotParsed(errors =>
    {
        Environment.Exit(1);
    })
    .WithParsedAsync(RunAsync);



static async Task RunAsync(CommandLineOptions options)
{
    // Arguments parsed successfully
    Console.WriteLine("File: " + options.InputFile);
    Console.WriteLine("Alg.: " + options.Algorithm);
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
// See https://aka.ms/new-console-template for more information
using CommandLine;

var result = Parser.Default.ParseArguments<CommandLineOptions>(Environment.GetCommandLineArgs())
    .WithNotParsed(errors =>
    {
        Console.WriteLine("Invalid command line arguments.");
        Environment.Exit(1);
    });

// Arguments parsed successfully
Console.WriteLine("File: " + result.Value.InputFile);
Console.WriteLine("Alg.: " + result.Value.Algorithm);

class CommandLineOptions
{
    [Option('f', "file", Required = true, HelpText = "Path to the file to hash")]
    public required string InputFile { get; set; }

    [Option('a', "algorithm", Default = "sha256", Required = false, HelpText = "Hash algorithm to use")]
    public required string Algorithm { get; set; }
}
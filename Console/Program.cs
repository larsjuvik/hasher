// See https://aka.ms/new-console-template for more information
using CommandLine;

var result = Parser.Default.ParseArguments<CommandLineOptions>(Environment.GetCommandLineArgs())
    .WithNotParsed(errors =>
    {
        Console.WriteLine("Invalid command line arguments.");
        Environment.Exit(1);
    });

class CommandLineOptions
{
    [Option('f', "file", Required = true, HelpText = "Path to the file to hash")]
    public string? InputFile { get; set; }

    [Option('a', "algorithm", Required = true, HelpText = "Hash algorithm to use")]
    public string? Algorithm { get; set; }
}
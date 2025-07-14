// See https://aka.ms/new-console-template for more information
// ReSharper disable SuggestVarOrType_BuiltInTypes

using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");

// Determine the OS where the console app is running
string os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows"
    : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux"
    : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS"
    : "Unknown";

Console.WriteLine($"Operating System: {os}");

// Error handling for unsupported OS
if (os == "Unknown")
{
    Console.Error.WriteLine("Unsupported operating system. This application only supports Windows, Linux, and macOS.");
    return;
}

// Ask for a source connection string (MongoDB or CosmosDB)
Console.Write("Enter source connection string (MongoDB or CosmosDB): ");
string sourceConnectionString = Console.ReadLine() ?? string.Empty;

// Ask for a destination connection string (MongoDB or CosmosDB) - Should be MongoDB if #1 is CosmosDB or vice versa
Console.Write("Enter destination connection string (MongoDB or CosmosDB): ");
string targetConnectionString = Console.ReadLine() ?? string.Empty;

// Error handling for empty connection strings
if (string.IsNullOrEmpty(sourceConnectionString) || string.IsNullOrEmpty(targetConnectionString))
{
    Console.Error.WriteLine("Source and/or Target Connection Strings cannot be empty.");
    return;
}

// Determine the Mongo version of the source and destination
Console.WriteLine("Determining MongoDB versions...");

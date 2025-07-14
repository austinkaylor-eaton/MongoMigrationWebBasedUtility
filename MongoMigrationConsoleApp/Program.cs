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
// Ask for a source connection string (MongoDB or CosmosDB)
// Ask for a destination connection string (MongoDB or CosmosDB) - Should be MongoDB if #1 is CosmosDB or vice versa
// Determine the Mongo version of the source and destination

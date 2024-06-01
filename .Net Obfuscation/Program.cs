
using dnlib.DotNet.Writer;
using dnlib.DotNet;
using Obfuscation;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

using Assembly.Obfuscation;




Console.ForegroundColor = ConsoleColor.Yellow;
Console.Write("\n\n\t\tInput an assembly:\n\n\t");

// Gets assembly path.
//string? assemblyPath = args.Length > 0 ? args[0] : Console.ReadLine()?.Replace("\"", "");
//! TEMP CODE FOR TESTING
string? assemblyPath = @"C:\Users\Berkay\Desktop\TempTest\Paint 2.exe";

// Returns if assembly path is null or empty.
if (string.IsNullOrEmpty(assemblyPath)) return;

// Gets assembly file info.
var assemblyFileInfo = new FileInfo(assemblyPath);

// Returns if assembly file doesn't exists.
if (!assemblyFileInfo.Exists) return;

// Creates obfuscator.
var obfuscator = new Obfuscator(assemblyPath);

// Logs completed obfuscation.
obfuscator.ObfuscationCompleted += ObfuscatorObfuscationCompleted;

static void ObfuscatorObfuscationCompleted(object? sender, ObfuscationCompletedEventArgs e)
{
    Console.WriteLine($"\t{e.ObfuscatedObjectType}: {e.InitialFullName}");
}

// Displays assembly file info.
Console.ForegroundColor = ConsoleColor.Green;
Console.Write("\n\n\tFile     :  ");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(assemblyFileInfo.Name);

// Displays assembly info.
Console.ForegroundColor = ConsoleColor.Green;
Console.Write("\tAssembly :  ");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(obfuscator.Assembly.FullName);

Console.ForegroundColor = ConsoleColor.Red;
Console.Write("\n\n\tPress any key to start obfuscation...");
Console.ReadKey();
Console.WriteLine("\n\n");

// Starts stop watch to calculate time that obfuscation took.
var obfuscationStopWatch = Stopwatch.StartNew();

Console.ForegroundColor = ConsoleColor.Cyan;

// Creates obfuscating options.
var obfuscatorOptions = new ObfuscatorOptions
{
    NameGenerator = new ComplexNameGenerator()
};

// Obfuscates assembly.
obfuscator.Obfuscate(obfuscatorOptions);

// Calculates time that obfuscation took.
obfuscationStopWatch.Stop();
double obfusationDurationSeconds = Math.Round(obfuscationStopWatch.Elapsed.TotalSeconds, 3);

// Displays seconds that obfuscation took.
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\n\n\tDuration :  {obfusationDurationSeconds} seconds.");

Console.ForegroundColor = ConsoleColor.Red;
Console.Write("\n\n\tPress any key to save file...");
Console.ReadKey();
Console.WriteLine("\n");

Console.ForegroundColor = ConsoleColor.Cyan;
Console.Write("\tSaving file...");

// Creates output file name.
string outputFileName = assemblyFileInfo.Name[..^assemblyFileInfo.Extension.Length] + " Obfuscated" + assemblyFileInfo.Extension;
string outputFile = Path.Combine(assemblyFileInfo.Directory!.FullName, outputFileName);

// Deletes if file already exists.
if (File.Exists(outputFile)) File.Delete(outputFile);

// Saves output assembly file.
obfuscator.SaveAssemblyFile(outputFile);

// Displays output file info.
Console.ForegroundColor = ConsoleColor.Green;
Console.Write("\n\n\tOutput :  ");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(outputFileName);


Console.ReadKey();



using dnlib.DotNet.Writer;
using dnlib.DotNet;
using Obfuscation;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

using Assembly;
using Assembly.Obfuscation;



// Gets assembly path.
string? assemblyPath;
//! TEMP CODE FOR TESTING
assemblyPath = @"C:\Users\Berkay\Desktop\TempTest\Paint 2.exe";

//if (args.Length > 0) assemblyPath = args[0];
//else
//{
//    Console.ForegroundColor = ConsoleColor.Red;
//    Console.Write("\n\n\t\tInput an assembly:\n\n\t");
//    Console.ForegroundColor = ConsoleColor.Yellow;

//    assemblyPath = Console.ReadLine()?.Replace("\"", "");
//}

// Returns if assembly path is null or empty.
if (string.IsNullOrEmpty(assemblyPath)) return;

// Gets assembly file info.
var assemblyFileInfo = new FileInfo(assemblyPath);

// Returns if assembly file doesn't exists.
if (!assemblyFileInfo.Exists) return;

// Creates obfuscator.
var obfuscator = new Obfuscator(assemblyPath);

// Logs completed obfuscations.
obfuscator.NameChanged += ObfuscatorNameChanged;
obfuscator.ValueModified += ObfuscatorValueModified;

static void ObfuscatorNameChanged(object? sender, NameChangedEventArgs e)
{
    Console.WriteLine($"\t{e.ObjectType}: {e.InitialName}");
}
static void ObfuscatorValueModified(object? sender, ValueModifiedEventArgs e)
{
    Console.WriteLine($"\t{e.ObjectType}: {e.InitialValue}");

}

// Displays assembly file info.
Console.ForegroundColor = ConsoleColor.Green;
Console.Write("\n\n\tFile     :  ");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(assemblyFileInfo.Name);

// Displays assembly info.
Console.ForegroundColor = ConsoleColor.Green;
Console.Write("\n\tAssembly :  ");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(obfuscator.Assembly.FullName);

// Displays resolved dependencies.
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n\tModules:");
Console.ForegroundColor = ConsoleColor.Blue;
foreach (var module in obfuscator.Assembly.Modules)
{
    Console.WriteLine($"\t- {module.Name}");
}

// Displays dependencies.
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n\tDependencies:");
Console.ForegroundColor = ConsoleColor.Blue;
foreach (var assembly in obfuscator.GetAllDependencies())
{
    Console.WriteLine($"\t- {assembly.Name}");
}

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
Console.Write($"\n\n\tDuration :  ");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"{obfusationDurationSeconds} seconds.");


Console.ForegroundColor = ConsoleColor.Red;
Console.Write("\n\n\tPress any key to save file...");
Console.ReadKey();
Console.WriteLine("\n\n");

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
Console.Write("\n\n\n\tOutput :  ");
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine(outputFileName);


Console.ReadKey();

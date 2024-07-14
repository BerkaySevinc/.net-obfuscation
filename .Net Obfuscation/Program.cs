
using dnlib.DotNet.Writer;
using dnlib.DotNet;
using Obfuscation;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

using Assembly;
using Assembly.Obfuscation;


// TODO: take options as an input from user
// TODO: better logs
// TODO: WriteLineColored(text, color) like method, for Console.WriteLine with text color parameter



// Gets assembly path.
string? assemblyPath;

if (args.Length > 0) assemblyPath = args[0];
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("\n\n\t\tInput an assembly:\n\n\t");
    Console.ForegroundColor = ConsoleColor.Yellow;

    assemblyPath = Console.ReadLine()?.Replace("\"", "");
}

// Returns if assembly path is null or empty.
if (string.IsNullOrEmpty(assemblyPath)) return;

// Gets assembly file info.
var assemblyFileInfo = new FileInfo(assemblyPath);

// Returns if assembly file doesn't exists.
if (!assemblyFileInfo.Exists) return;

// Creates obfuscator.
var obfuscator = new Obfuscator(assemblyPath);

// Logs completed obfuscations.
obfuscator.MemberGenerated += ObfuscatorMemberGenerated;
obfuscator.ValueModified += ObfuscatorValueModified;
obfuscator.NameChanged += ObfuscatorNameChanged;

static void ObfuscatorMemberGenerated(object? sender, MemberGeneratedEventArgs e)
{
    Console.WriteLine($"\tJunk {e.ObjectType} Generated ({e.ParentObject.Name}): {e.Objects.Count}");
}
static void ObfuscatorValueModified(object? sender, ValueModifiedEventArgs e)
{
    Console.WriteLine($"\t{e.ObjectType} Value Modified: {e.InitialValue}");
}
static void ObfuscatorNameChanged(object? sender, NameChangedEventArgs e)
{
    Console.WriteLine($"\t{e.ObjectType} Name Obfuscated: {e.InitialName} => {e.Object?.Name}");
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
    JunkFieldCount = 10,


    ObfuscatedNameGenerator = new ComplexNameGenerator(),

    ObfuscateAssemblyName = true,
    ObfuscateModuleNames = true,
    ObfuscateTypeNames = true,

    ObfuscateEventNames = true,
    ObfuscateFieldNames = true,
    ObfuscateMethodNames = true,
    ObfuscateParameterNames = true,
    ObfuscatePropertyNames = true,


    ObfuscateStringValues = true,
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

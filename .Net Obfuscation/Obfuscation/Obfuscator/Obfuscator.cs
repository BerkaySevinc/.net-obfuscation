using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Obfuscation;


// add signiture
// add logger (event or smth)
// be sure that every name is different (via hashsetle etc.) e.g: https://stackoverflow.com/questions/4616685/how-to-generate-a-random-string-and-specify-the-length-you-want-or-better-gene
// make renamer methods simpler

public partial class Obfuscator : AssemblyModifier
{
    public Obfuscator(string inputAssemblyFile) : base(inputAssemblyFile) { }


    public event EventHandler<ObfuscationCompletedEventArgs>? ObfuscationCompleted;

    protected virtual void OnObfuscationCompleted(ObfuscationCompletedEventArgs e) =>
        ObfuscationCompleted?.Invoke(this, e);


    public void Obfuscate(ObfuscatorOptions? options = null)
    {
        // Creates default options if given is null.
        options ??= new ObfuscatorOptions();

        // Obfuscates assembly name.
        if (options.ObfuscateAssemblyName)
            ObfuscateAssemblyName(options);

        // Obfuscates module names.
        if (options.ObfuscateModuleNames)
            ObfuscateModuleNames(options);

        // Obfuscates type names.
        if (options.ObfuscateTypeNames)
            ObfuscateTypeNames(options);

        // Obfuscates method names.
        if (options.ObfuscateMethodNames)
            ObfuscateMethodNames(options);

        // Obfuscates field names.
        if (options.ObfuscateFieldNames)
            ObfuscateFieldNames(options);

        // Obfuscates property names.
        if (options.ObfuscatePropertyNames)
            ObfuscatePropertyNames(options);
    }
}

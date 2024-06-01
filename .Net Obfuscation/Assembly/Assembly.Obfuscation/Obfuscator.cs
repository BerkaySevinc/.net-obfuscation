using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Assembly.Obfuscation;


// add signiture to renamer
// be sure that every name is different for complexnamegenerator (using hashsetle etc.) e.g: https://stackoverflow.com/questions/4616685/how-to-generate-a-random-string-and-specify-the-length-you-want-or-better-gene

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
        var nameGenerator = options.NameGenerator;

        // Obfuscates assembly name.
        if (options.ObfuscateAssemblyName)
            ObfuscateAssemblyName(nameGenerator);

        // Obfuscates module names.
        if (options.ObfuscateModuleNames)
            ObfuscateModuleNames(nameGenerator);

        // Obfuscates type names.
        if (options.ObfuscateTypeNames)
            ObfuscateTypeNames(nameGenerator);

        // Obfuscates method names.
        if (options.ObfuscateMethodNames)
            ObfuscateMethodNames(nameGenerator);

        // Obfuscates field names.
        if (options.ObfuscateFieldNames)
            ObfuscateFieldNames(nameGenerator);

        // Obfuscates property names.
        if (options.ObfuscatePropertyNames)
            ObfuscatePropertyNames(nameGenerator);

        // Obfuscates parameter names.
        if (options.ObfuscateParameterNames)
            ObfuscateParameterNames(nameGenerator);

        // Resets obfuscated name generator.
        options.NameGenerator.Reset();

    }
}

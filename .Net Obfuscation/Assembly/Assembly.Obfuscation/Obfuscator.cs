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



public class Obfuscator : AssemblyModifier
{
    public Obfuscator(string inputAssemblyFile) : base(inputAssemblyFile) { }


    public event EventHandler<NameChangedEventArgs>? NameChanged;
    public event EventHandler<ValueModifiedEventArgs>? ValueModified;


    protected virtual void OnNameChanged(NameChangedEventArgs e) =>
        NameChanged?.Invoke(this, e);
    protected virtual void OnValueModified(ValueModifiedEventArgs e) =>
        ValueModified?.Invoke(this, e);


    public void Obfuscate(ObfuscatorOptions? options = null)
    {
        // Creates default options if given is null.
        options ??= new ObfuscatorOptions();


        // Creates renamer.
        var renamer = new ObfuscatorRenamer(Assembly, options.NameGenerator);
        renamer.NameChanged += NameChanged;

        // Creates value modifier.
        var valueModifier = new ObfuscatorValueModifier(Assembly);
        valueModifier.ValueModified += ValueModified;


        // Obfuscates assembly name.
        if (options.ObfuscateAssemblyName)
            renamer.ObfuscateAssemblyName();

        // Obfuscates module names.
        if (options.ObfuscateModuleNames)
            renamer.ObfuscateModuleNames();

        // Obfuscates type names.
        if (options.ObfuscateTypeNames)
            renamer.ObfuscateTypeNames();

        // Obfuscates method names.
        if (options.ObfuscateMethodNames)
            renamer.ObfuscateMethodNames();

        // Obfuscates field names.
        if (options.ObfuscateFieldNames)
            renamer.ObfuscateFieldNames();

        // Obfuscates property names.
        if (options.ObfuscatePropertyNames)
            renamer.ObfuscatePropertyNames();

        // Obfuscates parameter names.
        if (options.ObfuscateParameterNames)
            renamer.ObfuscateParameterNames();

        // Resets obfuscated name generator.
        options.NameGenerator.Reset();


        // Encodes string values.
        if (options.ObfuscateStringValues)
            valueModifier.EncodeStringValues();

    }
}

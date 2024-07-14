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


// TODO: better EventArgs (ResultName prop, parent prop for some of EventArgs)
// TODO: obfuscationType prop (name, value, junk vs.) for events, so logger can know what type is it
// TODO: check special cases like IsRuntimeSpecial etc. for NameGenerators, ValueModifiers, MemberGenerators etc.
// TODO: namespace obfuscation or hidden namespace, and junk namespaces




public class Obfuscator : AssemblyModifier
{
    public Obfuscator(string inputAssemblyFile) : base(inputAssemblyFile) { }


    public event EventHandler<NameChangedEventArgs>? NameChanged;
    public event EventHandler<ValueModifiedEventArgs>? ValueModified;
    public event EventHandler<MemberGeneratedEventArgs>? MemberGenerated;


    protected virtual void OnNameChanged(NameChangedEventArgs e) =>
        NameChanged?.Invoke(this, e);
    protected virtual void OnValueModified(ValueModifiedEventArgs e) =>
        ValueModified?.Invoke(this, e);
    protected virtual void OnMemberGenerated(MemberGeneratedEventArgs e) =>
        MemberGenerated?.Invoke(this, e);


    public void Obfuscate(ObfuscatorOptions? options = null)
    {
        // Creates default options if given is null.
        options ??= new ObfuscatorOptions();

        // Generates junks.
        GenerateJunks(options);

        // Obfuscates values.
        ObfuscateValues(options);

        // Obfuscates names.
        ObfuscateNames(options);
    }

    private void GenerateJunks(ObfuscatorOptions options)
    {
        // Gets & resets name generator.
        NameGenerator junkNameGenerator = options.JunkNameGenerator;
        junkNameGenerator.Reset();

        // Creates junk generator.
        var junkGenerator = new JunkGenerator(Assembly, junkNameGenerator);
        junkGenerator.MemberGenerated += MemberGenerated;

        // Generates junk fields.
        junkGenerator.GenerateField(options.JunkFieldCount);
    }

    private void ObfuscateNames(ObfuscatorOptions options)
    {
        // Gets & resets name generator.
        NameGenerator obfuscatedNameGenerator = options.ObfuscatedNameGenerator;
        obfuscatedNameGenerator.Reset();

        // Creates renamer.
        var renamer = new NameObfuscator(Assembly, obfuscatedNameGenerator);
        renamer.NameChanged += NameChanged;

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

        // Obfuscates event names.
        if (options.ObfuscateEventNames)
            renamer.ObfuscateEventNames();

        // Obfuscates parameter names.
        if (options.ObfuscateParameterNames)
            renamer.ObfuscateParameterNames();

        // Resets name generator.
        obfuscatedNameGenerator.Reset();
    }

    private void ObfuscateValues(ObfuscatorOptions options)
    {
        // Creates value modifier.
        var valueModifier = new ValueObfuscator(Assembly);
        valueModifier.ValueModified += ValueModified;

        // Obfuscates string values.
        if (options.ObfuscateStringValues)
            valueModifier.EncodeStringValues();
    }
}

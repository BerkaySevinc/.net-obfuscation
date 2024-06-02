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

// ( encoderda encoding type vs. her method çağırıldığında oluşturulmasın, methodun dışına taşı fieldda dursun)
// namegeneratorlarda set edilebilir yap propları, onlyxin ismini değişip OneLetter fln yap
// selectmany methodunu kullan

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

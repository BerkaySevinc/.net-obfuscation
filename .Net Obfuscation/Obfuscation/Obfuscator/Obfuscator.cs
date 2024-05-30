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
// her type namein vs. farklı olduğundan emin ol (hashsetle vs.) örn: https://stackoverflow.com/questions/4616685/how-to-generate-a-random-string-and-specify-the-length-you-want-or-better-gene


public partial class Obfuscator : AssemblyModifier
{
    public Obfuscator(string inputAssemblyFile) : base(inputAssemblyFile) { }

    public void Obfuscate(ObfuscatorOptions? options = null)
    {
        // Creates default options if given is null.
        options ??= new ObfuscatorOptions();

        // Loops through assembly modules.
        foreach (var module in Assembly.Modules)
        {
            // Loops through types.
            foreach (var type in module.Types)
            {
                // Obfuscates method names.
                if (options.ObfuscateMethodNames)
                    ObfuscateMethodNames(type, options);
            }
        }

    }



}

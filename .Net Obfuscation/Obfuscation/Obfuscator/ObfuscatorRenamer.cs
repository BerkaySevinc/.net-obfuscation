using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Obfuscation;


public partial class Obfuscator
{

    private static void ObfuscateMethodNames(TypeDef type, ObfuscatorOptions options)
    {
        // Loops through methods.
        foreach (var method in type.Methods)
        {
            if (!IsMethodNameObfuscatable(method)) continue;

            string obfuscatedName = GenerateObfuscatedName(options.ObfuscationType);
            method.Name = obfuscatedName;
        }
    }
    private static bool IsMethodNameObfuscatable(MethodDef method)
    {
        if (method.IsSpecialName) return false;

        return true;
    }


}

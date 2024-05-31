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
    private static readonly Random random = new();
    private static string GenerateObfuscatedName(NameObfuscationType type)
    {
        // Chooses obfuscated name generator.
        Func<string> obfuscatedNameGenerator = type switch
        {
            NameObfuscationType.Complex => GenerateComplexName,
        };

        // Generates obfuscated name.
        return obfuscatedNameGenerator.Invoke();
    }

    private static string GenerateComplexName()
    {
        // Gets random name length.
        int nameLength = random.Next(ComplexNameMinLength, ComplexNameMaxLength + 1);

        // Creates random name.
        var chars = new char[nameLength];
        for (int i = 0; i < nameLength; ++i)
        {
            chars[i] = ComplexNameChars[random.Next(ComplexNameChars.Length)];
        }

        var complexName = new string(chars);

        return complexName;
    }
}

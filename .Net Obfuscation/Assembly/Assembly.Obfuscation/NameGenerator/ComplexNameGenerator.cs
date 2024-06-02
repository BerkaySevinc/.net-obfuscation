using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly.Obfuscation;


public class ComplexNameGenerator : NameGenerator
{

    private const string ComplexNameChars =
        "的一是不了人我在有他这为之大来以个中上们到说国和地也子时道出而要于就下得可你年生"
        + "abcdefghijklmnopqrstuvwxyz"
        + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        + "_";

    public int ComplexNameAverageLength { get; set; } = 20;
    public int ComplexNameLengthDeviation { get; set; } = 5;


    private static readonly Random random = new();
    public override string GenerateName()
    {
        // Calculates min-max name lengths.
        int ComplexNameMinLength = ComplexNameAverageLength - ComplexNameLengthDeviation;
        int ComplexNameMaxLength = ComplexNameAverageLength + ComplexNameLengthDeviation;

        // Gets random name length.
        int nameLength = random.Next(ComplexNameMinLength, ComplexNameMaxLength + 1);

        // Creates random name.
        var chars = new char[nameLength];
        for (int i = 0; i < nameLength; ++i)
        {
            chars[i] = ComplexNameChars[random.Next(ComplexNameChars.Length)];
        }

        var generatedName = new string(chars);

        return generatedName;
    }

    public override void Reset() { }
}

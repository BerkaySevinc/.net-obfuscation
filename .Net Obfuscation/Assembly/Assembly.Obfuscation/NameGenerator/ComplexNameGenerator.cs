using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly.Obfuscation;


public class ComplexNameGenerator : NameGenerator
{
    public string ComplexNameChars { get; set; } =
        "的一是不了人我在有他这为之大来以个中上们到说国和地也子时道出而要于就下得可你年生"
        + "abcdefghijklmnopqrstuvwxyz"
        + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        + "_";

    public int ComplexNameAverageLength { get; set; } = 20;
    public int ComplexNameLengthDeviation { get; set; } = 5;


    private char[]? _signatureAsArray;
    private string? _signature;
    public string? Signature
    {
        get => _signature;
        set
        {
            _signature = value;
            _signatureAsArray = Signature?.ToCharArray();
        }
    }



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
        for (int i = 0; i < nameLength; i++)
        {
            chars[i] = ComplexNameChars[random.Next(ComplexNameChars.Length)];
        }

        if (Signature is not null)
        {
            // Gets random signature count.
            int signatureCount = random.Next(((nameLength - 1) / (Signature.Length + 1)) + 1);

            if (signatureCount > 0)
            {
                // Gets space length details between signatures.
                int totalSpaceLength = nameLength - Signature.Length * signatureCount;
                int spaceCount = signatureCount + 1;
                int averageSpaceLenght = totalSpaceLength / spaceCount;
                int spaceLengthLeft = totalSpaceLength % spaceCount;

                // Adds signatures.
                int signatureIndex = averageSpaceLenght;
                int plusCount = averageSpaceLenght + Signature.Length;
                for (int i = spaceCount; i > 1; i--)
                {
                    // Selects by probablity = (number needed) / (number left)
                    if (spaceLengthLeft > 0 && random.Next(i) < spaceLengthLeft)
                    {
                        signatureIndex++;
                        spaceLengthLeft--;
                    }

                    Array.Copy(_signatureAsArray!, 0, chars, signatureIndex, Signature.Length);

                    signatureIndex += plusCount;
                }
            }
        }

        var generatedName = new string(chars);

        return generatedName;
    }

    public override void Reset() { }
}

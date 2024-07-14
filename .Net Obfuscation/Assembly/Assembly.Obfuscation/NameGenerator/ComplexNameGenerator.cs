using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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


    private int _complexNameMinLength;
    private int _complexNameMaxLength;

    private int _complexNameAverageLength = 20;
    public int ComplexNameAverageLength
    {
        get => _complexNameAverageLength;
        set
        {
            _complexNameAverageLength = value;
            SetNameLengths();
        }
    }
    public int ComplexNameLengthDeviation
    {
        get => complexNameLengthDeviation;
        set
        {
            complexNameLengthDeviation = value;
            SetNameLengths();
        }
    }

    private char[]? _signatureAsArray;
    private string? _signature;
    private int complexNameLengthDeviation = 5;

    public string? Signature
    {
        get => _signature;
        set
        {
            _signature = value;
            _signatureAsArray = Signature?.ToCharArray();
        }
    }


    public ComplexNameGenerator() : base() => SetNameLengths();



    private static readonly Random random = new();
    public override string GenerateName(IDnlibDef? target)
    {
        // Gets random name length.
        int nameLength = random.Next(_complexNameMinLength, _complexNameMaxLength + 1);

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

    private void SetNameLengths()
    {
        // Calculates min-max name lengths.
        _complexNameMinLength = ComplexNameAverageLength - ComplexNameLengthDeviation;
        _complexNameMaxLength = ComplexNameAverageLength + ComplexNameLengthDeviation;
    }

    public override void Reset() { }
}

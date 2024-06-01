using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Assembly.Obfuscation;

public class OnlyXNameGenerator : NameGenerator
{

    private const char SelectedCharLower = 'x';

    private static readonly char SelectedCharUpper = char.ToUpper(SelectedCharLower);


    private int generatedNameCount = 0;
    public override string GenerateName()
    {
        int charCount = (int)Math.Log2(generatedNameCount + 2);

        int firstOfChars = (int)Math.Pow(2, charCount) - 2;
        int indexOfChars = generatedNameCount - firstOfChars;

        string binaryCombination = Convert.ToString(indexOfChars, 2);

        string generatedName = binaryCombination
            .Replace('0', SelectedCharLower)
            .Replace('1', SelectedCharUpper)
            .PadLeft(charCount, SelectedCharLower);

        generatedNameCount++;

        return generatedName;
    }

    public override void Reset()
    {
        generatedNameCount = 0;
    }
}

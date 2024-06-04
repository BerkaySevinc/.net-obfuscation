using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Assembly.Obfuscation;

public class OneLetterNameGenerator : NameGenerator
{

    private char _letterLowerCase = 'x';
    private char _letterUpperCase = 'X';
    public char Letter
    {
        get => _letterLowerCase;
        set
        {
            _letterLowerCase = char.ToLower(value);
            _letterUpperCase = char.ToUpper(value);
        }
    }

    private int generatedNameCount = 0;
    public override string GenerateName()
    {
        int charCount = (int)Math.Log2(generatedNameCount + 2);

        int firstOfChars = (int)Math.Pow(2, charCount) - 2;
        int indexOfChars = generatedNameCount - firstOfChars;

        string binaryCombination = Convert.ToString(indexOfChars, 2);

        string generatedName = binaryCombination
            .Replace('0', _letterLowerCase)
            .Replace('1', _letterUpperCase)
            .PadLeft(charCount, _letterLowerCase);

        generatedNameCount++;

        return generatedName;
    }

    public override void Reset()
    {
        generatedNameCount = 0;
    }
}

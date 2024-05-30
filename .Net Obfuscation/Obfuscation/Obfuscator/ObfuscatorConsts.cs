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
    private const int ComplexNameAverageLength = 20;

    private const int ComplexNameLengthDeviation = 5;

    private const int ComplexNameMinLength = ComplexNameAverageLength - ComplexNameLengthDeviation;
    private const int ComplexNameMaxLength = ComplexNameAverageLength + ComplexNameLengthDeviation;


    private const string ComplexNameChars = 
        "的一是不了人我在有他这为之大来以个中上们到说国和地也子时道出而要于就下得可你年生" 
        + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" 
        + "_";
}

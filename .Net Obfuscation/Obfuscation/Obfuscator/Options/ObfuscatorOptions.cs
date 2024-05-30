using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;



namespace Obfuscation;


public class ObfuscatorOptions
{
    public NameObfuscationType ObfuscationType { get; set; } = NameObfuscationType.Complex;

    public bool ObfuscateMethodNames { get; set; } = true;
}

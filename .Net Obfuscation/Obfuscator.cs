using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;



namespace Obfuscation;


public class Obfuscator : AssemblyModifier
{
    public Obfuscator(string inputAssemblyFile) : base(inputAssemblyFile) { }

    public void Obfuscate()
    {
        
    }
}

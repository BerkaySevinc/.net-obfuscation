using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace Assembly;

public abstract class AssemblyModifier
{
    public AssemblyDef Assembly { get; }

    public AssemblyModifier(string inputAssemblyFile)
    {
        Assembly = AssemblyDef.Load(inputAssemblyFile);
    }

    public void SaveAssemblyFile(string outputFile)
    {
        Assembly.Write(outputFile);
    }
}

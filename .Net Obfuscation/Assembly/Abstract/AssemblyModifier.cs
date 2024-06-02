using System;
using System.Collections;
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

    public IEnumerable<AssemblyDef> GetAllDependencies() =>
        Assembly.Modules.SelectMany(GetModuleDependencies);

    public static IEnumerable<AssemblyDef> GetModuleDependencies(ModuleDef module)
    {
        var resolver = new AssemblyResolver();

        var assemblyReferences = module.GetAssemblyRefs();
        var resolvedDependencies = assemblyReferences.Select(r => resolver.ResolveThrow(r, module));

        return resolvedDependencies;
    }

    public void SaveAssemblyFile(string outputFile)
    {
        Assembly.Write(outputFile);
    }
}

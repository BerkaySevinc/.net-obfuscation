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
    public IList<ModuleDef> Modules { get; }

    public AssemblyModifier(AssemblyDef assembly) =>
        (Assembly, Modules) = (assembly, assembly.Modules);
    public AssemblyModifier(string inputAssemblyFile) : this(AssemblyDef.Load(inputAssemblyFile)) { }


    public IEnumerable<AssemblyDef> GetAllDependencies() =>
        Assembly.Modules.SelectMany(GetModuleDependencies);

    public static IEnumerable<AssemblyDef> GetModuleDependencies(ModuleDef module)
    {
        var resolver = new AssemblyResolver();

        var assemblyReferences = module.GetAssemblyRefs();
        var resolvedDependencies = assemblyReferences.Select(r => resolver.ResolveThrow(r, module));

        return resolvedDependencies;
    }


    protected static IEnumerable<TypeDef> GetMatchingTypesFromModuleDependencies(ModuleDef module, string name, bool matchFullName)
    {
        var matchingTypes =
            GetModuleDependencies(module)
            .SelectMany(a => a.Modules)
            .SelectMany(m => m.Types)
            .Where(type => (matchFullName ? type.FullName : type.Name) == name);

        return matchingTypes;
    }
    protected static IEnumerable<TypeDef> GetMatchingTypesFromModuleDependencies(ModuleDef module, string name) =>
        GetMatchingTypesFromModuleDependencies(module, name, false);
    protected static IEnumerable<TypeDef> GetMatchingTypesFromModuleDependencies(ModuleDef module, Type type, bool matchFullName)
    {
        string? nameToCompare = matchFullName ? type.FullName : type.Name;

        if (nameToCompare is null) return Enumerable.Empty<TypeDef>();

        return GetMatchingTypesFromModuleDependencies(module, nameToCompare, matchFullName);
    }
    protected static IEnumerable<TypeDef> GetMatchingTypesFromModuleDependencies(ModuleDef module, Type type) =>
        GetMatchingTypesFromModuleDependencies(module, type, false);



    public void SaveAssemblyFile(string outputFile) => 
        Assembly.Write(outputFile);
}

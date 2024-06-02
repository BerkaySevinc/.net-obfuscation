using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly;


public abstract class ValueModifier
{
    public AssemblyDef Assembly { get; }

    public ValueModifier(AssemblyDef assembly) => Assembly = assembly;


    public event EventHandler<ValueModifiedEventArgs>? ValueModified;

    protected virtual void OnValueModified(ValueModifiedEventArgs e) =>
        ValueModified?.Invoke(this, e);


    protected static IEnumerable<TypeDef> GetMatchingTypesFromModuleDependencies(ModuleDef module, string name, bool matchFullName)
    {
        var matchingTypes =
            AssemblyModifier.GetModuleDependencies(module)
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

}

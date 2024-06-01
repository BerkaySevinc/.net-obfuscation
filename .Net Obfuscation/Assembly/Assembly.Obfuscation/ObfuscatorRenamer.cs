using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly.Obfuscation;


public partial class Obfuscator
{
    private void ObfuscateAssemblyName(NameGenerator nameGenerator)
    {
        ObfuscateObjectNameDefault(ObfuscatedObjectType.Assembly, Assembly, nameGenerator);
    }


    private void ObfuscateModuleNames(NameGenerator nameGenerator)
    {
        nameGenerator.Reset();

        foreach (var module in Assembly.Modules)
        {
            ObfuscateObjectNameDefault(ObfuscatedObjectType.Module, module, nameGenerator);
        }
    }


    private void ObfuscateTypeNames(NameGenerator nameGenerator)
    {
        foreach (var module in Assembly.Modules)
        {
            nameGenerator.Reset();

            foreach (var type in module.Types)
            {
                if (type.IsSpecialName) continue;

                string initialName = type.Name;
                string initialFullName = type.FullName;

                string obfuscatedName = nameGenerator.GenerateName();

                type.Name = obfuscatedName;

                foreach (var referancedModule in Assembly.Modules)
                {
                    // Renames referances.
                    foreach (TypeRef TypeRef in referancedModule.GetTypeRefs())
                        if (TypeRef.FullName == initialFullName)
                            TypeRef.Name = obfuscatedName;

                    // Renames resources.
                    foreach (Resource resource in referancedModule.Resources)
                    {
                        if (!resource.Name.Contains(".")) continue;
                        if (resource.Name.ToString()[..resource.Name.IndexOf(".")] != initialFullName) continue;

                        resource.Name = resource.Name.Replace(initialName, obfuscatedName);
                    }
                }

                var args = new ObfuscationCompletedEventArgs(ObfuscatedObjectType.Type, type, initialFullName, initialName);
                OnObfuscationCompleted(args);
            }
        }
    }

    private void ObfuscateMethodNames(NameGenerator nameGenerator)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
            {
                nameGenerator.Reset();

                foreach (var method in type.Methods)
                {
                    if (method.IsSpecialName) continue;

                    ObfuscateObjectNameDefault(ObfuscatedObjectType.Method, method, nameGenerator);
                }
            }
    }

    private void ObfuscateFieldNames(NameGenerator nameGenerator)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
            {
                nameGenerator.Reset();

                foreach (var field in type.Fields)
                {
                    if (field.IsSpecialName) continue;

                    ObfuscateObjectNameDefault(ObfuscatedObjectType.Field, field, nameGenerator);
                }
            }
    }


    private void ObfuscatePropertyNames(NameGenerator nameGenerator)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
            {
                nameGenerator.Reset();

                foreach (var property in type.Properties)
                {
                    if (property.IsSpecialName) continue;

                    ObfuscateObjectNameDefault(ObfuscatedObjectType.Property, property, nameGenerator);
                }
            }
    }


    private void ObfuscateParameterNames(NameGenerator nameGenerator)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
                foreach (var method in type.Methods)
                {
                    nameGenerator.Reset();

                    foreach (var parameter in method.Parameters.Where(p => p.HasParamDef).Select(p => p.ParamDef))
                    {
                        string initialName = parameter.Name;
                        string initialFullName = parameter.FullName;

                        string obfuscatedName = nameGenerator.GenerateName();
                        parameter.Name = obfuscatedName;

                        var args = new ObfuscationCompletedEventArgs(ObfuscatedObjectType.Parameter, null, initialFullName, initialName);
                        OnObfuscationCompleted(args);
                    }
                }
    }


    private void ObfuscateObjectNameDefault(ObfuscatedObjectType obfuscatedObjectType, IDnlibDef target, NameGenerator nameGenerator)
    {
        string initialName = target.Name;
        string initialFullName = target.FullName;

        string obfuscatedName = nameGenerator.GenerateName();
        target.Name = obfuscatedName;

        var args = new ObfuscationCompletedEventArgs(obfuscatedObjectType, target, initialFullName, initialName);
        OnObfuscationCompleted(args);
    }
}

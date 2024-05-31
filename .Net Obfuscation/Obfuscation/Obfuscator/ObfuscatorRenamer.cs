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
    private void ObfuscateAssemblyName(ObfuscatorOptions options)
    {
        ObfuscateObjectNameDefault(ObfuscatedObjectType.Assembly, Assembly, options);
    }


    private void ObfuscateModuleNames(ObfuscatorOptions options)
    {
        foreach (var module in Assembly.Modules)
        {
            ObfuscateObjectNameDefault(ObfuscatedObjectType.Module, module, options);
        }
    }


    private void ObfuscateTypeNames(ObfuscatorOptions options)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
            {
                if (type.IsSpecialName) continue;

                string initialName = type.Name;
                string initialFullName = type.FullName;

                string obfuscatedName = GenerateObfuscatedName(options.ObfuscationType);

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

    private void ObfuscateMethodNames(ObfuscatorOptions options)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
                foreach (var method in type.Methods)
                {
                    if (method.IsSpecialName) continue;

                    ObfuscateObjectNameDefault(ObfuscatedObjectType.Method, method, options);
                }
    }

    private void ObfuscateFieldNames(ObfuscatorOptions options)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
                foreach (var field in type.Fields)
                {
                    if (field.IsSpecialName) continue;

                    ObfuscateObjectNameDefault(ObfuscatedObjectType.Field, field, options);
                }
    }


    private void ObfuscatePropertyNames(ObfuscatorOptions options)
    {
        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
                foreach (var property in type.Properties)
                {
                    if (property.IsSpecialName) continue;

                    ObfuscateObjectNameDefault(ObfuscatedObjectType.Property, property, options);
                }
    }


    private void ObfuscateObjectNameDefault(ObfuscatedObjectType obfuscatedObjectType, IDnlibDef target, ObfuscatorOptions options)
    {
        string initialName = target.Name;
        string initialFullName = target.FullName;

        string obfuscatedName = GenerateObfuscatedName(options.ObfuscationType);
        target.Name = obfuscatedName;

        var args = new ObfuscationCompletedEventArgs(obfuscatedObjectType, target, initialFullName, initialName);
        OnObfuscationCompleted(args);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Assembly.Obfuscation;



public class NameObfuscator : Renamer
{
    public NameObfuscator(AssemblyDef assembly, NameGenerator nameGenerator) : base(assembly, nameGenerator) { }


    public void ObfuscateAssemblyName()
    {
        NameGenerator.Reset();

        ObfuscateMemberNameDefault(MemberObjectType.Assembly, Assembly);
    }


    public void ObfuscateModuleNames()
    {
        NameGenerator.Reset();

        foreach (var module in Assembly.Modules)
        {
            ObfuscateMemberNameDefault(MemberObjectType.Module, module);
        }
    }


    public void ObfuscateTypeNames()
    {
        foreach (var module in Assembly.Modules)
        {
            NameGenerator.Reset();

            foreach (var type in module.Types)
            {
                // Skips special cases
                if (type.IsSpecialName) continue;

                string initialName = type.Name;
                string initialFullName = type.FullName;

                string obfuscatedName = NameGenerator.GenerateName(type);

                type.Name = obfuscatedName;

                foreach (var referancedModule in Assembly.Modules)
                {
                    // Renames referances.
                    foreach (TypeRef TypeRef in referancedModule.GetTypeRefs())
                    {
                        if (TypeRef.FullName == initialFullName)
                            TypeRef.Name = obfuscatedName;
                    }

                    // Renames resources.
                    foreach (Resource resource in referancedModule.Resources)
                    {
                        if (!resource.Name.Contains(".")) continue;
                        if (resource.Name.ToString()[..resource.Name.LastIndexOf(".")] != initialFullName) continue;

                        resource.Name = resource.Name.Replace(initialName, obfuscatedName);
                    }
                }

                var args = new NameChangedEventArgs(MemberObjectType.Type, type, initialName);
                OnNameChanged(args);
            }
        }
    }

    public void ObfuscateMethodNames()
    {
        foreach (var type in Assembly.Modules.SelectMany(m => m.Types))
        {
            NameGenerator.Reset();

            foreach (var method in type.Methods)
            {
                if (method.IsSpecialName) continue;

                ObfuscateMemberNameDefault(MemberObjectType.Method, method);
            }
        }
    }

    public void ObfuscateFieldNames()
    {
        foreach (var type in Assembly.Modules.SelectMany(m => m.Types))
        {
            NameGenerator.Reset();

            foreach (var field in type.Fields)
            {
                if (field.IsSpecialName) continue;

                ObfuscateMemberNameDefault(MemberObjectType.Field, field);
            }
        }
    }


    public void ObfuscatePropertyNames()
    {
        foreach (var type in Assembly.Modules.SelectMany(m => m.Types))
        {
            NameGenerator.Reset();

            foreach (var property in type.Properties)
            {
                if (property.IsSpecialName) continue;

                ObfuscateMemberNameDefault(MemberObjectType.Property, property);
            }
        }
    }

    public void ObfuscateEventNames()
    {
        foreach (var type in Assembly.Modules.SelectMany(m => m.Types))
        {
            NameGenerator.Reset();

            foreach (var eventDef in type.Events)
            {
                if (eventDef.IsSpecialName) continue;

                ObfuscateMemberNameDefault(MemberObjectType.Event, eventDef);
            }
        }
    }

    public void ObfuscateParameterNames()
    {
        foreach (var method in Assembly.Modules.SelectMany(m => m.Types).SelectMany(t => t.Methods))
        {
            NameGenerator.Reset();

            foreach (var parameter in method.ParamDefs)
            {
                string initialName = parameter.Name;

                string obfuscatedName = NameGenerator.GenerateName();
                parameter.Name = obfuscatedName;

                var args = new NameChangedEventArgs(MemberObjectType.Parameter, null, initialName);
                OnNameChanged(args);
            }
        }
    }



    public void ObfuscateMemberNameDefault(MemberObjectType objectType, IDnlibDef target)
    {
        string initialName = target.Name;

        string obfuscatedName = NameGenerator.GenerateName(target);
        target.Name = obfuscatedName;

        var args = new NameChangedEventArgs(objectType, target, initialName);
        OnNameChanged(args);
    }
}

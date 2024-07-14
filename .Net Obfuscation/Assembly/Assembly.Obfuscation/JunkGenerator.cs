using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;

namespace Assembly.Obfuscation;


// UNDONE: junk generate for other members (method, prop, namespace etc.), with other types (string, int, short, bool etc.), and with various values


public class JunkGenerator : MemberGenerator
{
    public JunkGenerator(AssemblyDef assembly, NameGenerator nameGenerator) : base(assembly, nameGenerator) { }   

    public void GenerateField(int junkCount)
    {
        if (junkCount is 0) return;

        foreach (var module in Assembly.Modules)
            foreach (var type in module.Types)
            {
                if (type.IsGlobalModuleType || type.IsRuntimeSpecialName || type.IsSpecialName || type.IsWindowsRuntime || type.IsInterface) continue;

                NameGenerator.Reset();

                var fields = new List<IMemberDef>();
                for (int i = 0; i < junkCount; i++)
                {
                    string name = NameGenerator.GenerateName();

                    var fieldSignature = new FieldSig(module.CorLibTypes.Int32);
                    var field = new FieldDefUser(name, fieldSignature);

                    type.Fields.Add(field);
                    fields.Add(field);
                }
                var args = new MemberGeneratedEventArgs(MemberObjectType.Type, type, MemberObjectType.Field, fields);
                OnMemberGenerated(args);
            }

        NameGenerator.Reset();
    }

}

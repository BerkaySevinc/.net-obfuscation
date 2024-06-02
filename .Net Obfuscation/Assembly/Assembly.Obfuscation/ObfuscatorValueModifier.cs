using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Assembly.Obfuscation;


public class ObfuscatorValueModifier : ValueModifier
{

    public ObfuscatorValueModifier(AssemblyDef assembly) : base(assembly) { }



    public event EventHandler<ValueModifiedEventArgs>? ValueModified;

    protected virtual void OnValueModified(ValueModifiedEventArgs e) =>
        ValueModified?.Invoke(this, e);


    public void EncodeStringValues()
    {
        foreach (var module in Assembly.Modules)
        {
            // Gets needed type imports from dependencies.
            TypeDef? encodingType = GetMatchingTypesFromModuleDependencies(module, typeof(Encoding), true).FirstOrDefault();
            TypeDef? convertType = GetMatchingTypesFromModuleDependencies(module, typeof(Convert), true).FirstOrDefault();

            if (encodingType is null || convertType is null) continue;

            // Gets needed methods from imported types.
            MethodDef methodUTF8Get = encodingType.FindMethod("get_UTF8");

            MethodDef methodFromBase64String =
                convertType.FindMethod(
                    nameof(Convert.FromBase64String),
                    MethodSig.CreateStatic(new SZArraySig(module.CorLibTypes.Byte), module.CorLibTypes.String));

            MethodDef methodGetString =
                encodingType.FindMethod(
                    nameof(Encoding.UTF8.GetString),
                    MethodSig.CreateInstance(module.CorLibTypes.String, new SZArraySig(module.CorLibTypes.Byte)));

            foreach (var type in module.Types)
                foreach (var method in type.Methods)
                {
                    // Continues if method doesn't have body or instructions.
                    if (!method.HasBody || !method.Body.HasInstructions) continue;

                    var body = method.Body;

                    for (int i = 0; i < body.Instructions.Count; i++)
                    {
                        Instruction instruction = body.Instructions[i];

                        // Continues if the instruction is not a string.
                        if (instruction.OpCode != OpCodes.Ldstr) continue;

                        // Gets string value.
                        string? value = instruction.Operand.ToString();

                        if (value is null) continue;

                        // Gets encoded value.
                        string encodedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

                        // Creates decoder instructions.
                        instruction.OpCode = OpCodes.Nop;

                        body.Instructions.Insert(++i,
                            new(
                                OpCodes.Call,
                                module.Import(methodUTF8Get)));

                        body.Instructions.Insert(++i,
                            new(
                                OpCodes.Ldstr,
                                encodedValue));

                        body.Instructions.Insert(++i,
                            new(
                                OpCodes.Call,
                                module.Import(methodFromBase64String)));

                        body.Instructions.Insert(++i,
                            new(
                                OpCodes.Callvirt,
                                module.Import(methodGetString)));

                        var args = new ValueModifiedEventArgs(ValueModifiedObjectType.String, value);
                        OnValueModified(args);
                    }
                }
        }
    }
}

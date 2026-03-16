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


// UNDONE: value encoders just encodes method body local variables, not encoding props, fields etc.?


public class ValueObfuscator : ValueModifier
{

    public ValueObfuscator(AssemblyDef assembly) : base(assembly) { }


    public void EncodeStringValues()
    {
        foreach (var module in Assembly.Modules)
        {
            var methodUTF8Get = module.Import(typeof(Encoding).GetProperty("UTF8")!.GetGetMethod()!);

            var methodFromBase64String = module.Import(
                typeof(Convert).GetMethod(
                    nameof(Convert.FromBase64String),
                    new[] { typeof(string) })!);

            var methodGetString = module.Import(
                typeof(Encoding).GetMethod(
                    nameof(Encoding.GetString),
                    new[] { typeof(byte[]) })!);

            foreach (var method in module.Types.SelectMany(t => t.Methods))
            {
                // Continues if method doesn't have body or instructions.
                if (!method.HasBody || !method.Body.HasInstructions) continue;

                var body = method.Body;
                body.SimplifyBranches();

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

                    var args = new ValueModifiedEventArgs(ValueObjectType.String, value);
                    OnValueModified(args);
                }
            }
        }
    }
}

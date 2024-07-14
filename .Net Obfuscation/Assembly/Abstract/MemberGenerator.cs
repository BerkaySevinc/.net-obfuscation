using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly;


public abstract class MemberGenerator
{
    public AssemblyDef Assembly { get; }
    public NameGenerator NameGenerator { get; set; }


    public MemberGenerator(AssemblyDef assembly, NameGenerator nameGenerator) =>
        (Assembly, NameGenerator) = (assembly, nameGenerator);


    public event EventHandler<MemberGeneratedEventArgs>? MemberGenerated;

    protected virtual void OnMemberGenerated(MemberGeneratedEventArgs e) =>
        MemberGenerated?.Invoke(this, e);
}

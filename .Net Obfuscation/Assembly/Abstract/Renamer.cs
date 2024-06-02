using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly;


public abstract class Renamer
{
    public AssemblyDef Assembly { get; }
    public NameGenerator NameGenerator { get; set; }


    public Renamer(AssemblyDef assembly, NameGenerator nameGenerator)
    {
        Assembly = assembly;
        NameGenerator = nameGenerator;
    }


    public event EventHandler<NameChangedEventArgs>? NameChanged;

    protected virtual void OnNameChanged(NameChangedEventArgs e) =>
        NameChanged?.Invoke(this, e);
}

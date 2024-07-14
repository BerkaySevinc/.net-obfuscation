using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly;


public abstract class NameGenerator
{
    public NameGenerator() { }

    public abstract string GenerateName(IDnlibDef? target);
    public virtual string GenerateName() => GenerateName(null);

    public abstract void Reset();
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly.Obfuscation;


public abstract class NameGenerator
{
    public abstract string GenerateName();

    public abstract void Reset();
}

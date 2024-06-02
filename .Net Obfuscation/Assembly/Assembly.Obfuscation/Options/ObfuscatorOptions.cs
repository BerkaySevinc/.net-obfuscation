using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;


namespace Assembly.Obfuscation;


public class ObfuscatorOptions
{
    public NameGenerator NameGenerator { get; set; } = new ComplexNameGenerator();

    public bool ObfuscateAssemblyName { get; set; } = true;
    public bool ObfuscateModuleNames { get; set; } = true;

    public bool ObfuscateTypeNames { get; set; } = true;
    public bool ObfuscateMethodNames { get; set; } = true;
    public bool ObfuscateFieldNames { get; set; } = true;
    public bool ObfuscatePropertyNames { get; set; } = true;

    public bool ObfuscateParameterNames { get; set; } = true;


    public bool ObfuscateStringValues { get; set; } = true;


}

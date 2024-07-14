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
    public NameGenerator ObfuscatedNameGenerator { get; set; } = new ComplexNameGenerator();
    public NameGenerator JunkNameGenerator { get; set; } = new ComplexNameGenerator();



    public bool ObfuscateAssemblyName { get; set; } = true;
    public bool ObfuscateModuleNames { get; set; } = true;


    public bool ObfuscateTypeNames { get; set; } = true;

    public bool ObfuscateMethodNames { get; set; } = true;

    public bool ObfuscateFieldNames { get; set; } = true;
    public bool ObfuscatePropertyNames { get; set; } = true;
    public bool ObfuscateEventNames { get; set; } = true;

    public bool ObfuscateParameterNames { get; set; } = true;



    public int JunkFieldCount { get; set; } = 100;



    public bool ObfuscateStringValues { get; set; } = true;


}

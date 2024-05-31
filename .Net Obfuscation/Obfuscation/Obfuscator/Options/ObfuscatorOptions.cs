﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;



namespace Obfuscation;


public class ObfuscatorOptions
{
    public NameObfuscationType ObfuscationType { get; set; } = NameObfuscationType.Complex;

    public bool ObfuscateAssemblyName { get; set; } = true;
    public bool ObfuscateModuleNames { get; set; } = true;

    public bool ObfuscateTypeNames { get; set; } = true;
    public bool ObfuscateMethodNames { get; set; } = true;
    public bool ObfuscateFieldNames { get; set; } = true;
    public bool ObfuscatePropertyNames { get; set; } = true;


}

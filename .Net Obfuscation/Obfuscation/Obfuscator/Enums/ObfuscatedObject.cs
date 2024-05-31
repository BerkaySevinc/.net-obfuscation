using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;



namespace Obfuscation;


public enum ObfuscatedObjectType
{
    Assembly,
    Module,

    Type,
    Method,
    Property,
    Field
}

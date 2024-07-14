using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;



namespace Assembly;


public enum MemberObjectType
{
    Assembly,
    Module,
    Type,
    Method,
    Field,
    Property,
    Event,
    Parameter,

    Other
}

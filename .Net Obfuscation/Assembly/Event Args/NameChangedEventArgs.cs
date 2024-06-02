using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly;


public class NameChangedEventArgs : EventArgs
{
    public NameChangedObjectType ObjectType { get; }
    public IDnlibDef? Object { get; }

    public string InitialName { get; }

    public NameChangedEventArgs(NameChangedObjectType objectType, IDnlibDef? @object, string initialName)
    {
        ObjectType = objectType;
        Object = @object;

        InitialName = initialName;
    }
}
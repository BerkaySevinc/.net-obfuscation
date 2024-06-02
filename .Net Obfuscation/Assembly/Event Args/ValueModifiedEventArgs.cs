using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly;


public class ValueModifiedEventArgs : EventArgs
{
    public ValueModifiedObjectType ObjectType { get; }

    public string InitialValue { get; }

    public ValueModifiedEventArgs(ValueModifiedObjectType objectType, string initialValue)
    {
        ObjectType = objectType;

        InitialValue = initialValue;
    }
}
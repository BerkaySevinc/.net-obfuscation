using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib;
using dnlib.DotNet;


namespace Assembly.Obfuscation;


public class ObfuscationCompletedEventArgs : EventArgs
{
    public ObfuscatedObjectType ObfuscatedObjectType { get; }
    public IDnlibDef? ObfuscatedObject { get; }

    public string InitialFullName { get; }
    public string InitialName { get; }

    public ObfuscationCompletedEventArgs(ObfuscatedObjectType obfuscatedObjectType, IDnlibDef? obfuscatedObject, string initialFullName, string initialName)
    {
        ObfuscatedObjectType = obfuscatedObjectType;
        ObfuscatedObject = obfuscatedObject;

        InitialFullName = initialFullName;
        InitialName = initialName;
    }
}
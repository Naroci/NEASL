using System;

namespace NEASL.Base.Linking;

/// <summary>
/// Identifier Attribute to decorate a class to define its name and type within the scripting language.
/// </summary>
[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
public class Signature : Attribute
{
    public LinkType Type { get; private set; }
    public string Name { get; private set; }
    
    public bool IsGeneric { get; private set; }

    public Signature(string identifierName, LinkType type)
    {
        this.Name = identifierName;
        this.Type = type;
    }
    
    public Signature(string identifierName, LinkType type, bool Generic = false)
    {
        this.Name = identifierName;
        this.Type = type;
        this.IsGeneric = Generic;
    }
}

public enum LinkType
{
    Property,
    Method,
    Event
}
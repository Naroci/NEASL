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

    public Signature(string identifierName, LinkType type)
    {
        this.Name = identifierName;
        this.Type = type;
    }
}

public enum LinkType
{
    Property,
    Method,
    Event
}
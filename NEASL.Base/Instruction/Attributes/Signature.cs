using System;

namespace NEASL.Base.Linking;

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
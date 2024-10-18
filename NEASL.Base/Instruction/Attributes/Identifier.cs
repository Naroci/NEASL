namespace NEASL.Base;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public abstract class Identifier : Attribute
{
    public string Name { get; private set; }
    
    public IdentifierType IdentifierType { get; private set; }

    public Identifier(string identifierName, IdentifierType type)
    {
        this.Name = identifierName;
        this.IdentifierType = type;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class Component(string identifierName) : Identifier(identifierName, IdentifierType.Component);


[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class Section(string identifierName) : Identifier(identifierName, IdentifierType.Section);

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class Main(string identifierName) : Identifier(identifierName, IdentifierType.Root);


public enum IdentifierType
{
    Root,
    Component,
    Section
}
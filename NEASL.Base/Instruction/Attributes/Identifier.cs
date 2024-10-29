using System;

namespace NEASL.Base;

// Class Attributes.
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

// Basic Attributes depending on Role (Component, Section and Root)
// Root -> represents the main context of the application and be seen as the standard lib or something.
// Section -> represents a sub content within the Root and could be for example a page / window.
// Component -> represents a single element withing a Section. Could be for example a button that is being pressed on a page / window.
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
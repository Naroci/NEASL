namespace NEASL.Base;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class InstructionIdentifier : Attribute
{
    public string Name { get; }

    public InstructionIdentifier(string identifierName)
    {
        Name = identifierName;
    }
}
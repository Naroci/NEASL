using System.Collections.Generic;
using System.Reflection;

namespace NEASL.Base.Object;

public interface INEASL_Object
{
    public int UniquePtrHash { get; }
    public IdentifierType ObjectType { get; }
    
    List<MethodInfo> Methods { get;}
    
    public string GetNamespace();
    public void SetNamespace(string value);

    public void SetObjectTypeName(string value);
    public string GetObjectTypeName();
    
    object GetVariableValue(string variableName, bool fireFinish = false);
    void SetVariableValue(string variableName, string value);

    void EventCallFinished(string methodname, params object[] args);
}
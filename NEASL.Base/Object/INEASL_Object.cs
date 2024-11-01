using System.Collections.Generic;
using System.Reflection;

namespace NEASL.Base.Object;

public interface INEASL_Object
{
    public int UniquePtrHash { get; }
    public IdentifierType ObjectType { get; }

    MethodInfo FindMethod(string scriptMethodName, object[] args);
    
    List<MethodInfo> Methods { get;}

    public string GetFullName();

    bool IsMember(string method, object[] args);
    
    public string GetName();
    public void SetName(string value);

    public void SetObjectTypeName(string value);
    public string GetObjectTypeName();
    
    object GetVariableValue(string variableName);
    void SetVariableValue(string variableName, string value);
    
    void CompareValues(string valueL, string comparisonIdentifier, string valueR);

    void EventCallFinished(string methodname, params object[] args);
    
    void ReturnEventResult(string methodname, object[] args, object result);
}
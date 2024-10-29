using System.Collections.Generic;
using System.Reflection;

namespace NEASL.Base.Object;

public interface INEASL_Object
{
    public int UniquePtrHash { get; }
    public IdentifierType ObjectType { get; }
    
    List<MethodInfo> Methods { get;}

    public string GetFullName();
    
    public string GetName();
    public void SetName(string value);

    public void SetObjectTypeName(string value);
    public string GetObjectTypeName();
    
    object GetVariableValue(string variableName, bool fireFinish = false);
    void SetVariableValue(string variableName, string value);
    
    void CompareValues(string valueL, string comparisonIdentifier, string valueR);

    void EventCallFinished(string methodname, params object[] args);
    
    void ReturnEventResult(string methodname, object[] args, object result);
}
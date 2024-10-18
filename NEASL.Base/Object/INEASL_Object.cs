using System.Reflection;

namespace NEASL.Base.Object;

public interface INEASL_Object
{
    public string GetNamespace();
    public void SetNamespace(string value);

    public void SetObjectTypeName(string value);
    public string GetObjectTypeName();
    
    object GetVariableValue(string variableName, bool fireFinish = false);
    void SetVariableValue(string variableName, string value);

    List<MethodInfo> Methods { get;}
}
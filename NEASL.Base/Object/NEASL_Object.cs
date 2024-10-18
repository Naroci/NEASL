using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Linking;

namespace NEASL.Base.Object;

public class NEASL_Object : INEASL_Object
{
    /*
     * Creating a Dictionary which holds the variables defined in
     * the script and inside each section of the script.
     * use a seperator ':' in between to define each lifescope
     * SELF as a "this" keyword
     *
     * example:
     * $var1 = "asd"
     * scope:
     *      $var1 = $self.var1
     * :scope
     *
     * then the first key = $var1, the second key = scope:$var1
     */
    private Dictionary<string,object> scriptVariables = null;
    public int UniquePtrHash { get; private set; }
    private List<MethodInfo> m_methods;
    
    public IdentifierType ObjectType { get; private set; }
    private string m_namespaceString;
    private string m_typeName;
    
    public List<MethodInfo> Methods => m_methods;

    public NEASL_Object()
    {
        
        Identifier ident = AttributeHandler.GetAttributeByObject<Identifier>(this);
        if (ident == null)
            throw new CustomAttributeFormatException();
        m_methods = new List<MethodInfo>();
        scriptVariables = new Dictionary<string, object>();
        this.m_typeName = ident.Name;
        this.ObjectType = ident.IdentifierType;
        if (this.GetType().GetMethods().Any(m => m.GetCustomAttribute<Signature>() != null))
        {
            m_methods = this.GetType().GetMethods().Where(m => m.GetCustomAttribute<Signature>() != null)
                .Select(x => x).ToList();
        }
        int uniqueId = RuntimeHelpers.GetHashCode(this);
        this.UniquePtrHash = uniqueId;
    }

    public string GetNamespace()
    {
        return m_namespaceString;
    }

    public void SetNamespace(string value)
    {
        m_namespaceString = value;
    }

    public void SetObjectTypeName(string value)
    {
        m_typeName = value;
    }

    public string GetObjectTypeName()
    {
        return m_typeName;
    }
    
    [Signature(nameof(GetVariableValue), LinkType.Method)]
    public object GetVariableValue(string variableName, bool fire = false)
    {
        object result = null;
        if (scriptVariables.ContainsKey(variableName))
            result = scriptVariables[variableName];
     
        if (fire)
            EventCallFinished(nameof(GetVariableValue), variableName);
        
        return result;
    }

    [Signature(nameof(SetVariableValue), LinkType.Method)]
    public void SetVariableValue(string variableName, string value)
    {
        if (scriptVariables.ContainsKey(variableName))
            scriptVariables[variableName] = value;
        else
        {
            scriptVariables.Add(variableName, value);
        }
        EventCallFinished(nameof(SetVariableValue),variableName, value);
    }
    
    public void EventCallFinished(string methodname, params object[] args)
    {
        Context.GetInstance().GetQueryManager().SendCompleted(m_typeName, methodname,args);
    }
}
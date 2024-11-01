using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Global.Definitions;
using NEASL.Base.Linking;

namespace NEASL.Base.Object;

public class NEASL_Object : INEASL_Object
{
    /*
     * TODO: Namespacing / Creation of seperation between the value and sections -> all variables are public even if created inside a section.
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
    public Dictionary<string,object> scriptVariables = null;
    public int UniquePtrHash { get; private set; }
    private List<MethodInfo> m_methods;
    
    public IdentifierType ObjectType { get; private set; }
    private string m_NameValueString;
    private string m_typeName;
    
    public List<MethodInfo> Methods => m_methods;

    public string GetFullName()
    {
        if (this.ObjectType == IdentifierType.Root)
            return m_typeName;
                
        return $"{m_typeName}({this.GetName()})";
    }
    
    public bool IsMember(string method, object[] args)
    {
        var _method = FindMethod(method, args);
        if (_method != null && _method.GetCustomAttribute<Signature>() != null)
        {
            ParameterInfo[] parameters = _method.GetParameters();
            bool argsValid  = ParametersMatch(parameters, args);
            try
            {
                var isProp = _method.GetCustomAttribute<Signature>().Type == LinkType.Property;
                return isProp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
        }
        return false;
    }
    
    public MethodInfo FindMethod(string scriptMethodName, object[] args)
    {
        if (string.IsNullOrEmpty(scriptMethodName))
            throw new NullReferenceException();
        
        if (Methods == null || !Methods.Any())
            return null;

        if (Methods.Exists(x => x.GetCustomAttribute<Signature>()?.Name.Equals(scriptMethodName) == true))
        {
            var matchingNames = Methods.Where(x => x.GetCustomAttribute<Signature>()?.Name.Equals(scriptMethodName) == true)
                .ToList();
            foreach (var method in matchingNames)
            {
                ParameterInfo[] parameters = method.GetParameters();
                bool argsValid  = ParametersMatch(parameters, args);
                if (argsValid)
                    return method;
            }
        }
        return null;
    }
    
    public bool ParametersMatch(ParameterInfo[] parameters, object[] args)
    {
        if (parameters != null && args == null || args != null && parameters == null) 
            return false;
        
        if (parameters != null && args != null && args.Length  != parameters.Length) 
            return false;
        
        if (parameters == null && args == null 
            || parameters != null && args != null &&parameters.Length == 0 && args.Length == 0)
            return true;

        bool misMatchFound = false;
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType != args[i].GetType())
                misMatchFound = true;
        }
        return !misMatchFound;
    }
    

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

    public string GetName()
    {
        return m_NameValueString;
    }

    public void SetName(string value)
    {
        m_NameValueString = value;
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
    public object GetVariableValue(string variableName)
    {
        object result = null;
        if (scriptVariables.ContainsKey(variableName))
            result = scriptVariables[variableName];
     
        //if (fire)
        //    EventCallFinished(nameof(GetVariableValue), variableName);
        
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

    [Signature(nameof(CompareValues), LinkType.Method)]
    public void CompareValues(string valueL, string comparisonIdentifier, string valueR)
    {
        // Compare and then send the result to the manager to ensure to either
        // Skip or run through all remaining instructions.
        //throw new NotImplementedException();
        Values.Keywords.Comparisons.Comparison comparison = Comparer.GetComparison(comparisonIdentifier);
        bool comparisonResult = Comparer.Compare(valueL, valueR, comparison);
        
        ReturnEventResult(nameof(CompareValues), new[] { valueL,comparisonIdentifier, valueR }, comparisonResult);
    }

    public void EventCallFinished(string methodname, params object[] args)
    {
        Context.GetInstance().GetQueryManager().SendCompleted(GetFullName(), methodname,args);
    }

    public void ReturnEventResult(string methodname, object[] args, object result)
    {
        Context.GetInstance().GetQueryManager().SendCompleted(GetFullName(), methodname,args,result);
    }
}
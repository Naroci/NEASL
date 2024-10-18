using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using NEASL.Base;
using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Linking;

namespace NEASL.TEST_GUI;

public class NEASL_UserControl : UserControl, IBaseLinkedObject
{
    public string NAME { get; private set;  }
    
    Dictionary<string,string> scriptSections = new Dictionary<string,string>();
    private string scriptRawContent = string.Empty;
    private string filePath = string.Empty;
    private string fileName = string.Empty;
    private object m_parent;
    
    public NEASL_UserControl(string scriptContent) : base()
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
        
        
        scriptRawContent = scriptContent;
        LinkToScript();
        
        SelfAssign();
        Context.GetInstance().GetEventManager().Register(this);
        scriptRawContent = scriptContent;
        LinkToScript();
    }

    public NEASL_UserControl() : base()
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
        
        SelfAssign();
        Context.GetInstance().GetEventManager().Register(this);
    }
     
    public Type GetParentType()
    {
        if (m_parent == null)
            return null;
        
        return m_parent.GetType();
    }
    
    public void SelfAssign()
    {
        m_parent = this;
        var attribute = this.GetType().GetCustomAttribute<Identifier>();
        
        // Needed Attribute was not assigned! => Throw exception.
        if (attribute == null)
        {
            throw new TypeInitializationException(this.GetType().FullName, new Exception());
        }
        else
        {
            if (string.IsNullOrEmpty(attribute.Name))
                throw new NullReferenceException();
        }
    }
    
    public string GetUniqueIdentifier()
    {
        return GetObjectTypeName();
    }

    private MethodInfo FindMethod(string scriptMethodName, object[] args)
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

    public void Notify(string method, object[] args)
    {
        if (this.m_parent == null)
        {
            EventCallFinished(method, args);
            return;
        }
        
        var _method = FindMethod(method, args);
        if (_method != null && _method.GetCustomAttribute<Signature>() != null)
        {
            ParameterInfo[] parameters = _method.GetParameters();
            bool argsValid  = ParametersMatch(parameters, args);
            try
            {
                _method.Invoke(this, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine($"Method {method} not found!");
            EventCallFinished(method, args);
            return;
        }
    }

    private bool ParametersMatch(ParameterInfo[] parameters, object[] args)
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
    
    public void AssignScript(string scriptContent)
    {
        scriptRawContent = scriptContent;
        LinkToScript();
    }
    
    public void AssignScript(string FilePath,string FileName)
    {
        scriptRawContent = FetchScript(System.IO.Path.Combine(FilePath, FileName));
        LinkToScript();
    }
   
    public string FetchScript(string scriptFilePath)
    {
        if (!File.Exists(scriptFilePath))
            throw new FileNotFoundException();

        return File.ReadAllText(scriptFilePath);
    }

    public bool LinkToScript()
    {
        if (string.IsNullOrEmpty(scriptRawContent))
            throw new MissingFieldException(this.scriptRawContent);

        var _scriptSections =Linker.LoadSections(this, this.scriptRawContent);
        if (_scriptSections!= null &&_scriptSections.Count > 0)
        {
            this.scriptSections = _scriptSections;
            return true;
        }
        return false;
    }

    public void PerformScriptEvent(string EventIdentifierName)
    {
        if (scriptSections == null || scriptSections != null && scriptSections.Count == 0)
        {
            Console.WriteLine($"[WARN] Requested Event / Method \"{EventIdentifierName}\" not found, Script was empty.");
            return;
        }

        if (!scriptSections.ContainsKey(EventIdentifierName))
        {
            Console.WriteLine($"[WARN] Requested Event / Method \"{EventIdentifierName}\" not found!");
            return;
        }
        if (scriptSections.ContainsKey(EventIdentifierName))
            InstructionRunner.GetInstance().Execute(this,scriptSections[EventIdentifierName]);
    }
    
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

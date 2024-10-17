using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Linking;

namespace NEASL.Base;

public class BaseReceiver : IBaseEventReceiver
{
    private object m_parent;
    private string m_uniqueIdentifier;
    private List<MethodInfo> m_methods;
    
    public BaseReceiver()
    {
        SelfAssign();
        Context.GetInstance().GetEventManager().Register(this);
    }
    
    public void SelfAssign()
    {
        m_methods = new List<MethodInfo>();
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

            this.m_uniqueIdentifier = attribute.Name;
        }

        if (m_parent.GetType().GetMethods().Any(m => m.GetCustomAttribute<Signature>() != null))
        {
            m_methods = m_parent.GetType().GetMethods().Where(m => m.GetCustomAttribute<Signature>() != null)
                .Select(x => x).ToList();
        }
    }
    
    public Type GetParentType()
    {
        if (m_parent == null)
            return null;
        
        return m_parent.GetType();
    }

    public string GetUniqueIdentifier()
    {
        return m_uniqueIdentifier;
    }

    private MethodInfo FindMethod(string scriptMethodName, object[] args)
    {
        if (string.IsNullOrEmpty(scriptMethodName))
            throw new NullReferenceException();
        
        if (this.m_methods == null || !this.m_methods.Any())
            return null;

        if (this.m_methods.Exists(x => x.GetCustomAttribute<Signature>()?.Name.Equals(scriptMethodName) == true))
        {
           var matchingNames = this.m_methods.Where(x => x.GetCustomAttribute<Signature>()?.Name.Equals(scriptMethodName) == true)
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

    public void EventCallFinished(string methodname, params object[] args)
    {
        Context.GetInstance().GetQueryManager().SendCompleted(m_uniqueIdentifier, methodname,args);
    }
}
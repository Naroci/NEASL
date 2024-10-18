using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Linking;
using NEASL.Base.Object;

namespace NEASL.Base;

public class BaseReceiver : NEASL_Object, IBaseEventReceiver
{
    private object m_parent;
    
    
    public BaseReceiver()
    {
        SelfAssign();
        Context.GetInstance().GetEventManager().Register(this);
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
    
    public Type GetParentType()
    {
        if (m_parent == null)
            return null;
        
        return m_parent.GetType();
    }

    public string GetUniqueIdentifier()
    {
        return base.GetObjectTypeName();
    }

    private MethodInfo FindMethod(string scriptMethodName, object[] args)
    {
        if (string.IsNullOrEmpty(scriptMethodName))
            throw new NullReferenceException();
        
        if (base.Methods == null || !base.Methods.Any())
            return null;

        if (base.Methods.Exists(x => x.GetCustomAttribute<Signature>()?.Name.Equals(scriptMethodName) == true))
        {
           var matchingNames = base.Methods.Where(x => x.GetCustomAttribute<Signature>()?.Name.Equals(scriptMethodName) == true)
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

   
}
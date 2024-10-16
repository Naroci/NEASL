using System.Reflection;
using System.Runtime.CompilerServices;

namespace NEASL.Base;

public class BaseReceiver : IBaseEventReceiver
{
    private object m_parent;
    private string m_uniqueIdentifier;
    
    public BaseReceiver()
    {
        SelfAssign();
    }
    
    public void SelfAssign()
    {
        m_parent = this;
        var attribute = this.GetType().GetCustomAttribute<InstructionIdentifier>();
        
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

    public void Notify(string method, object[] args)
    {
        if (this.m_parent == null)
        {
            EventCallFinished(method, args);
            return;
        }
        
        var _method = this.m_parent.GetType().GetMethod(method);
        if (_method != null)
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
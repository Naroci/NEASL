using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Linking;
using NEASL.Base.Object;

namespace NEASL.Base;

/// <summary>
/// In theory nothing else than an Eventlistener which listens to Notifications from the event Manager.
/// Needed to figure when its supposed to execute Instructions given by the QueryManager.
/// </summary>
public class BaseReceiver : NEASL_Object, IBaseEventReceiver
{
    private object m_parent;
    
    public BaseReceiver()
    {
       // SelfAssign();
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
        Context.GetInstance().GetEventManager().Register(this);
    }
    
    public string GetUniqueIdentifier()
    {
        return base.UniquePtrHash.ToString();
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

    

   
}
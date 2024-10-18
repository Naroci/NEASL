using System;

namespace NEASL.Base;

public interface IBaseEventReceiver
{
    Type GetParentType();
    
    string GetUniqueIdentifier();
    
    void Notify(string method, object[] args);
    
    void EventCallFinished(string methodname, object[] args);

    void SelfAssign();

}
using System;
using NEASL.Base.Object;

namespace NEASL.Base;

public interface IBaseEventReceiver: INEASL_Object
{
    Type GetParentType();
    
    string GetUniqueIdentifier();
    
    void Notify(string method, object[] args);
    
    void SelfAssign();

}
using NEASL.Base.Object;

namespace NEASL.Base;

public class Reference
{
    public static object GetMembersValue(INEASL_Object sender, string fullReferenceName)
    {
        if (sender != null || string.IsNullOrEmpty(fullReferenceName))
            throw new NullReferenceException();
        
        
        
        return null;
    }
    
    public static object GetMethodsResult(INEASL_Object sender,string fullReferenceName)
    {
        return null;
    }


    public INEASL_Object ResolveReferenceName(string FullReferenceName)
    {
        return null;
    }

}
using NEASL.Base.AppContext;
using NEASL.Base.AppContext.Interfaces;
using NEASL.Base.Package;

namespace NEASL.Base;

public class NEASL
{
    public static IBaseApplicationContext Application() => applicationContext;
    
    private static IBaseApplicationContext applicationContext;
    public static IBaseApplicationContext Initialize()
    {
        if (applicationContext == null)
            applicationContext = new BaseApplicationContext();
        return applicationContext;
    }
    
    public static T Initialize<T>() where T : BaseApplicationContext
    {
        object obj = null;
        obj = Activator.CreateInstance(typeof(T));
        if (obj == null || obj != null &&  obj is not IBaseApplicationContext)
        {
            applicationContext = null;
            return null;
        }
   
        applicationContext = obj as IBaseApplicationContext;
        return applicationContext as T;
    }

   
}
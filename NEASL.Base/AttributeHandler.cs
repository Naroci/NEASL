using System.Reflection;

namespace NEASL.Base;

public static class AttributeHandler
{
    public static T GetAttributeByObject<T>(Object obj) where T : Attribute
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        
        var attribute = obj.GetType().GetCustomAttribute<T>();
        return attribute;
    }

    public static List<MethodInfo> GetMethodsByAttributeType<T>(Object obj) where T : Attribute
    {
        if (obj == null) 
            throw new ArgumentNullException(nameof(obj));
        
        var methods = obj.GetType().GetMethods();
        
        if (methods == null ||methods != null && methods.Length == 0)
        {
            return null;
        }

        bool found = methods.ToList().Exists(x => x.GetCustomAttribute<T>() != null);
        if (found)
        {
            var result = methods.ToList().Where(x => x.GetCustomAttribute<T>() != null).Select(y=>y).ToList();
            return result;
        }
        return null;
    }
    
    /*
     *   Property,
         Method,
         Event
     */

    public static List<T> GetAttributesByAttributeTypeFromObjectsFields<T>(Object obj) where T : Attribute
    {
        List<T> attributes = new List<T>();
        
        if (obj == null) 
            throw new ArgumentNullException(nameof(obj));
        
        var methods = obj.GetType().GetMethods();
        
        if (methods == null ||methods != null && methods.Length == 0)
        {
            return null;
        }

        bool found = methods.ToList().Exists(x => x.GetCustomAttribute<T>() != null);
        if (found)
        {
            var methodAttributes = methods.ToList().Where(x => x.GetCustomAttribute<T>() != null).Select(y=>y.GetCustomAttribute<T>()).ToList();
            attributes.AddRange(methodAttributes);
        }
        
        var props = obj.GetType().GetProperties();
        bool foundInProps = props.ToList().Exists(x => x.GetCustomAttribute<T>() != null);
        if (foundInProps)
        {
            var propAttributes = props.ToList().Where(x => x.GetCustomAttribute<T>() != null).Select(y=>y.GetCustomAttribute<T>()).ToList();
            attributes.AddRange(propAttributes);
        }

        return attributes;
    }
   
}
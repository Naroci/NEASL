using System;
using System.Collections.Generic;
using System.Linq;

namespace NEASL.Base.Assembly;

public static class AssemblyHelper
{
    private static List<Type> GetAssembliesOfSubclassType<T>()
    {
        List<Type> resultList = new List<Type>();
        
        // Get a list of all current loaded Assemblies.
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        // Not needed but just to make sure.
        if (loadedAssemblies != null && loadedAssemblies.Length > 0)
        {
            foreach (var assembly in loadedAssemblies)
            {
                // Check if any of the given Types has the Subclass that we are looking for.
                if (assembly.GetTypes().Any(t => t.IsSubclassOf(typeof(T))))
                {
                    //fetch and add them to the return list.
                    var results = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(T))).ToList();
                    if (results != null && results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            resultList.Add(result);
                            Console.WriteLine($"Found \"{result.Name}\" Type");
                        }
                        Console.WriteLine($"Found {results.Count} assemblies");
                    }
                }
            }
        }
        return resultList;
    }
}
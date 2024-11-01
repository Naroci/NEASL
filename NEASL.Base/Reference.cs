using NEASL.Base.Object;

namespace NEASL.Base;

public class ReferenceResolver
{
    private static ReferenceResolver instance ;
    private InstructionReader instructionReader;

    public ReferenceResolver()
    {
        instructionReader = new InstructionReader();
    }
    public static ReferenceResolver  GetInstance(){
        if (instance == null)
            instance = new ReferenceResolver();

        return instance;
    }
    
    
    /// <summary>
    /// Resolves the referenced variable values and sets the according values.
    /// </summary>
    /// <param name="source">The source of the Value.</param>
    /// <param name="args">The name of the variables to resolve.</param>
    /// <param name="isAssigment">is the current request an assignment of a value?, important so that the left side does not get written (ex. "a=2")</param>
    /// <returns>object[]</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task <object[]> ResolveReferencedVariables(INEASL_Object source, object[] args,bool isAssigment = false)
    {
        if (source == null)
            throw new ArgumentException(nameof(source));
        
        if (args == null || args.Length == 0)
            return args;
        
        for (int i = 0; i < args.Length; i++)
        {
            if (isAssigment && i == 0)
                continue;

            if (instructionReader.IsMethod((string)args[i]) && !instructionReader.IsLocalMethod((string)args[i]))
            {
                var referencedValueResult = await instructionReader.ResolveReference((string)args[i]);
                args[i] = referencedValueResult;
            }
            else
            {
                var value = source.GetVariableValue((string)args[i]);
                if (value != null)
                {
                    args[i] = value;
                }
            }
       
        }
        return args;
    }



}
namespace NEASL.Base;

public class Instruction
{
    public Instruction()
    {
        Id = (long)DateTime.UtcNow.TimeOfDay.TotalNanoseconds;
    }

    // Identifier / Name of the Target Class name (prob. Identifying via a class attribute might be the best)
    public string BaseName;
    
    // Arguments given for executing a Method.
    public object[] Arguments;
    
    // Target name of the Method to call.
    public string MethodName;
    
    // Unique Identifier / Sorting value.
    public long Id { get; private set; }
    
    // If the Instruction is completed and can be removed from the stack.
    public bool Completed { get; set; }
}
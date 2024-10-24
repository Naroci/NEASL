using System;
using NEASL.Base.Object;

namespace NEASL.Base;

public class Instruction
{
    public Instruction()
    {
        Id = (long)DateTime.UtcNow.TimeOfDay.TotalNanoseconds;
    }
    
    // From which context the instruction has been calle.
    public INEASL_Object Sender { get; set; }

    // Defines the point of where the other part of the SubSection Ends or Starts 
    // depending on what the current role of the curren Instruction is.
    public Instruction EntryLeavePoint { get; set; }

    // Identifier / Name of the Target Class name (prob. Identifying via a class attribute might be the best)
    public string BaseName;
    
    // Arguments given for executing a Method.
    public object[] Arguments;
    
    // Target name of the Method to call.
    public string MethodName;

    // If the current Instruction is Assigning a value to another.
    public bool IsAssignment { get; set; }

    public bool IsCondition { get; set; }
    
    public bool IsLoop { get; set; }

    public bool IsSubSectionEntry { get; set; }
    
    public bool IsSubSectionLeave { get; set; }

    // Unique Identifier / Sorting value.
    public long Id { get; private set; }
    
    // If the Instruction is completed and can be removed from the stack.
    public bool Completed { get; set; }
}
using NEASL.Base.Object;

namespace NEASL.Base;

public interface IInstructionReader
{
    Instruction getInstruction(INEASL_Object source, string line);

    bool IsMethod(string line);

    bool IsLocalMethod(string line);
    
    string ResolveSelfRefencedMethod(INEASL_Object sender,string line);

    bool IsAssignment(string line);

    string[] GetAssignmentValues(string line);
}
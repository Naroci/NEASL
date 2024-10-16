namespace NEASL.Base;

public interface IInstructionReader
{
    Instruction getInstruction(string line);
}
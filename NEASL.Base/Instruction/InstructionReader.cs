using FormatException = System.FormatException;

namespace NEASL.Base;

public class InstructionReader : IInstructionReader
{
    public Instruction getInstruction(string line)
    {
        if (string.IsNullOrEmpty(line))
            return null;

        Instruction returnValue = new Instruction();

        if (line.IndexOf("->") > -1)
        {
            var splittedParts = line.Split("->");
            if (splittedParts.Length > 0)
            {
                returnValue.BaseName = splittedParts[0];
                if (splittedParts.Length > 1)
                {
                    var methodResult =tryParseMethod(splittedParts[1]);
                    if (methodResult != null)
                    {
                        returnValue.MethodName = methodResult.Item1;
                        returnValue.Arguments = methodResult.Item2;
                    }
                }
            }
        }
        
        return returnValue;
    }

    Tuple<string, string[]> tryParseMethod(string methodPart)
    {
        string[] args = null;
        string MethodName = null;
        int argsStart = methodPart.IndexOf("(");
        int argsEnd = methodPart.IndexOf(")");
        if (argsStart > -1 && argsEnd > 0)
        {
            MethodName = methodPart.Substring(0, argsStart);
            if (string.IsNullOrEmpty(MethodName))
                throw new FormatException("Invalid method name");
            
            string argsPart = methodPart.Substring(argsStart + 1, argsEnd - argsStart - 1);
            if (argsPart.IndexOf(",") > -1)
                args = argsPart.Split(",");
            else
                args = new[] { argsPart };
        }
        return new Tuple<string, string[]>(MethodName, args);
    }
}
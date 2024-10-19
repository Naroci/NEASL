using System;
using NEASL.Base.Object;
using ArithmeticException = System.ArithmeticException;
using FormatException = System.FormatException;

namespace NEASL.Base;

public class InstructionReader : IInstructionReader
{
    public Instruction getInstruction(INEASL_Object source, string line)
    {
        if (line == null)
            return null;
        
        line = line.TrimStart();
        if (string.IsNullOrEmpty(line))
            return null;
        
        Instruction returnValue = new Instruction();
        returnValue.IsAssignment = IsAssignment(line);
        returnValue.Sender = source;
        if (returnValue.IsAssignment)
        {
            var avalues = GetAssignmentValues(line);
            if (IsMethod(avalues[0]))
                throw new ArithmeticException($"The Method {avalues[0]} cannot be assigned to another value.");
            
            if (avalues.Length > 2)
                throw new ArithmeticException($"Assignment found in line {line}");
            
            line = GetAssignmentCommand(source,avalues[0],avalues[1]);
        }
        
       
        if (IsLocalMethod(line)) {
            line = ResolveSelfRefencedMethod(source, line);
        }
        
        if (line.IndexOf("->") > -1)
        {
            var splittedParts = line.Split("->");
            if (splittedParts.Length > 0)
            {
                returnValue.BaseName = splittedParts[0].Trim();
                if (splittedParts.Length > 1)
                {
                    var methodResult = tryParseMethod(splittedParts[1]);
                    if (methodResult != null)
                    {
                        returnValue.MethodName = methodResult.Item1.Trim();
                        returnValue.Arguments = methodResult.Item2;
                    }
                }
            }
        }

        return returnValue;
    }

    public static object[] ResolveReferencedVariables(INEASL_Object source, object[] args,bool isAssigment = false)
    {
        if (source == null)
            throw new ArgumentException(nameof(source));
        if (args == null || args.Length == 0)
            return args;
        
        for (int i = 0; i < args.Length; i++)
        {
            if (isAssigment && i == 0)
                    continue;
            
            var value = source.GetVariableValue((string)args[i], false);
            if (value != null)
            {
                args[i] = value;
            }
        }
        return args;
    }

    /// <summary>
    /// Checks if the current line is a Variable assignment.
    /// A = B
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public bool IsAssignment(string line)
    {
        return line.IndexOf("=") > -1;
    }

    public string[] GetAssignmentValues(string line)
    {
        string[] vals = line.Split("=");
        for (int i = 0; i < vals.Length - 1; i++)
        {
            vals[i] = vals[i].Trim();
        }
        return vals;
    }

    public string GetAssignmentCommand(INEASL_Object sender,string varName, object value)
    {
        string line = $"{sender.GetObjectTypeName()}->{nameof(sender.SetVariableValue)} ({varName},{value})";
        return line.Trim();
    }

    
    /// <summary>
    /// Checks the current line for the case of () inside of it.
    /// </summary>
    /// <param name="line">the actual line.</param>
    /// <returns>FALSE || TRUE</returns>
    public bool IsMethod(string line)
    {
        return line.IndexOf("(") > 0 && line.IndexOf(")") > 0;
    }

    public bool IsLocalMethod(string line)
    {
        return IsMethod(line) && line.IndexOf("->") < 0;
    }

    public string ResolveSelfRefencedMethod(INEASL_Object sender, string line)
    {
        if (sender is null || string.IsNullOrEmpty(line))
            throw new ArgumentNullException(nameof(line));
        
        return $"{sender.GetObjectTypeName()}->{line}";
    }

    Tuple<string, object[]> tryParseMethod(string methodPart)
    {
        object[] args = null;
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

            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = ((string)args[i]).Trim();
                }
            }
        }
        return new Tuple<string, object[]>(MethodName, args);
    }
}
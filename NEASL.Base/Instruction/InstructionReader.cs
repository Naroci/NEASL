using System;
using NEASL.Base.Object;
using NEASL.Base.Global.Definitions;
using ArithmeticException = System.ArithmeticException;
using FormatException = System.FormatException;

namespace NEASL.Base;

public class InstructionReader : IInstructionReader
{
    /// <summary>
    /// Parses a instruction by the given source INEASL_Object and the given line thats supposed to be
    /// executed.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    /// <exception cref="ArithmeticException"></exception>
    public Instruction getInstruction(INEASL_Object source, string line)
    {
        if (line == null)
            return null;
        
        line = line.TrimStart();
        if (string.IsNullOrEmpty(line))
            return null;
        
        Instruction returnValue = new Instruction();
        returnValue.IsCondition = IsCondition(line);
        if (returnValue.IsCondition)
        {
            returnValue.Arguments = GetConditionValues(line);
        }

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
        
        // check if the identifier "->" Exists.
        if (line.IndexOf(Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER) > -1)
        {
            var splittedParts = line.Split(Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER);
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

    /// <summary>
    /// Resolves the referenced variable values and sets the according values.
    /// </summary>
    /// <param name="source">The source of the Value.</param>
    /// <param name="args">The name of the variables to resolve.</param>
    /// <param name="isAssigment">is the current request an assignment of a value?, important so that the left side does not get written (ex. "a=2")</param>
    /// <returns>object[]</returns>
    /// <exception cref="ArgumentException"></exception>
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
        return line.IndexOf(Values.Keywords.Comparisons.EQUALS_KEYWORD) > -1 
               && line.IndexOf(Values.Keywords.Identifier.CONDITION_CONDITION_END_IDENTIFIER) == -1
               && line.IndexOf(Values.Keywords.Identifier.LOOP_CONDITION_END_IDENTIFIER) == -1;
    }

    /// <summary>
    /// Gets the value from the right side of a "="
    /// </summary>
    /// <param name="line">the current assignment instruction (ex "a = 2")</param>
    /// <returns>string[] (ex. {"a" , "2"})</returns>
    public string[] GetAssignmentValues(string line)
    {
        string[] vals = line.Split(Values.Keywords.Comparisons.EQUALS_KEYWORD);
        for (int i = 0; i < vals.Length - 1; i++)
        {
            vals[i] = vals[i].Trim();
        }
        return vals;
    }
    
    
    /// <summary>
    /// Gets the value from the right side of a "="
    /// </summary>
    /// <param name="line">the current condition instruction (ex  IF(a = 2): )</param>
    /// <returns>string[] (ex. {"a" , "2"})</returns>
    public string[] GetConditionValues(string line)
    {
        string[] vals = line.Split();
        for (int i = 0; i < vals.Length - 1; i++)
        {
            vals[i] = vals[i].Trim();
        }
        return vals;
    }
    
    public string GetComparisonCommand(INEASL_Object sender,string varName, object value)
    {
        string line = $"{sender.GetObjectTypeName()}{Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER}{nameof(sender.SetVariableValue)} ({varName},{value})";
        return line.Trim();
    }

    /// <summary>
    /// Returns the full path / method to the targeted section to set a certain
    /// variable in its context.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="varName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public string GetAssignmentCommand(INEASL_Object sender,string varName, object value)
    {
        string line = $"{sender.GetObjectTypeName()}{Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER}{nameof(sender.SetVariableValue)} ({varName},{value})";
        return line.Trim();
    }
    
    /*
     * NOTE!!! :
     * Instruction / Line specific Checks should rather be moved to the Instruction itself.
     * Therefore, the class can check its own content and give feedback to the InstructionReader when requested.
     * (ex. Instruction.IsMethod() or Instruction.IsLocalMethod() etc.)
     * 
     * Because the Instruction has already the context of the current line.
     */
    
    /// <summary>
    /// Checks the current line for the case of () inside of it.
    /// Also checks that there are no (): values in line which would cause
    /// confusing with loops(UNTIL/WHILE) and conditions (IF/ELSE)
    /// </summary>
    /// <param name="line">the actual line.</param>
    /// <returns>FALSE || TRUE</returns>
    public bool IsMethod(string line)
    {
        bool isMethod = line.IndexOf(Values.Keywords.Identifier.METHOD_START_IDENTIFIER) > 0 
                        && line.IndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER) > 0 
                        && line.IndexOf(Values.Keywords.Identifier.CONDITION_CONDITION_END_IDENTIFIER) == -1
                        && line.IndexOf(Values.Keywords.Identifier.LOOP_CONDITION_END_IDENTIFIER) == -1;
        return isMethod;
    }

    public bool IsCondition(string line)
    {
        line = line.Trim();
        bool isCondition = line.IndexOf(Values.Keywords.Identifier.CONDITION_CONDITION_START_IDENTIFIER) > 0 
                        && line.IndexOf(Values.Keywords.Identifier.CONDITION_CONDITION_END_IDENTIFIER) > 1
                        && line.IndexOf(Values.Keywords.Conditions.IF_KEYWORD) > -1 ||line.IndexOf(Values.Keywords.Conditions.ELSE_KEYWORD) > -1 ;
        return isCondition;
    }

    /// <summary>
    /// Checks if the method call refers to a different object / class or to a Method inside it the current context.
    /// </summary>
    /// <param name="line">the line instruction that is about to be executed.</param>
    /// <returns>FALSE || TRUE</returns>
    public bool IsLocalMethod(string line)
    {
        // Check if the current line is a Method and if it refers to a different object / class
        return IsMethod(line) && line.IndexOf(Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER) < 0;
    }

    /// <summary>
    /// Resolves a self referenced method by reurning a full instruction line, so that
    /// it can globally be run and be requested from the instruction reader.
    /// </summary>
    /// <param name="sender">Sender / Source of the Instruction / Method</param>
    /// <param name="line">Instruction / Method to execute.</param>
    /// <returns>a Full referenced instruction (ex. "BUTTON->PRESSED()")</returns>
    /// <exception cref="ArgumentNullException">is being thrown if the sender or line is null or empty.</exception>
    public string ResolveSelfRefencedMethod(INEASL_Object sender, string line)
    {
        if (sender is null || string.IsNullOrEmpty(line))
            throw new ArgumentNullException(nameof(line));
        
        return $"{sender.GetObjectTypeName()}{Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER}{line}";
    }

    /// <summary>
    /// Tries to parse a method by the given string and to initalize its args.
    /// ex. WRITE_LINE("Hello World")
    /// </summary>
    /// <param name="methodPart">The string of the Instruction/Method to call (ex. "WRITE_LINE("Hello World")")</param>
    /// <returns>name of the method (if found), and the given arguments (if found and parsed)</returns>
    /// <exception cref="FormatException">is being thrown if the method name is invalid.</exception>
    Tuple<string, object[]> tryParseMethod(string methodPart)
    {
        object[] args = null;
        string MethodName = null;
        int argsStart = methodPart.IndexOf(Values.Keywords.Identifier.METHOD_START_IDENTIFIER);
        int argsEnd = methodPart.IndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER);
        if (argsStart > -1 && argsEnd > 0)
        {
            MethodName = methodPart.Substring(0, argsStart);
            if (string.IsNullOrEmpty(MethodName))
                throw new FormatException("Invalid method name");
            
            string argsPart = methodPart.Substring(argsStart + 1, argsEnd - argsStart - 1);
            if (argsPart.IndexOf(Values.Keywords.Identifier.ARGUMENT_SEPARATOR_IDENTIFIER) > -1)
                args = argsPart.Split(Values.Keywords.Identifier.ARGUMENT_SEPARATOR_IDENTIFIER);
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
using System;
using System.Reflection;
using System.Text.RegularExpressions;
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

        string orignLine = new string(line);
        line = line.TrimStart();
        if (string.IsNullOrEmpty(line))
            return null;

        Instruction returnValue = new Instruction();
        bool isConditionEntry = IsConditionEntryPoint(line);
        if (isConditionEntry)
        {
            returnValue.IsSubSectionEntry = isConditionEntry;

            returnValue.IsCondition = isConditionEntry;
            returnValue.Arguments = GetConditionValues(line);
            if (returnValue.Arguments != null && returnValue.Arguments.Length > 1)
            {
                line = GetComparisonCommand(source, returnValue.Arguments[0], (string)returnValue.Arguments[1],
                    returnValue.Arguments[2]);
            }
        }

        bool isConditionLeaveEntry = IsConditionLeavePoint(line);
        if (isConditionLeaveEntry)
        {
            returnValue.IsCondition = isConditionLeaveEntry;
            returnValue.IsSubSectionLeave = isConditionLeaveEntry;
        }

        if (returnValue.IsCondition)
            returnValue.ConditionMethod = GetConditionMethodName(orignLine);

        returnValue.IsAssignment = IsAssignment(line) && !returnValue.IsCondition;
        returnValue.Sender = source;
        if (returnValue.IsAssignment)
        {
            var avalues = GetAssignmentValues(line);
            if (IsMethod(avalues[0]))
                throw new ArithmeticException($"The Method {avalues[0]} cannot be assigned to another value.");

            if (avalues.Length > 2)
                throw new ArithmeticException($"Assignment found in line {line}");

            line = GetAssignmentCommand(source, avalues[0], avalues[1]);
        }

        if (IsLocalMethod(line))
        {
            line = ResolveSelfRefencedMethod(source, line);
        }

        // check if the identifier "->" Exists.
        if (line.IndexOf(Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER) > -1)
        {
            var assignPos = line.IndexOf(Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER);
            var splittedParts = line.Substring(assignPos + Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER.Length,
                line.Length - (assignPos + Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER.Length));

            // Value / Object that is refered to.
            // Make sure to differ between Type and references.
            var refName = line.Substring(0, assignPos);
            if (splittedParts.Length > 0 && !string.IsNullOrEmpty(refName))
            {
                returnValue.ObjectName = refName;

                if (splittedParts.Length > 1)
                {
                    var methodResult = tryParseMethod(splittedParts);
                    if (methodResult != null && methodResult.Item1 != null)
                    {
                        returnValue.MethodName = methodResult.Item1.Trim();
                        returnValue.Arguments = methodResult.Item2;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        return returnValue;
    }


    public async Task<object> ResolveReference(string line)
    {
        INEASL_Object source = null;
        string[] sorted = line.Split(Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER);
        if (sorted != null && sorted.Length > 0)
        {
            var eventReceiver = Context.GetInstance().GetEventManager().FindReceiver(sorted[0]);
            source = eventReceiver;
        }

        if (source != null)
        {
            Instruction returnValue = getInstruction(source, line);
            var member = source.IsMember(returnValue.MethodName, returnValue.Arguments);
            MethodInfo methodInfo = source.FindMethod(returnValue.MethodName, returnValue.Arguments);
            if (methodInfo != null)
            {
                object result = await Task.Run(() => { return methodInfo.Invoke(source, returnValue.Arguments); });

                return result;
            }
        }

        return null;
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
               && line.IndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER) == -1
               && line.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER) == -1
               || line.IndexOf(Values.Keywords.Comparisons.EQUALS_KEYWORD) > -1
               && line.IndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER) > -1
               && line.IndexOf(Values.Keywords.Comparisons.EQUALS_KEYWORD) <
               line.IndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER)
               && line.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER) == -1;
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


    public string GetConditionMethodName(string line)
    {
        if (string.IsNullOrEmpty(line))
            return null;

        if (line.Contains(Values.Keywords.Conditions.IF_KEYWORD))
            return Values.Keywords.Conditions.IF_KEYWORD;
        else if (line.Contains(Values.Keywords.Conditions.ELSE_KEYWORD))
            return Values.Keywords.Conditions.ELSE_KEYWORD;

        return null;
    }

    /*
     * TODO: Change the method later to take in the arguments for what it should parse loop OR condition
     */

    /// <summary>
    /// Gets the value from the right side of a "="
    /// </summary>
    /// <param name="line">the current condition instruction (ex  IF(a = 2): )</param>
    /// <returns>string[] (ex. {"a" , "2"})</returns>
    public string[] GetConditionValues(string line)
    {
        // TODO:
        // Super ugly... but honestly too lazy to loop through it and for what it does it should be fine
        // at least for first.... 

        bool isElse = line.Contains(Values.Keywords.Conditions.ELSE_KEYWORD);
        line = line.TrimStart();
        int IFKeywordIndex = line.IndexOf(Values.Keywords.Conditions.IF_KEYWORD);
        if (IFKeywordIndex == 0)
        {
            line = line.Remove(0, IFKeywordIndex + Values.Keywords.Conditions.IF_KEYWORD.Length);
            line = line.TrimStart();

            int condStartIndex = line.IndexOf(Values.Keywords.Identifier.CONDITION_CONDITION_START_IDENTIFIER);
            if (condStartIndex == 0)
            {
                line = line.Remove(0,
                    condStartIndex + Values.Keywords.Identifier.CONDITION_CONDITION_START_IDENTIFIER.Length);
                line = line.TrimStart();

                int closingIndexLog = line.LastIndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER);
                if (closingIndexLog > 0)
                {
                    line = line.Substring(0, closingIndexLog);
                    int closingIndex = line.LastIndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER);
                    if (closingIndex > 0 && closingIndexLog > 0)
                    {
                        line = line.Substring(0, closingIndex);
                    }
                }
            }
        }

        if (!isElse)
        {
            string comparisonValue = Comparer.FindComparisonKeyword(line);
            string[] result = Regex.Split(line, $"({comparisonValue})");
            for (int i = 0; i < result.Length - 1; i++)
            {
                result[i] = result[i].Trim();
            }

            return result;
        }

        return new string[0];
    }


    // Creates a comparison method that can be executed.
    public string GetComparisonCommand(INEASL_Object sender, object valueL, string comparisonString, object valueR)
    {
        string line =
            $"{sender.GetFullName()}{Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER}{nameof(sender.CompareValues)} ({valueL},{comparisonString},{valueR})";
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
    public string GetAssignmentCommand(INEASL_Object sender, string varName, object value)
    {
        string line =
            $"{sender.GetFullName()}{Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER}{nameof(sender.SetVariableValue)} ({varName},{value})";
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
                        && line.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER) == -1;
        return isMethod;
    }

    public bool IsConditionEntryPoint(string line)
    {
        line = line.Trim();
        bool isCondition = line.IndexOf(Values.Keywords.Identifier.CONDITION_CONDITION_START_IDENTIFIER) > 0
                           && line.IndexOf(Values.Keywords.Identifier.METHOD_START_IDENTIFIER) > 1
                           && line.IndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER) > 1
                           && line.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER) > 1
                           && line.IndexOf(Values.Keywords.Conditions.IF_KEYWORD) > -1 ||
                           (line.IndexOf(Values.Keywords.Conditions.ELSE_KEYWORD) > -1
                            && line.IndexOf(Values.Keywords.Identifier.SECTION_START_IDENTIFIER) > line.IndexOf(Values.Keywords.Conditions.ELSE_KEYWORD));
        return isCondition;
    }

    public bool IsConditionLeavePoint(string line)
    {
        line = line.Trim();
        bool isCondition = line.IndexOf(Values.Keywords.Identifier.SECTION_END_IDENTIFIER) == 0
                           && (line.IndexOf(Values.Keywords.Conditions.IF_KEYWORD) > 0 ||
                               line.IndexOf(Values.Keywords.Conditions.ELSE_KEYWORD) > 0);
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
        string refMethod = $"{sender.GetFullName()}{Values.Keywords.Identifier.CLASS_SUBMETHOD_IDENTIFIER}{line}";
        return refMethod;
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
        int argsEnd = methodPart.LastIndexOf(Values.Keywords.Identifier.METHOD_END_IDENTIFIER);
        if (argsStart > -1 && argsEnd > 0)
        {
            MethodName = methodPart.Substring(0, argsStart);
            if (string.IsNullOrEmpty(MethodName))
                throw new FormatException("Invalid method name");

            string argsPart = methodPart.Substring(argsStart + 1, argsEnd - argsStart - 1);
            if (argsPart.IndexOf(Values.Keywords.Identifier.ARGUMENT_SEPARATOR_IDENTIFIER) > -1)
                args = argsPart.Split(Values.Keywords.Identifier.ARGUMENT_SEPARATOR_IDENTIFIER);
            else
                args = !string.IsNullOrEmpty(argsPart) ? new[] { argsPart } : new object[] { };

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
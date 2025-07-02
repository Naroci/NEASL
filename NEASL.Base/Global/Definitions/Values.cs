namespace NEASL.Base.Global.Definitions;


// Defining the basic keywords used inside the scripts to identify
// each parts.
public struct Values
{
    public struct Keywords
    {
        public struct Booleans
        {
            // Keyword for a TRUE boolean
            public const string TRUE_KEYWORD = "TRUE";
            
            // Keyword for a FALSE boolean.
            public const string FALSE_KEYWORD = "FALSE";
        }

        public struct Conditions
        {
            // Keyword for an IF condition.
            public const string IF_KEYWORD = "IF";
            
            // Keyword for an ELSE condition.
            public const string ELSE_KEYWORD = "ELSE";
        }
        
        public struct Loops
        {
            // Basically somewhat of a for / foreach loop. But instead just for a fixed amount of 
            // repetitions. So repeat the same command 10x or something like that.
            public const string UNTIL_KEYWORD = "UNTIL";
            
            // While loop. Just cheching if a given boolean is true.
            public const string WHILE_KEYWORD = "WHILE";
        }
        
        public struct Identifier
        {
            // Identifier to go down in a tree to point from the class to its Method
            // ex. APP->WRITE_LINE()
            public const string CLASS_SUBMETHOD_IDENTIFIER = "->";
            
            // Identifier to identify a value as a string.
            // ex. message = "Hello World!"
            public const string STRING_IDENTIFIER = "\"";
            
            // Identifier from where to start a section or event
            // ex. PRESSED:, BUTTON:
            public const string SECTION_START_IDENTIFIER = ":";
            
            // Identifier from where to end a section or event
            // ex. :BUTTON, :PRESSED
            public const string SECTION_END_IDENTIFIER = ":";
            
            // Identifier to identify a Method.
            // ex. WRITE_LINE()
            public const string METHOD_START_IDENTIFIER = "(";
            
            // Identifier to identify a Method.
            // ex. WRITE_LINE()
            public const string METHOD_END_IDENTIFIER = ")";
            
            // Identifier to identify the condition start of a loop.
            // ex. UNTIL( ....
            public const string LOOP_CONDITION_START_IDENTIFIER = "(";
            
            
            // Identifier to identify the condition start of a condition.
            // ex. IF( ....
            public const string CONDITION_CONDITION_START_IDENTIFIER = "(";
            
     

            
            public const string ARGUMENT_SEPARATOR_IDENTIFIER = ",";
        }
        
        public struct Comparisons
        {
            // For equal comparisons or to assign a value.
            public const string EQUALS_KEYWORD = "=";
            
            // Comparing if a value is not equal.
            public const string NOT_EQUAL_KEYWORD = "!=";
            
            // Comparing if a value is smaller than another.
            public const string SMALLER_THAN_KEYWORD = "<";
            
            // Comparing if a value is bigger than another.
            public const string BIGGER_THAN_KEYWORD = ">";
            
            // And keyword for when more than a single condition should be true.
            public const string AND_KEYWORD = "AND";
            
            // And keyword for when either one or the other should be true.
            public const string OR_KEYWORD = "OR";
            
            public enum Comparison
            {
                UNDEFINED,
                EQUAL,
                NOT_EQUAL,
                SMALLER_THAN,
                BIGGER_THAN,
                AND,
                OR
            }
        }

        public struct MathematicalOperators
        {
            // Plus calculation keyword.
            public const string PLUS_KEYWORD = "+";
            
            // Minus calculation keyword.
            public const string MINUS_KEYWORD = "-";
            
            // Division calculation keyword.
            public const string DIVIDE_KEYWORD = "/";
            
            // Multiplication calculation keyword.
            public const string MULTIPLY_KEYWORD = "*";
        }
    }
}
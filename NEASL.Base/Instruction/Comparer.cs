using NEASL.Base.Global.Definitions;

namespace NEASL.Base;

/// <summary>
/// Simple static class to compare two values to each other.
/// trying to keep everything simple as possible ... therefore this should be enough for first.
/// </summary>
public static class Comparer
{
    
    public static bool Compare(object obj1, object obj2, Values.Keywords.Comparisons.Comparison ComparisonType)
    {
        switch (ComparisonType)
        {
            case Values.Keywords.Comparisons.Comparison.EQUAL:
                return obj1.Equals(obj2);
            case Values.Keywords.Comparisons.Comparison.NOT_EQUAL:
                return !obj1.Equals(obj2);
        }
        return false;
    }

    // Returns the Comparison based on the given input string and matches it with the Keyword values.
    public static Values.Keywords.Comparisons.Comparison GetComparison(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            throw new ArgumentNullException();
        
        inputValue = inputValue.Trim();
        switch (inputValue)
        {
            case Values.Keywords.Comparisons.OR_KEYWORD:
                return Values.Keywords.Comparisons.Comparison.OR;
            case Values.Keywords.Comparisons.AND_KEYWORD:
                return Values.Keywords.Comparisons.Comparison.AND;
            case Values.Keywords.Comparisons.EQUALS_KEYWORD:
                return Values.Keywords.Comparisons.Comparison.EQUAL;
            case Values.Keywords.Comparisons.NOT_EQUAL_KEYWORD:
                return Values.Keywords.Comparisons.Comparison.NOT_EQUAL;
            case Values.Keywords.Comparisons.SMALLER_THAN_KEYWORD:
                return Values.Keywords.Comparisons.Comparison.SMALLER_THAN;
            case Values.Keywords.Comparisons.BIGGER_THAN_KEYWORD:
                return Values.Keywords.Comparisons.Comparison.BIGGER_THAN;
        }
        return Values.Keywords.Comparisons.Comparison.UNDEFINED;
    }

    public static string FindComparisonKeyword(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            throw new ArgumentNullException();

        if (!inputValue.Contains(Values.Keywords.Comparisons.OR_KEYWORD)
            && !inputValue.Contains(Values.Keywords.Comparisons.AND_KEYWORD)
            && !inputValue.Contains(Values.Keywords.Comparisons.EQUALS_KEYWORD)
            && !inputValue.Contains(Values.Keywords.Comparisons.NOT_EQUAL_KEYWORD)
            && !inputValue.Contains(Values.Keywords.Comparisons.SMALLER_THAN_KEYWORD)
            && !inputValue.Contains(Values.Keywords.Comparisons.BIGGER_THAN_KEYWORD))
        {
            throw new ArgumentException(inputValue);
        }
        
        if (!inputValue.Contains(Values.Keywords.Comparisons.NOT_EQUAL_KEYWORD)
            && inputValue.Contains(Values.Keywords.Comparisons.EQUALS_KEYWORD))
            return Values.Keywords.Comparisons.EQUALS_KEYWORD;
        if (inputValue.Contains(Values.Keywords.Comparisons.NOT_EQUAL_KEYWORD))
            return Values.Keywords.Comparisons.NOT_EQUAL_KEYWORD;
        if (inputValue.Contains(Values.Keywords.Comparisons.BIGGER_THAN_KEYWORD))
            return Values.Keywords.Comparisons.BIGGER_THAN_KEYWORD;
        if (inputValue.Contains(Values.Keywords.Comparisons.SMALLER_THAN_KEYWORD))
            return Values.Keywords.Comparisons.SMALLER_THAN_KEYWORD;

        throw new NotImplementedException();
    }
}
using NEASL.Base.Object;

namespace NEASL.Base;

public interface IBaseEventExecuter: INEASL_Object
{
    // The Unique Object Name. Used to Identify the Script to load.
    string NAME { get; }
    
    // Fetches the content of the script by the given file path and returns it as a string.
    string FetchScript(string scriptFilePath);

    void AssignScript(string FilePath, string FileName);
    
    // Reads the script Contents and connects Events via the given Signature names.
    bool LinkToScript();
    
    // Performs the content of a script event. where the IdentifierName is the part of the
    // script that is being executed.
    /* Example with IdentifierName = "PRESSED"
     * PRESSED:
     *      -> {SCRIPT PART THATS EXECUTED HERE LINE BY LINE!}
     * :PRESSED
     */
    void PerformScriptEvent(string IdentifierName);
}
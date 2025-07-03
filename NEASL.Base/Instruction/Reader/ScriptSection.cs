namespace NEASL.Base.Reader;

public class ScriptSection
{
    // Base Identifier used for the Section itself (e.g START:, APP: etc)
    public string KeyNameIdentifier;

    // Child Content Text.
    public string Content;

    // Raw Inputed String.
    public string RawString;

    public ScriptSection()
    {
        
    }
    
    public ScriptSection(string rawstring)
    {
        
    }
}
using NEASL.Base.Linking;

namespace NEASL.Base.AppContext.Interfaces;

public interface IBaseApplicationContext
{
    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    void WRITE_LINE(string text);

    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    void WRITE_LINE(string text, string keineAhnung);

    [Signature(nameof(READ_LINE), LinkType.Property)]
     void READ_LINE();
     
    [Signature(nameof(WAIT), LinkType.Method)]
    void WAIT(string seconds);
    
    void AssignScript(string scriptContent);

    void AssignScript(string FilePath, string FileName);

    string FetchScript(string scriptFilePath);
}
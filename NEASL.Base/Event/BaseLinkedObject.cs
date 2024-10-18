using System;
using System.Collections.Generic;
using System.IO;

namespace NEASL.Base;

public class BaseLinkedObject : BaseReceiver, IBaseLinkedObject
{
    public string NAME { get; private set;  }
    
    Dictionary<string,string> scriptSections = new Dictionary<string,string>();
    private string scriptRawContent = string.Empty;
    private string filePath = string.Empty;
    private string fileName = string.Empty;

    public BaseLinkedObject() : base()
    {
    }
    
    public BaseLinkedObject(string scriptContent)
    {
        scriptRawContent = scriptContent;
        LinkToScript();
    }
    
    public void AssignScript(string scriptContent)
    {
        scriptRawContent = scriptContent;
        LinkToScript();
    }
    
    public void AssignScript(string FilePath,string FileName)
    {
        scriptRawContent = FetchScript(System.IO.Path.Combine(FilePath, FileName));
        LinkToScript();
    }
   
    public string FetchScript(string scriptFilePath)
    {
        if (!File.Exists(scriptFilePath))
            throw new FileNotFoundException();

        return File.ReadAllText(scriptFilePath);
    }

    public bool LinkToScript()
    {
        if (string.IsNullOrEmpty(scriptRawContent))
            throw new MissingFieldException(this.scriptRawContent);

        var _scriptSections =Linker.LoadSections(this, this.scriptRawContent);
        if (_scriptSections!= null &&_scriptSections.Count > 0)
        {
            this.scriptSections = _scriptSections;
            return true;
        }
        return false;
    }

    public void PerformScriptEvent(string EventIdentifierName)
    {
        if (scriptSections == null || scriptSections != null && scriptSections.Count == 0)
        {
            Console.WriteLine($"[WARN] Requested Event / Method \"{EventIdentifierName}\" not found, Script was empty.");
            return;
        }

        if (!scriptSections.ContainsKey(EventIdentifierName))
        {
            Console.WriteLine($"[WARN] Requested Event / Method \"{EventIdentifierName}\" not found!");
            return;
        }
        if (scriptSections.ContainsKey(EventIdentifierName))
            InstructionRunner.GetInstance().Execute(this,scriptSections[EventIdentifierName]);
    }
    

}
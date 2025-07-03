using System;
using System.Collections.Generic;
using System.IO;
using NEASL.Base.Linking;

namespace NEASL.Base;

public class BaseLinkedObject : BaseReceiver, IBaseLinkedObject
{
    public string NAME { get; private set; }

    Dictionary<string, string> scriptSections = new Dictionary<string, string>();
    private string scriptRawContent = string.Empty;
    private string filePath = string.Empty;
    private string fileName = string.Empty;

    public Dictionary<string, string> GetScriptSections() => scriptSections;

    public BaseLinkedObject() : base()
    {
        //Context.GetInstance().GetEventManager().Register(this);
    }

    public BaseLinkedObject(string scriptContent)
    {
        if (string.IsNullOrEmpty(scriptContent))
            return;
        scriptRawContent = scriptContent;
        LinkToScript();
        base.SelfAssign();
    }

    public void AssignScript(string scriptContent)
    {
        scriptRawContent = scriptContent;
        LinkToScript();
        base.SelfAssign();
    }

    public void AssignScript(string FilePath, string FileName)
    {
        scriptRawContent = FetchScript(System.IO.Path.Combine(FilePath, FileName));
        if (string.IsNullOrEmpty(scriptRawContent))
            return;
        LinkToScript();
        base.SelfAssign();
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


        var name = Linker.GetObjectsName(this, this.scriptRawContent);
        if (string.IsNullOrEmpty(name) && this.ObjectType != IdentifierType.Root)
            Console.WriteLine("No Object name given! Skipping...");

        base.SetName(name);
        if (this.ObjectType != IdentifierType.Root && !string.IsNullOrEmpty(name))
            Console.Title = name;
        else
            Console.Title = "NEASL";
        


        var _scriptSections = Linker.LoadSections(this, this.scriptRawContent);
        if (_scriptSections != null && _scriptSections.Count > 0)
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
            //Console.WriteLine($"[WARN] Requested Event / Method \"{EventIdentifierName}\" not found, Script was empty.");
            return;
        }

        if (!scriptSections.ContainsKey(EventIdentifierName))
        {
            //Console.WriteLine($"[WARN] Requested Event / Method \"{EventIdentifierName}\" not found!");
            return;
        }

        if (scriptSections.ContainsKey(EventIdentifierName))
            InstructionRunner.GetInstance().Execute(this, scriptSections[EventIdentifierName]);
    }
}
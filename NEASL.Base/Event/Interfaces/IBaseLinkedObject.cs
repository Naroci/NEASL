namespace NEASL.Base;

public interface IBaseLinkedObject : IBaseEventExecuter, IBaseEventReceiver
{
    public string FetchScript(string scriptFilePath);

    public bool LinkToScript();

    public void PerformScriptEvent(string EventIdentifierName);
}
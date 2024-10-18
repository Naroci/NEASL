using NEASL.Base.Object;

namespace NEASL.Base;

public interface IBaseLinkedObject : IBaseEventExecuter, IBaseEventReceiver, INEASL_Object
{
    public string FetchScript(string scriptFilePath);

    public bool LinkToScript();

    public void PerformScriptEvent(string EventIdentifierName);
}
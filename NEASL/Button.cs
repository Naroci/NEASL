using NEASL.Base.Linking;

namespace NEASL.Base;

[Component("BUTTON")]
public class Button : BaseLinkedObject
{
    public Button(string FilePath, string FileName) : base( FilePath,  FileName)
    {
       
    }

    [Signature("PRESSED", LinkType.Event)]
    public void ButtonPress()
    {
        this.PerformScriptEvent("PRESSED");
    }

    public void TestCall(string asdf)
    {
        Console.WriteLine(asdf);
        EventCallFinished(nameof(TestCall), asdf);
    }
}
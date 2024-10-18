using NEASL.Base.Linking;

namespace NEASL.Base;

[Component("BUTTON")]
public class Button : BaseLinkedObject
{
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
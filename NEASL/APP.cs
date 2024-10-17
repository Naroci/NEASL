using NEASL.Base.Linking;

namespace NEASL.Base;

[Section(nameof(APP))]
public class APP : BaseReceiver
{
    public APP() : base()
    {
       
    }

    [Signature("WriteLine", LinkType.Method)]
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WriteLine), text);
    }
    
    [Signature("WriteLine", LinkType.Method)]
    public void WriteLine(string text, string keineAhnung)
    {
        Console.WriteLine(text + " " +keineAhnung);
        EventCallFinished(nameof(WriteLine), text,keineAhnung);
    }

    public void ShowPopup(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WriteLine), text);
    }
}
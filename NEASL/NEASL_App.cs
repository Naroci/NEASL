using System;
using NEASL.Base.Linking;

namespace NEASL.Base;

[Main("APP")]
public class NEASL_App : BaseLinkedObject
{
    public NEASL_App() : base()
    {
       
    }
    
    [Signature(nameof(START), LinkType.Event)]
    public void START()
    {
        this.PerformScriptEvent(nameof(START));
    }

    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    public void WRITE_LINE(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WRITE_LINE), text);
    }
    
    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    public void WRITE_LINE(string text, string keineAhnung)
    {
        Console.WriteLine(text + " " +keineAhnung);
        EventCallFinished(nameof(WRITE_LINE), text, keineAhnung);
    }
    
    [Signature(nameof(READ_LINE), LinkType.Method)]
    public void READ_LINE()
    {
        string result = Console.ReadLine();
        ReturnEventResult (nameof(READ_LINE),null, result);
    }
    
    public void ShowPopup(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(ShowPopup), text);
    }
}
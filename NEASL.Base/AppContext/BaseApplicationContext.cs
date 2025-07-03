using NEASL.Base.AppContext.Interfaces;
using NEASL.Base.Global.Definitions;
using NEASL.Base.Linking;

namespace NEASL.Base.AppContext;

[Main(Values.Application.APPLICATION_CONTEXT_KEYWORD_IDENTIFIER)] 
public class BaseApplicationContext : BaseLinkedObject, IBaseApplicationContext
{
 
    
    [Signature(nameof(START), LinkType.Event)]
    public void START()
    {
        this.PerformScriptEvent(nameof(START));
    }
    
  

    public BaseApplicationContext() : base()
    {
        base.SelfAssign();
    }
    
    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    public virtual void WRITE_LINE(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WRITE_LINE), text);
    }
    
    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    public virtual void WRITE_LINE(string text, string keineAhnung)
    {
        Console.WriteLine(text + " " +keineAhnung);
        EventCallFinished(nameof(WRITE_LINE), text,keineAhnung);
    }
    
    [Signature(nameof(WAIT), LinkType.Method)]
    public async void WAIT(string seconds)
    {
        try
        {
            int secondsParsed = -1;
            int.TryParse(seconds, out secondsParsed);
            if (secondsParsed > -1)
            {
                Thread.Sleep(secondsParsed * 1000);
                // Load the .axaml file
                EventCallFinished(nameof(WAIT), seconds);
                /*
                await Task.Run(() =>
                {
                    Thread.Sleep(secondsParsed * 1000);
                    // Load the .axaml file
                    EventCallFinished(nameof(WAIT), seconds);
                });*/
            }
        }
        catch (Exception)
        {
            
        }
        
       
    }

    [Signature(nameof(READ_LINE), LinkType.Method)]
    public virtual void READ_LINE()
    {
        string result = Console.ReadLine();
        ReturnEventResult (nameof(READ_LINE),null, result);
    }
}
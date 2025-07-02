using NEASL.Base.AppContext.Interfaces;
using NEASL.Base.Global.Definitions;
using NEASL.Base.Linking;

namespace NEASL.Base.AppContext;

[Main(Values.Application.APPLICATION_CONTEXT_KEYWORD_IDENTIFIER)] 
public class BaseApplicationContext : BaseLinkedObject, IBaseApplicationContext
{
    private static IBaseApplicationContext applicationContext;
    public static IBaseApplicationContext Initialize()
    {
        if (applicationContext == null)
            applicationContext = new BaseApplicationContext();
        return applicationContext;
    }
    
    public static T Initialize<T>() where T : BaseApplicationContext
    {
        object obj = null;
        obj = Activator.CreateInstance(typeof(T));
        if (obj == null || obj != null &&  obj is not IBaseApplicationContext)
        {
            applicationContext = null;
            return null;
        }
   
        applicationContext = obj as IBaseApplicationContext;
        return applicationContext as T;
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
                await Task.Run(() =>
                {
                    Thread.Sleep(secondsParsed * 1000);
                });
            }
        }
        catch (Exception)
        {
            
        }
        
        // Load the .axaml file
        EventCallFinished(nameof(WAIT), seconds);
    }

    [Signature(nameof(READ_LINE), LinkType.Method)]
    public virtual void READ_LINE()
    {
        string result = Console.ReadLine();
        ReturnEventResult (nameof(READ_LINE),null, result);
    }
}
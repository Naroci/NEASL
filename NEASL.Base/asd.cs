namespace NEASL.Base;

[InstructionIdentifier (nameof(asd))]
public class asd : BaseReceiver
{
    public asd() : base()
    {
        Context.GetInstance().GetEventManager().Register(this);
    }

    public void TestCall(string asdf)
    {
        Console.WriteLine(asdf);
        EventCallFinished(nameof(TestCall), asdf);
    }
    
    public void KeineAhnung(string asdf)
    {
        int i = 0;
        while (i < 10)
        {
            Thread.Sleep(100);
            Console.WriteLine(asdf);
            i++;
        }
        EventCallFinished(nameof(KeineAhnung), asdf);
    }
    
    public void Test2(string asdf, string asdf2)
    {
        int i = 0;
        while (i < 10)
        {
            Thread.Sleep(100);
            Console.WriteLine($"{asdf}, {asdf2}");
            i++;
        }
        EventCallFinished(nameof(Test2), asdf, asdf2);
    }
}
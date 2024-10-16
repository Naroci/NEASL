namespace NEASL.Base;

[InstructionIdentifier ("Hello")]
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
        Console.WriteLine(asdf);
        EventCallFinished(nameof(TestCall), asdf);
    }
}
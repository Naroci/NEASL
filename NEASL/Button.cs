using System;
using NEASL.Base.Linking;

namespace NEASL.Base;

[Component(nameof(BUTTON))]
public class BUTTON : BaseLinkedObject
{
    public BUTTON(string scriptContent) : base(scriptContent)
    {
    }
    
    public BUTTON() : base()
    {
    }

    [Signature(nameof(PRESSED), LinkType.Event)]
    public void PRESSED()
    {
        this.PerformScriptEvent(nameof(PRESSED));
    }
    
    [Signature(nameof(HOVER), LinkType.Event)]
    public void HOVER()
    {
        this.PerformScriptEvent(nameof(HOVER));
    }
    
    [Signature(nameof(LEAVE), LinkType.Event)]
    public void LEAVE()
    {
        this.PerformScriptEvent(nameof(LEAVE));
    }
}
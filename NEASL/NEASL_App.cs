using System;
using NEASL.Base.AppContext;
using NEASL.Base.Linking;

namespace NEASL.Base;

[Main("APP")]
public class NEASL_App : BaseApplicationContext
{
    public NEASL_App() : base()
    {
       
    }
    
    [Signature(nameof(START), LinkType.Event)]
    public void START()
    {
        this.PerformScriptEvent(nameof(START));
    }

}
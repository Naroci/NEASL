using System;
using Avalonia.Controls;
using Avalonia.Media;
using NEASL.Base.Linking;
using NEASL.CONTROLS;
using NEASL.TEST_GUI;

namespace NEASL.Base;

[Component("BUTTON")]
public class btn : BaseLinkedObject
{
    private NEASL_Button controlBtn;
    public btn() : base()
    {
        
    }
    
    public btn(string content) : base(content)
    {
        
    }

    public void SetControl(NEASL_Button control)
    {
        if (control == null)
            throw new ArgumentNullException(nameof(control));
        
        this.controlBtn = control;
    }

    [Signature(nameof(BACKGROUND_COLOR), LinkType.Method)]
    public void BACKGROUND_COLOR(string ColorStringValue)
    {
        if (this.controlBtn == null)
            return;
        
        var brush = new SolidColorBrush(Color.Parse(ColorStringValue));
        this.controlBtn.Background = brush;
        EventCallFinished(nameof(BACKGROUND_COLOR),ColorStringValue);
    }
    
    [Signature(nameof(SET_TEXT), LinkType.Method)]
    public void SET_TEXT(string textValue)
    {
        if (this.controlBtn == null)
            return;
        
        this.controlBtn.Text = textValue;
        EventCallFinished(nameof(SET_TEXT),textValue);
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

    public void TestCall(string asdf)
    {
        Console.WriteLine(asdf);
        EventCallFinished(nameof(TestCall), asdf);
    }
}
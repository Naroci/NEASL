using System;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using NEASL.Base.Linking;
using NEASL.TEST_GUI;
using NEASL.CONTROLS;

namespace NEASL.Base;

[Component(nameof(TEXTINPUT))]
public class TEXTINPUT : BaseLinkedObject
{
    private NEASL_TextInput _control;
    public TEXTINPUT() : base()
    {
        
    }
    
    public TEXTINPUT(string content) : base(content)
    {
        
    }

    public void SetControl(NEASL_TextInput control)
    {
        if (control == null)
            throw new ArgumentNullException(nameof(control));
        
        this._control = control;
    }

    [Signature(nameof(BACKGROUND_COLOR), LinkType.Method)]
    public void BACKGROUND_COLOR(string ColorStringValue)
    {
        if (this._control == null)
            return;
        
        var brush = new SolidColorBrush(Color.Parse(ColorStringValue));
        this._control.Background = brush;
        EventCallFinished(nameof(BACKGROUND_COLOR),ColorStringValue);
    }
    
    [Signature(nameof(SET_TEXT), LinkType.Method)]
    public void SET_TEXT(string textValue)
    {
        if (this._control == null)
            return;
        
        this._control.Text = textValue;
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
    
    
    [Signature(nameof(GET_TEXT), LinkType.Event)]
    public string GET_TEXT()
    {
        string result = Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (this._control == null)
                return null;

            return this._control.Text;
        }).Result;

        return result;
    }


    public void TestCall(string asdf)
    {
        Console.WriteLine(asdf);
        EventCallFinished(nameof(TestCall), asdf);
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NEASL.Base;

namespace NEASL.TEST_GUI.Controls.NEASL.TextLabel;

public partial class NEASL_TextLabel : NEASL_UserControl
{
    private bool eventsAssigned = false;
    TEXTLABEL _textlabel = new();
    
    public NEASL_TextLabel() : base()
    {
        _textlabel.SetControl(this);
        if (!eventsAssigned)
        {
            this.PointerPressed += delegate
            {
                _textlabel.PRESSED(); 
            };
            this.PointerEntered += delegate { _textlabel.HOVER(); };
            this.PointerExited += delegate { _textlabel.LEAVE(); };
            eventsAssigned = true;
        }
    }
    
    public override void AssignScript(string content)
    {
        _textlabel = new TEXTLABEL(content);
        _textlabel.AssignScript(content);
        _textlabel.SetControl(this);
        
        if (!eventsAssigned)
        {
            this.PointerPressed += delegate
            {
                _textlabel.PRESSED(); 
            };
            this.PointerEntered += delegate { _textlabel.HOVER(); };
            this.PointerExited += delegate { _textlabel.LEAVE(); };
            eventsAssigned = true;
        }
    }

}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NEASL.Base;
using NEASL.TEST_GUI;

namespace NEASL.CONTROLS;

public partial class NEASL_TextInput : NEASL_UserControl
{
    
    TEXTINPUT neaslObj = new();
    private bool eventsAssigned = false;
    
    public static readonly StyledProperty<double> TextSizeProperty =
        AvaloniaProperty.Register<NEASL_Button, double>(nameof(TextSize), defaultValue: 12.0d);
    
    public double TextSize
    {
        get => GetValue(TextSizeProperty);
        set => SetValue(TextSizeProperty, value);
    }
    
 
    
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Button, string>(nameof(Text), defaultValue: "Button");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    
    public NEASL_TextInput() : base()
    {
       
        InitializeComponent();
        neaslObj.SetControl(this);
        if (!eventsAssigned)
        {
            this.textBox.PointerEntered += delegate { neaslObj.HOVER(); };
            this.textBox.PointerExited += delegate { neaslObj.LEAVE(); };
            eventsAssigned = true;
        }
    }

    public NEASL_TextInput(string scriptContent)
    {
        InitializeComponent();
        neaslObj.SetControl(this);
        neaslObj.AssignScript(scriptContent);
        if (!eventsAssigned)
        {
            this.textBox.PointerEntered += delegate { neaslObj.HOVER(); };
            this.textBox.PointerExited += delegate { neaslObj.LEAVE(); };
            eventsAssigned = true;
        }
    }

    public override void AssignScript(string content)
    {
        neaslObj = new TEXTINPUT(content);
        neaslObj.AssignScript(content);
        neaslObj.SetControl(this);
        if (!eventsAssigned)
        {
            this.textBox.PointerEntered += delegate { neaslObj.HOVER(); };
            this.textBox.PointerExited += delegate { neaslObj.LEAVE(); };
            eventsAssigned = true;
        }
    }

    /*
    public NEASL_Button(string path,string fileName)
    {
        InitializeComponent();
        InitScript(path, fileName);
        Btn.Click += delegate { PRESSED(); };
        Btn.PointerEntered += delegate { HOVER(); };
        Btn.PointerExited += delegate { LEAVE(); };
    }*/
}
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NEASL.Base;
using NEASL.Base.Linking;

namespace NEASL.TEST_GUI;

[Component("BUTTON")]
public partial class NEASL_Button : NEASL_UserControl
{
    public static readonly StyledProperty<double> TextSizeProperty =
        AvaloniaProperty.Register<NEASL_Button, double>(nameof(TextSize), defaultValue: 12.0d);

    public double TextSize
    {
        get => GetValue(TextSizeProperty);
        set => SetValue(TextSizeProperty, value);
        
    }
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<NEASL_Button, string>(nameof(Text), defaultValue: "Button");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public NEASL_Button()
    {
        InitializeComponent();
        string path = Environment.CurrentDirectory;
        string fileName = @"BUTTON.neasl";
        //InitScript(path, fileName);
        
        Btn.Click += delegate { PRESSED(); };
        Btn.PointerEntered += delegate { HOVER(); };
        Btn.PointerExited += delegate { LEAVE(); };
    }

    public NEASL_Button(string scriptContent)
    {
        InitializeComponent();
        AssignScript(scriptContent);
        Btn.Click += delegate { PRESSED(); };
        Btn.PointerEntered += delegate { HOVER(); };
        Btn.PointerExited += delegate { LEAVE(); };
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
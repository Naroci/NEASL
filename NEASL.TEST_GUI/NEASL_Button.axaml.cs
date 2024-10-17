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
        InitScript(path, fileName);
        Btn.Click += delegate { ButtonPress(); };
    }
    
    public NEASL_Button(string path,string fileName)
    {
        InitializeComponent();
        InitScript(path, fileName);
        Btn.Click += delegate { ButtonPress(); };
    }
    
    [Signature("PRESSED", LinkType.Event)]
    public void ButtonPress()
    {
        this.PerformScriptEvent("PRESSED");
    }
}
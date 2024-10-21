using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NEASL.Base;
using NEASL.Base.Linking;

namespace NEASL.TEST_GUI;

public partial class NEASL_Button : NEASL_UserControl
{
    private bool eventsAssigned = false;
    public static readonly StyledProperty<double> TextSizeProperty =
        AvaloniaProperty.Register<NEASL_Button, double>(nameof(TextSize), defaultValue: 12.0d);

    btn btn = new();
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
        btn.SetControl(this);
        if (!eventsAssigned)
        {
            Btn.Click += delegate { btn.PRESSED(); };
            Btn.PointerEntered += delegate { btn.HOVER(); };
            Btn.PointerExited += delegate { btn.LEAVE(); };
            eventsAssigned = true;
        }
    }

    public NEASL_Button(string scriptContent)
    {
        InitializeComponent();
        btn.SetControl(this);
        btn.AssignScript(scriptContent);
        if (!eventsAssigned)
        {
            Btn.Click += delegate { btn.PRESSED(); };
            Btn.PointerEntered += delegate { btn.HOVER(); };
            Btn.PointerExited += delegate { btn.LEAVE(); };
            eventsAssigned = true;
        }
    }

    public void AssignScript(string content)
    {
        btn = new btn(content);
        btn.AssignScript(content);
        btn.SetControl(this);
        if (!eventsAssigned)
        {
            Btn.Click += delegate { btn.PRESSED(); };
            Btn.PointerEntered += delegate { btn.HOVER(); };
            Btn.PointerExited += delegate { btn.LEAVE(); };
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
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NEASL.Base;
using NEASL.Base.Linking;

namespace NEASL.TEST_GUI;

public partial class NEASL_Button : NEASL_UserControl
{
    btn btn = new();
    private bool eventsAssigned = false;
    
    public static readonly StyledProperty<double> TextSizeProperty =
        AvaloniaProperty.Register<NEASL_Button, double>(nameof(TextSize), defaultValue: 12.0d);
    
    public static readonly StyledProperty<object?> contentProp =
        AvaloniaProperty.Register<NEASL_Button, object>(nameof(Content), defaultValue: null);
    
    public double TextSize
    {
        get => GetValue(TextSizeProperty);
        set => SetValue(TextSizeProperty, value);
    }
    
    public object? Content
    {
        get => Btn.Content;
        set => Btn.Content = value;
    }
    
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Button, string>(nameof(Text), defaultValue: "Button");

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    
    public NEASL_Button() : base()
    {
       
        InitializeComponent();
        btn.SetControl(this);
        if (!eventsAssigned)
        {
            this.Btn.Click += delegate { btn.PRESSED(); };
            this.Btn.PointerEntered += delegate { btn.HOVER(); };
            this.Btn.PointerExited += delegate { btn.LEAVE(); };
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
            this.Btn.Click += delegate { btn.PRESSED(); };
            this.Btn.PointerEntered += delegate { btn.HOVER(); };
            this.Btn.PointerExited += delegate { btn.LEAVE(); };
            eventsAssigned = true;
        }
    }

    public override void AssignScript(string content)
    {
        btn = new btn(content);
        btn.AssignScript(content);
        btn.SetControl(this);
        if (!eventsAssigned)
        {
            this.Btn.Click += delegate { btn.PRESSED(); };
            this.Btn.PointerEntered += delegate { btn.HOVER(); };
            this.Btn.PointerExited += delegate { btn.LEAVE(); };
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
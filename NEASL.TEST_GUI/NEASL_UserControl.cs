using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using NEASL.Base;
using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Markup.Xaml;
using NEASL.Base.Linking;

namespace NEASL.TEST_GUI;

public class NEASL_UserControl : UserControl
{
    public static readonly StyledProperty<string> NAMEProperty =
        AvaloniaProperty.Register<NEASL_Button, string>(nameof(NAME), defaultValue: "newCtrl");

    public string NAME
    {
        get => GetValue(NAMEProperty);
        set => SetValue(NAMEProperty, value);
    }
    
    
    Dictionary<string,string> scriptSections = new Dictionary<string,string>();
    private string scriptRawContent = string.Empty;
    private string filePath = string.Empty;
    private string fileName = string.Empty;
    private object m_parent;
    
    public NEASL_UserControl() : base() 
    {
        
    }
    
    public void LoadFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            AvaloniaXamlLoader.Load(new Uri(fileName));
        }
    }


}

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
using NEASL.TEST_GUI;

namespace NEASL.CONTROLS;

public class NEASL_UserControl : UserControl, INEASL_UserControl
{
    public static readonly StyledProperty<string> scriptPathProperty =
        AvaloniaProperty.Register<NEASL_UserControl, string>(nameof(Script), defaultValue: String.Empty);
    

    public string Script
    {
        get
        {
            return GetValue(scriptPathProperty);
        }
        set
        {
            SetValue(scriptPathProperty, value);
        }
    }

    public virtual void AssignScript(string content)
    {
        
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

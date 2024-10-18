using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using NEASL.Base;
using System.Reflection;
using System.Runtime.CompilerServices;
using NEASL.Base.Linking;

namespace NEASL.TEST_GUI;

public class NEASL_UserControl : UserControl
{
    public string NAME { get; private set;  }
    
    Dictionary<string,string> scriptSections = new Dictionary<string,string>();
    private string scriptRawContent = string.Empty;
    private string filePath = string.Empty;
    private string fileName = string.Empty;
    private object m_parent;
    
 
 
}

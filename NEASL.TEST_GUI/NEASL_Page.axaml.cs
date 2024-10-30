using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace NEASL.TEST_GUI;

public partial class NEASL_Page : NEASL_UserControl
{
    public NEASL_Page()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
    }
    
    public  IEnumerable<T> GetNEASL_UserControls<T>()
    {
        return this.GetLogicalDescendants().OfType<T>();
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NEASL.TEST_GUI;

public partial class NEASL_Page : NEASL_UserControl
{
    public NEASL_Page()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
    }
}
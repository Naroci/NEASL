using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NEASL.PAGES;

namespace NEASL.TEST_GUI;

public partial class PreviewView : UserControl,INavigationControl
{
    public PreviewView()
    {
        InitializeComponent();
        App.NavigationControl = this;
    }

    public NEASL_Page GetCurrentPage()
    {
        if (CurrentPageDisplay != null && CurrentPageDisplay.Child != null && CurrentPageDisplay.Child is NEASL_Page page)
        {
            return page;
        }
        return null;
    }

    public void SetCurrentPage(NEASL_Page page)
    {
        if (page != null && CurrentPageDisplay != null)
        {
            CurrentPageDisplay.Child = page;
        }
    }
}

public interface INavigationControl
{
    public NEASL_Page GetCurrentPage();

    public void SetCurrentPage(NEASL_Page page);
}
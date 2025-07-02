using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NEASL.Base;
using NEASL.Base.AppContext;

namespace NEASL.TEST_GUI;

public partial class App : Application
{
    public static INavigationControl NavigationControl { get; set; }

    private static Window _mainWindow;
    public static Window GetMainWindow()
    {
        return _mainWindow;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        NEASL_App app = BaseApplicationContext.Initialize<NEASL_App>();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new SampleEditorPage();
            //desktop.MainWindow = new MainWindow();
            _mainWindow = desktop.MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
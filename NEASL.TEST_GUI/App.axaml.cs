using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NEASL.Base;

namespace NEASL.TEST_GUI;

public partial class App : Application
{
    private static Window _mainWindow;
    public static Window GetMainWindow()
    {
        return _mainWindow;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        NEASL_App app = new NEASL_App();
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
using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using NEASL.Base.Linking;
using NEASL.TEST_GUI;

namespace NEASL.Base;

[Section("APP")]
public class NEASL_App : BaseReceiver
{
    public NEASL_App() : base()
    {
       
    }

    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    public void WRITE_LINE(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WRITE_LINE), text);
    }
    
    [Signature(nameof(WRITE_LINE), LinkType.Method)]
    public void WRITE_LINE(string text, string keineAhnung)
    {
        Console.WriteLine(text + " " +keineAhnung);
        EventCallFinished(nameof(WRITE_LINE), text,keineAhnung);
    }

    public void ShowPopup(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(ShowPopup), text);
    }

    [Signature(nameof(SHOW_DIALOG), LinkType.Method)]
    public void SHOW_DIALOG(string Title, string Message)
    {
        Window window = new Window();
        window.Title = Title;
        window.HorizontalContentAlignment = HorizontalAlignment.Center;
        window.VerticalContentAlignment = VerticalAlignment.Center;
        window.Content = Message;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        window.ShowDialog(App.GetMainWindow());
        EventCallFinished(nameof(SHOW_DIALOG),Title,Message);
    }
    
    [Signature(nameof(SET_BACKGROUND_COLOR), LinkType.Method)]
    public void SET_BACKGROUND_COLOR(string colorValue)
    {
        var brush = new SolidColorBrush(Color.Parse(colorValue));
        App.GetMainWindow().Background = brush;
        EventCallFinished(nameof(SET_BACKGROUND_COLOR),colorValue);
    }
}
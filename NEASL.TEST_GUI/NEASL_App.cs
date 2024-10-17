using System;
using Avalonia.Controls;
using NEASL.Base.Linking;

namespace NEASL.Base;

[Section("APP")]
public class NEASL_App : BaseReceiver
{
    public NEASL_App() : base()
    {
       
    }

    [Signature("WriteLine", LinkType.Method)]
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WriteLine), text);
    }
    
    [Signature("WriteLine", LinkType.Method)]
    public void WriteLine(string text, string keineAhnung)
    {
        Console.WriteLine(text + " " +keineAhnung);
        EventCallFinished(nameof(WriteLine), text,keineAhnung);
    }

    public void ShowPopup(string text)
    {
        Console.WriteLine(text);
        EventCallFinished(nameof(WriteLine), text);
    }

    [Signature("SHOW_WINDOW", LinkType.Method)]
    public void ShowWindow(string Title)
    {
        Window window = new Window();
        window.Title = Title;
        window.Show();
        EventCallFinished(nameof(ShowWindow),Title);
    }
}
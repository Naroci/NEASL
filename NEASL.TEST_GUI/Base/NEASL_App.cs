using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using NEASL.Base.Linking;
using NEASL.TEST_GUI;

namespace NEASL.Base;

[Main("APP")]
public class NEASL_App : BaseReceiver
{
    public NEASL_App() : base()
    {
       base.SelfAssign();
       new NEASL_Downloader();
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
    
    
    [Signature(nameof(READ_LINE), LinkType.Property)]
    public string READ_LINE()
    {
        string result = Console.ReadLine();
        return result;
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
        window.FontSize = 32;
        window.Content = Message;
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        window.Height = 200;
        window.Width = 300;
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
    
    
    [Signature(nameof(NAVIGATE), LinkType.Method)]
    public async void NAVIGATE(string pageName)
    {
        if (!pageName.Contains(".axaml"))
            pageName = pageName + ".axaml";
        
        string path = Environment.CurrentDirectory;
        string filePath = System.IO.Path.Combine(path,pageName);
        
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            NEASL_Page returnCtrl = new NEASL_Page();
          
            if (File.Exists(filePath))
            {
                string xaml = File.ReadAllText(filePath);
                object asd = AvaloniaRuntimeXamlLoader.Parse<ContentControl>(xaml);
                returnCtrl = (NEASL_Page)asd;
                if (!returnCtrl.SciptsLoaded)
                    returnCtrl.LoadScripts();
            }
            
            if(App.NavigationControl != null && App.NavigationControl.GetCurrentPage() != null)
                NavigationHistory.Add((object)App.NavigationControl.GetCurrentPage());

            App.NavigationControl.SetCurrentPage(returnCtrl);
        });
        // Load the .axaml file
        
        EventCallFinished(nameof(NAVIGATE),pageName);
    }
    
    [Signature(nameof(NAVIGATE_BACK), LinkType.Method)]
    public async void NAVIGATE_BACK()
    {
        string path = Environment.CurrentDirectory;
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (NavigationHistory != null && NavigationHistory.Count > 0)
            {
               var itm = (NEASL_Page)NavigationHistory.Last();
               if (!itm.SciptsLoaded)
                   itm.LoadScripts();
               
               if(App.NavigationControl.GetCurrentPage() != null)
                   NavigationHistory.Add(App.NavigationControl.GetCurrentPage());
            
               App.NavigationControl.SetCurrentPage(itm);
               NavigationHistory.Remove(itm);
            }
        });
        
        // Load the .axaml file
        EventCallFinished(nameof(NAVIGATE_BACK));
    }
    
    [Signature(nameof(WAIT), LinkType.Method)]
    public async void WAIT(string seconds)
    {
        try
        {
            int secondsParsed = -1;
            int.TryParse(seconds, out secondsParsed);
            if (secondsParsed > -1)
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(secondsParsed * 1000);
                });
            }
        }
        catch (Exception)
        {
            
        }
        
        // Load the .axaml file
        EventCallFinished(nameof(WAIT), seconds);
    }
    
    List<object> NavigationHistory = new List<object>();
}
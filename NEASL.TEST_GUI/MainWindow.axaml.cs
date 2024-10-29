using System;
using System.IO;
using System.Net;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace NEASL.TEST_GUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        setTextInput();
    }
    
    public string FetchScript(string scriptFilePath)
    {
        if (!File.Exists(scriptFilePath))
            throw new FileNotFoundException();

        return File.ReadAllText(scriptFilePath);
    }

    private void setTextInput()
    {
        string path = System.IO.Path.Combine(Environment.CurrentDirectory, "BUTTON.neasl");
        string script = FetchScript(path);
        this.TextInput.Text = script;
        var btn = this.FindControl<NEASL_Button>("NeaslButton");
        btn.AssignScript(script);
        
        string path2 = System.IO.Path.Combine(Environment.CurrentDirectory, "BUTTON2.neasl");
        string script2 = FetchScript(path2);
        this.TextInput2.Text = script2;
        var btn2 = this.FindControl<NEASL_Button>("Button2");
        btn2.AssignScript(script2);
    }


    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var btn = this.FindControl<NEASL_Button>("NeaslButton");
        if (btn != null)
        {
            btn.AssignScript(this.TextInput.Text);
        }
        
        var btn2 = this.FindControl<NEASL_Button>("Button2");
        if (btn2 != null)
        {
            btn2.AssignScript(this.TextInput2.Text);
        }

   
    }
}
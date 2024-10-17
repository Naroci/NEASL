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
    }


    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var btn = this.FindControl<NEASL_Button>("NeaslButton");
        if (btn != null)
        {
            btn.ReAssign(this.TextInput.Text);
        }

   
    }
}
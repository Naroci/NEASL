using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace NEASL.TEST_GUI;

public partial class SampleEditorPage : Window
{
    public List<string> scripts { get; set; } = new List<string>();
    
    public SampleEditorPage()
    {
        InitializeComponent();
    }

    private async void LoadLayoutClick(object? sender, RoutedEventArgs e)
    {
        
        string result = await OpenFile(this);
        if (result != null)
        {
            NEASL_UserControl ctrl = await LoadControl(result);
            if (ctrl != null && ctrl is NEASL_Page page   && this.pagePreview != null)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    this.pagePreview.SetCurrentPage(page);
                    LoadScripts(page);
                    ControlCombo.ItemsSource = scripts;
                });
                this.LayoutContent.Text = File.ReadAllText(result);
                this.LayoutPathText.Text = result;
                
            }
        }
    }

    private async void LoadScripts(NEASL_Page page)
    {
        scripts = new List<string>();
        if (page != null)
        {
            var controls = page.GetNEASL_UserControls<INEASL_UserControl>();
            if (controls != null)
            {
                foreach (var ctrl in controls)
                {
                    if (ctrl != null && !string.IsNullOrEmpty(ctrl.Script) && File.Exists(ctrl.Script))
                    {
                        string fileContent = File.ReadAllText(ctrl.Script);
                        ctrl.AssignScript(fileContent);
                        scripts.Add(ctrl.GetType().Name);
                    }
                }
            }
        }
    }

    private async Task<NEASL_UserControl> LoadControl(string ctrlPath)
    {
        try
        {
            NEASL_UserControl returnCtrl = null;
            if (File.Exists(ctrlPath))
            {
                string xaml = File.ReadAllText(ctrlPath);
                returnCtrl = await loadControlFromAXAMLTEXT(xaml);
            }

            return returnCtrl;
        }
        catch (Exception ex)
        {
            
        }
        return null;
    }

    private async Task<NEASL_UserControl> loadControlFromAXAMLTEXT(string axaml)
    {
       
        return await Dispatcher.UIThread.InvokeAsync(() => 
                {
                    try
                    {
                        object asd = AvaloniaRuntimeXamlLoader.Parse<ContentControl>(axaml);
                        return (NEASL_UserControl)asd;
                    }
                    catch (Exception ex)
                    {
            
                    }

                    return null;
                }
            );
    }


    public async Task<string> OpenFile(Window owner)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a File",
            AllowMultiple = false,  // Set to true if multiple file selection is allowed
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Avalonia Files", Extensions = { "axaml" } },
                new FileDialogFilter { Name = "All Files", Extensions = { "*" } }
            }
        };

        // Show the dialog and get the selected file paths
        string[]? result = await dialog.ShowAsync(owner);

        if (result != null && result.Length > 0)
        {
            string selectedFile = result[0];
            return selectedFile;
        }

        return null;
    }

    private async void LoadLayoutFromText(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(this.LayoutContent.Text))
        {
            var xamlContent = this.LayoutContent.Text;
            var ctrl =  await loadControlFromAXAMLTEXT(xamlContent);
            if (ctrl != null && ctrl is NEASL_Page page   && this.pagePreview != null)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    this.pagePreview.SetCurrentPage(page);
                    LoadScripts(page);
                    ControlCombo.ItemsSource = scripts;
                });
            }
            else
            {
                ControlCombo.Items.Clear();
            }
        }
    }

    private INEASL_UserControl currentSelecteditm = null;

    private void ControlCombo_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        if ( ControlCombo.SelectedIndex  > -1 && ControlCombo.SelectionBoxItem != null && this.scripts != null  && this.ControlCombo.Items.Count == this.scripts.Count)
        {
            currentSelecteditm = null;
            var index = ControlCombo.SelectedIndex;
            var ctrls = this.pagePreview.GetCurrentPage().GetNEASL_UserControls<INEASL_UserControl>();
            if (ctrls != null && ctrls.ToList() != null && ctrls.ToList().Count == this.scripts.Count)
            {
                currentSelecteditm = ctrls.ToList()[index];
                if (currentSelecteditm != null)
                {
                    scriptPath.Text = currentSelecteditm.Script;
                    if (!string.IsNullOrEmpty(scriptPath.Text) && File.Exists(scriptPath.Text))
                    {
                        ScriptContent.Text = File.ReadAllText(scriptPath.Text);
                    }
                }
            }
        }
    }

    private void LoadNewScript(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(ScriptContent.Text) && currentSelecteditm != null)
        {
            currentSelecteditm.AssignScript(ScriptContent.Text);
        }
    }

    private void SaveScript(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(scriptPath.Text) && currentSelecteditm != null)
        {
            File.WriteAllText(scriptPath.Text,ScriptContent.Text);
            currentSelecteditm.AssignScript(ScriptContent.Text);
        }
    }
}
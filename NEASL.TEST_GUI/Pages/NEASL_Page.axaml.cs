using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using NEASL.CONTROLS;
using NEASL.TEST_GUI;

namespace NEASL.PAGES;


public partial class NEASL_Page : NEASL_UserControl
{
    public NEASL_Page()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
    }
    
    public  IEnumerable<T> GetNEASL_UserControls<T>()
    {
        return this.GetLogicalDescendants().OfType<T>();
    }
    
    private bool scriptsLoaded = false;
    public bool SciptsLoaded => scriptsLoaded;

    public async Task<List<string>> LoadScripts()
    {
        List<string>  scripts = new List<string>();
        try
        {     
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                string path = Environment.CurrentDirectory;
                
                var controls = GetNEASL_UserControls<INEASL_UserControl>();
                if (controls != null)
                {
                    foreach (var ctrl in controls)
                    {
                        if (ctrl != null && !string.IsNullOrEmpty(ctrl.Script) && File.Exists(System.IO.Path.Combine(path,ctrl.Script)))
                        {
                            string fileContent = File.ReadAllText(System.IO.Path.Combine(path,ctrl.Script));
                            ctrl.AssignScript(fileContent);
                            scripts.Add(ctrl.GetType().Name);
                        }
                    }

                    scriptsLoaded = true;
                }
            });

        }
        catch (Exception ex)
        {
        }
        return scripts;
    }
}
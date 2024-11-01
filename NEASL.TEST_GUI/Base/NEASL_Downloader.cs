using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

[Main("DOWNLOADER")]
public class NEASL_Downloader : BaseReceiver
{
    public NEASL_Downloader() : base()
    {
       base.SelfAssign();
    }

    [Signature(nameof(DOWNLOADINFO), LinkType.Method)]
    public async void DOWNLOADINFO(string url)
    {
        Console.WriteLine(url);
        HttpClient client = new HttpClient();
        try
        {
           var result = await client.GetAsync(url);
           if (result.IsSuccessStatusCode)
           {
               string stringResult = await result.Content.ReadAsStringAsync();
               setResult("RESULT", stringResult);
           }
        }
        catch (Exception ex)
        {
            
        }

        EventCallFinished(nameof(DOWNLOADINFO), url);
    }
    
    [Signature(nameof(RESULT), LinkType.Property)]
    public string RESULT()
    {
        string resultKey = nameof(RESULT);
        if (scriptVariables.ContainsKey(resultKey))
            return (string)scriptVariables[resultKey];

        return null;
    }

    private void setResult(string varName, object value)
    {
        if (scriptVariables.ContainsKey(varName))
            scriptVariables[varName] = value;
        else {
            scriptVariables.Add(varName, value);
        }
    }
}
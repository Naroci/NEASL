using Avalonia.Controls;

namespace NEASL.TEST_GUI;

public interface INEASL_UserControl 
{

    void AssignScript(string content);

    string Script { get; set; }
}
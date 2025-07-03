using NEASL.Base;
using NEASL.Base.AppContext;

namespace NEASL.Test;

[TestClass]
public sealed class LogicTests
{
    [TestMethod]
    public void ConditionTest()
    {
        var app = Base.NEASL.Initialize();
        string path = Environment.CurrentDirectory;
        string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";
        app.AssignScript(path, "script/app.neasl");
        if (app is BaseApplicationContext appl)
        {
           appl.PerformScriptEvent("START");
        }

        /*
        var result = PackageManager.SearchPackageFiles(path);
        string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";

        Button btnTest = new Button();
        btnTest.AssignScript(path, fileName);
        btnTest.ButtonPress();
        */
  
    }
}
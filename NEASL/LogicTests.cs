using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEASL.Base;
using NEASL.Base.AppContext;

namespace NEASL.Test;

[TestClass]
public sealed class LogicTests
{
    [TestMethod]
    public void ConditionTest()
    {
        var app = BaseApplicationContext.Initialize<NEASL_App>();
      
        string path = Environment.CurrentDirectory;
        string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";
       
           
           
        app.AssignScript(path, "script/app.neasl");
        app.START();
            
        BUTTON btn = new BUTTON();
        btn.AssignScript(path, fileName);
        btn.PRESSED();
            
        BUTTON btn2 = new BUTTON();
        /*
        var result = PackageManager.SearchPackageFiles(path);
        string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";

        Button btnTest = new Button();
        btnTest.AssignScript(path, fileName);
        btnTest.ButtonPress();
        */
      
    }
}
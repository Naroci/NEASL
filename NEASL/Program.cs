// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Reflection;
using NEASL.Base.Package;

namespace NEASL.Base
{
    // NEASL - Not Even A Scripting Language.
    public class Program
    {
        public static int Main(string[] args)
        {
            NEASL_App main = new NEASL_App();
            
            string path = Environment.CurrentDirectory;
            string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";
            BUTTON btn = new BUTTON();
            btn.AssignScript(path, fileName);
            btn.PRESSED();
           
           
            //main.AssignScript(path, "script/app.neasl");
            //main.START();
            /*
            var result = PackageManager.SearchPackageFiles(path);
            string fileName = @"script/PAGE/BUTTON/BUTTON.neasl";
            
            Button btnTest = new Button();
            btnTest.AssignScript(path, fileName);
            btnTest.ButtonPress();
            */
            return 0;
        }
        
        static string asdf = "asdf";
        private static string sample = "asd->TestCall(\"test\")\nasd->KeineAhnung(\"Keine Ahnung irgendwas anderes...\")\nasd->KeineAhnung(\"Mh!\")\nasd->Test2(\"Hallo Welt\",\"Mh!\")";
    }
}

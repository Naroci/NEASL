// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Reflection;

namespace NEASL.Base
{
    // NEASL - Not Even A Scripting Language.
    public class Program
    {
        public static int Main(string[] args)
        {
            APP main = new APP();
            string path = Environment.CurrentDirectory;
            string fileName = @"BUTTON.neasl";
            Button btnTest = new Button(path,fileName);
            btnTest.ButtonPress();
            return 0;
        }
        
        static string asdf = "asdf";
        private static string sample = "asd->TestCall(\"test\")\nasd->KeineAhnung(\"Keine Ahnung irgendwas anderes...\")\nasd->KeineAhnung(\"Mh!\")\nasd->Test2(\"Hallo Welt\",\"Mh!\")";
    }
}

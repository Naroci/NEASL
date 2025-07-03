// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Reflection;
using NEASL.Base.AppContext;
using NEASL.Base.Package;

namespace NEASL.Base
{
    // NEASL - Not Even A Scripting Language.
    public class Program
    {
        private const string defaultMessage =
            "Usage: neasl [options]\nUsage: neasl [path-to-application]\n\npath-to-application:\n  The path to an application file to execute.\n";

        public static int Main(string[] args)
        {
            string pgrmFileName = null;
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                pgrmFileName = args[0];
            else
            {
#if DEBUG
                try
                {
                    if (Environment.GetEnvironmentVariables() != null )
                    {
                        pgrmFileName = Environment.GetEnvironmentVariable("ScriptPath");
                        if (string.IsNullOrEmpty(pgrmFileName) == false && !File.Exists(pgrmFileName))
                        {
                            Console.WriteLine($"Script {pgrmFileName} was not found");
                            return 1;
                        }

                        string asd = File.ReadAllText(pgrmFileName);
                    }
                }
                catch (Exception e)
                {
                }
#endif
            }

            if (string.IsNullOrEmpty(pgrmFileName))
            {
                Console.WriteLine("No script file provided.\n");
                Console.WriteLine(defaultMessage);
                return 0;
            }

            var app = NEASL.Initialize<NEASL_App>();
            string path = Environment.CurrentDirectory;

            app.AssignScript(path, pgrmFileName);
            app.START();
            //app.PerformScriptEvent("lel");
            Console.Title = string.Empty;
            return 0;
        }
    }
}
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
            AssemblyTest();
            StringReader rs = new StringReader(sample);
            InstructionReader reader = new InstructionReader();
            List<Instruction> instructions = new List<Instruction>();
            
            string line;
            while ((line = rs.ReadLine()) != null)
            {
                Instruction instruction = reader.getInstruction(line);
                if (instruction != null)
                {
                    instructions.Add(instruction);
                }
            }

            if (instructions.Count > 0)
            {
                Context.GetInstance().GetQueryManager().AddToQuery(instructions);
            }
            Context.GetInstance().GetQueryManager().Start();
            return 0;
        }

        private static void AssemblyTest()
        {
           var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
           if (loadedAssemblies != null && loadedAssemblies.Length > 0)
           {
               foreach (var assembly in loadedAssemblies)
               {
                   if (assembly.GetTypes().Any(t => t.IsSubclassOf(typeof(BaseReceiver))))
                   {
                       var results = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseReceiver))).ToList();
                       if (results != null && results.Count > 0)
                       {
                           foreach (var result in results)
                           {
                               Console.WriteLine($"Found \"{result.Name}\" Type");
                           }
                           Console.WriteLine($"Found {results.Count} assemblies");
                       }
                   }
               }
           }
        }

        static string asdf = "asdf";
        private static string sample = "asd->TestCall(\"test\")\nasd->KeineAhnung(\"Keine Ahnung irgendwas anderes...\")\nasd->KeineAhnung(\"Mh!\")\nasd->Test2(\"Hallo Welt\",\"Mh!\")";
    }
}

// See https://aka.ms/new-console-template for more information

namespace NEASL.Base
{
    // NEASL - Not Even A Scripting Language.
    public class Program
    {
        public static int Main(string[] args)
        {
            asd asd = new asd();
            
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
        
        static string asdf = "asdf";
        private static string sample = "Hello->TestCall(\"test\")\nHello->TestCall(\"Keine Ahnung irgendwas anderes...\")";
    }
}

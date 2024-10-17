namespace NEASL.Base;

public class InstructionRunner
{
    private static InstructionRunner m_instructionRunnerl = null;
    InstructionReader reader = new InstructionReader();
            
  
    public static InstructionRunner GetInstance()
    {
        if (m_instructionRunnerl == null)
            m_instructionRunnerl = new InstructionRunner();

        return m_instructionRunnerl;
    }

    public async void Execute(string instructionSection)
    {
        if (string.IsNullOrEmpty(instructionSection))
            return;
        
       var instructions = GetInstructions(instructionSection);
       if (instructions.Count < 1)
       {
           Console.WriteLine("nothing to do.");
           return;
       }
       
       Context.GetInstance().GetQueryManager().AddToQuery(instructions);
       
       if (!Context.GetInstance().GetQueryManager().IsRunning())
           Context.GetInstance().GetQueryManager().Start();
    }

    private List<Instruction> GetInstructions(string instructionSection)
    {
        List<Instruction> instructions = new List<Instruction>();
        string line = string.Empty;
        StringReader rs = new StringReader(instructionSection);
        while ((line = rs.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            
            string trimmedLine = line.TrimStart();
            
            Instruction instruction = reader.getInstruction(trimmedLine);
            if (instruction != null)
            {
                instructions.Add(instruction);
            }
        }

        return instructions;
    }
}
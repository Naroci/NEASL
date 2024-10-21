using System;
using System.Collections.Generic;
using System.IO;
using NEASL.Base.Object;

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

    public async void Execute(INEASL_Object sender, string instructionSection)
    {
        if (string.IsNullOrEmpty(instructionSection))
            return;
        
       var instructions = GetInstructions(sender,instructionSection);
       if (instructions.Count < 1)
       {
           Console.WriteLine("nothing to do.");
           return;
       }
       
       Context.GetInstance().GetQueryManager().AddToQuery(instructions);
       
       if (!Context.GetInstance().GetQueryManager().IsRunning())
           Context.GetInstance().GetQueryManager().Start();
    }

    private List<Instruction> GetInstructions(INEASL_Object sender,string instructionSection)
    {
        List<Instruction> instructions = new List<Instruction>();
        string line = string.Empty;
        StringReader rs = new StringReader(instructionSection);
        
        Instruction subEntryPoint = null;
        Instruction subLeavePoint = null;
        
        while ((line = rs.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            
            Instruction instruction = reader.getInstruction(sender,line);
            if (instruction != null)
            {
                instructions.Add(instruction);
            }
        }
        return instructions;
    }
}